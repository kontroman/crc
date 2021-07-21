using UnityEngine;
using System.Collections;
using System;
using Photon.Pun;

[RequireComponent(typeof(Collider2D))]
public class DraggableOnline : MonoBehaviour
{
	enum DragState {Idle, Dragging};

	public Action<DraggableOnline> OnDragStart = delegate {};
	public Action<DraggableOnline> OnDragStop = delegate {};

	DragState state = DragState.Idle;

	Vector3 originalPos = Vector3.zero;
	public Vector3 OriginalPos { get { return originalPos; } }

	Vector3 dragOffset = Vector3.zero;

	public void SetOriginalPos(Vector3 newOriginalPos)
	{
		originalPos = newOriginalPos;
	}

	void Start()
	{
		originalPos = transform.position;
	}
	
	void OnMouseUp()
	{
		if(!enabled || state != DragState.Dragging)	return;

		state = DragState.Idle;
		OnDragStop(this);
	}
	
	void OnMouseDown()
	{
		if (!GameControllerOnline.Instance.GameInProgress) return;
		if (!enabled || state != DragState.Idle)	return;
		if (gameObject.GetComponent<Matryoshka>().Owner != TurnControllerOnline.Instance.PlayerToMove) return;
		if (!GetComponent<PhotonView>().IsMine) return;

		dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		dragOffset.z = 0f;

		state = DragState.Dragging;
		OnDragStart(this);
	}
	
	void OnMouseDrag()
	{
		if (!GameControllerOnline.Instance.GameInProgress) return;
		if (!enabled) return;
		if (gameObject.GetComponent<Matryoshka>().Owner != TurnControllerOnline.Instance.PlayerToMove) return;
		if (!GetComponent<PhotonView>().IsMine) return;

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 currentPos = transform.position;

		currentPos.y = mousePos.y;
		currentPos.x = mousePos.x;

		transform.position = dragOffset + currentPos;
	}
}
