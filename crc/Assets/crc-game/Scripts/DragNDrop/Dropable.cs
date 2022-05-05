using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;

[RequireComponent(typeof(Collider2D)),
 RequireComponent(typeof(Draggable))]
public class Dropable : MonoBehaviour 
{
	public float autoMoveArrivalThreshold = 0.05f;
	
	public Action<Dropable> OnDropComplete = delegate {};
	public Action<Dropable> OnReturnComplete = delegate {};

	public Func<Dropable, bool> OnDropAccepted = delegate { return true; };
	public Func<Dropable, bool> OnDropRejected = delegate { return true; };

	List<Dropzone> dropZones = new List<Dropzone>();
	Dropzone targetDropzone = null;
	public Dropzone TargetDropzone { get { return targetDropzone; } }

	Collider2D _collider;
	Draggable _dragging;
	
	void Start()
	{
		_collider = GetComponent<Collider2D>();
		_dragging = GetComponent<Draggable>();

		foreach(Dropzone dz in GameObject.FindObjectsOfType<Dropzone>())
			dropZones.Add(dz);
		
		_dragging.OnDragStop = (_) => OnDragStop();
	}
	
	void OnDragStop()
	{
		Dropzone newTargetDropzone = null;

		foreach(Dropzone dropzone in dropZones)
		{
			if(dropzone.CanDrop(_collider)){
				newTargetDropzone = dropzone;
				break;
			}
		}
		
		if (newTargetDropzone != null &&
			newTargetDropzone.GetComponent<Square>().GetOwner() != TurnController.Instance.PlayerToMove &&
			newTargetDropzone.GetComponent<Square>().CurrentSize < GetComponent<Matryoshka>().GetSize()
			)
		{
			if(targetDropzone)
				targetDropzone.Lift();

			targetDropzone = newTargetDropzone;

			if(OnDropAccepted(this))
			{
				MoveToTarget.Go(
					gameObject, 
					targetDropzone.transform.position,
					autoMoveArrivalThreshold
				).OnArrival = (_) => FinishDrop();

				ClearDropzone(targetDropzone);

				targetDropzone.GetComponent<Square>().SetOwner(TurnController.Instance.PlayerToMove);
				targetDropzone.GetComponent<Square>().ChangeTileSize(GetComponent<Matryoshka>().GetSize());

				this.GetComponent<Draggable>().enabled = false;

				TurnController.Instance.ChangeTurn(
					TurnController.Instance.PlayerToMove == GameController.Instance.Player1 ? GameController.Instance.Player2 : GameController.Instance.Player1
				);
			}

		} else 
		{
			if(OnDropRejected(this))
			{
				MoveToTarget.Go(
					gameObject, 
					_dragging.OriginalPos,
					autoMoveArrivalThreshold
				).OnArrival = (_) => FinishReturn();
			}
		}
	}

	private void ClearDropzone(Dropzone _dropzone)
    {
		if (_dropzone.transform.childCount != 0)
			Destroy(_dropzone.gameObject.transform.GetChild(0).gameObject);
	}


	[PunRPC]
	bool FinishDrop()
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
