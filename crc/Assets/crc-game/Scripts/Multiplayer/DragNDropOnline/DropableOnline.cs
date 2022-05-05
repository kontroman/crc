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

		_dragging.OnDragStop = (_) => OnDragStop();
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
			newTargetDropzone.GetComponent<Square>().GetOwner() != TurnControllerOnline.Instance.GetCurrentPlayer() &&
			newTargetDropzone.GetComponent<Square>().CurrentSize < GetComponent<Matryoshka>().GetSize()
			)
		{
			if(targetDropzone)
				targetDropzone.Lift();

			targetDropzone = newTargetDropzone; //сейвим viewID?

			if(OnDropAccepted(this))
			{
				MoveToTargetOnline.Go(
					gameObject,
					targetDropzone.transform.position,
					autoMoveArrivalThreshold
				).OnArrival = (_) => FinishDrop();

				//Не всегда успевает вызываться, если 2 игрока почти одновременно поставили фишки
				//Вариант решения - задержка после смены хода
				//Например показывать "Your move!"
				PhotonView.Get(this).RPC("ClearDropzone", RpcTarget.All, PhotonView.Get(targetDropzone).ViewID);

				PhotonView.Get(targetDropzone).RPC(
					"SetOwnerMultiplayer",
					RpcTarget.All,
					PhotonView.Get(TurnControllerOnline.Instance.GetCurrentPlayer()).ViewID
					);

				PhotonView.Get(targetDropzone).RPC(
					"ChangeTileSizeMultiplayer",
					RpcTarget.All,
					(int)GetComponent<Matryoshka>().GetSize()
					);

				//targetDropzone.GetComponent<Square>().SetOwner(TurnControllerOnline.Instance.GetCurrentPlayer());
				//targetDropzone.GetComponent<Square>().ChangeTileSize(GetComponent<Matryoshka>().GetSize());

				this.GetComponent<DraggableOnline>().enabled = false;

				TurnControllerOnline.Instance.ChangeMove();
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
	
	[PunRPC]
	private void ClearDropzone(int viewID)
    {
		var _dropzone = GetByViewID.Instance.GetById(viewID).gameObject.GetComponent<DropzoneOnline>();

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
