using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider2D))]
public class Draggable : MonoBehaviour
{
	enum DragState {Idle, Dragging};

	public Action<Draggable> OnDragStart = delegate {};
	public Action<Draggable> OnDragStop = delegate {};

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
		if (!GameController.Instance.GameInProgress) return;
		if(!enabled || state != DragState.Idle)	return;
		if (gameObject.GetComponent<Matryoshka>().Owner != TurnController.Instance.PlayerToMove) return;

		dragOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
		dragOffset.z = 0f;

		state = DragState.Dragging;
		OnDragStart(this);
	}
	
	void OnMouseDrag()
	{
		if (!GameController.Instance.GameInProgress) return;
		if (!enabled) return;
		if(gameObject.GetComponent<Matryoshka>().Owner != TurnController.Instance.PlayerToMove) return;

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 currentPos = transform.position;

		currentPos.y = mousePos.y;
		currentPos.x = mousePos.x;

		transform.position = dragOffset + currentPos;
	}
}
