using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

[RequireComponent(typeof(Collider2D)),
 RequireComponent(typeof(DraggableOnline))]
public class DropableOnline : MonoBehaviourPun
{
	public float autoMoveArrivalThreshold = 0.05f;
	
	public Action<DropableOnline> OnDropComplete = delegate {};
	public Action<DropableOnline> OnReturnComplete = delegate {};

	public Func<DropableOnline, bool> OnDropAccepted = delegate { return true; };
	public Func<DropableOnline, bool> OnDropRejected = delegate { return true; };

	List<DropzoneOnline> dropZones = new List<DropzoneOnline>();
	DropzoneOnline targetDropzone = null;
	public DropzoneOnline TargetDropzone { get { return targetDropzone; } }

	Collider2D _collider;
	DraggableOnline _dragging;
	
	void Start()
	{
		_collider = GetComponent<Collider2D>();
		_dragging = GetComponent<DraggableOnline>();

		foreach(DropzoneOnline dz in GameObject.FindObjectsOfType<DropzoneOnline>())
			dropZones.Add(dz);

		_dragging.OnDragStop = (_) => this.photonView.RPC("OnDragStop", RpcTarget.All);//OnDragStop();
	}

	[PunRPC]
    void OnDragStop()
	{
		DropzoneOnline newTargetDropzone = null;

		foreach(DropzoneOnline dropzone in dropZones)
		{
			if(dropzone.CanDrop(_collider)){
				newTargetDropzone = dropzone;
				break;
			}
		}

		if (newTargetDropzone != null &&
			newTargetDropzone.GetComponent<Square>().GetOwner() != TurnControllerOnline.Instance.PlayerToMove &&
			newTargetDropzone.GetComponent<Square>().CurrentSize < GetComponent<Matryoshka>().GetSize()
			)
		{
			if(targetDropzone)
				targetDropzone.Lift();

			targetDropzone = newTargetDropzone;

			if(OnDropAccepted(this))
			{
				MoveToTargetOnline.Go(
					gameObject,
					targetDropzone.transform.position,
					autoMoveArrivalThreshold
				).OnArrival = (_) => FinishDrop();

				ClearDropzone(targetDropzone);

				targetDropzone.GetComponent<Square>().SetOwner(TurnControllerOnline.Instance.PlayerToMove);
				targetDropzone.GetComponent<Square>().ChangeTileSize(GetComponent<Matryoshka>().GetSize());

				this.GetComponent<DraggableOnline>().enabled = false;

				TurnControllerOnline.Instance.ChangeTurn(TurnControllerOnline.Instance.PlayerToMove == GameControllerOnline.Instance.Player1 ? GameControllerOnline.Instance.Player2 : GameControllerOnline.Instance.Player1);
			}

		} else 
		{
			if(OnDropRejected(this))
			{
				MoveToTargetOnline.Go(
					gameObject, 
					_dragging.OriginalPos,
					autoMoveArrivalThreshold
				).OnArrival = (_) => FinishReturn();
			}
		}
	}

	private void ClearDropzone(DropzoneOnline _dropzone)
    {
		if (_dropzone.transform.childCount != 0)
			Destroy(_dropzone.gameObject.transform.GetChild(0).gameObject);
	}


	[PunRPC]
	public bool FinishDrop()
	{
		targetDropzone.Drop(this);
		_dragging.SetOriginalPos(targetDropzone.transform.position);
		_dragging.transform.SetParent(targetDropzone.transform, true);
		OnDropComplete(this);
		return true;
	}

	bool FinishReturn()
	{
		OnReturnComplete(this);
		return true;
	}
}
