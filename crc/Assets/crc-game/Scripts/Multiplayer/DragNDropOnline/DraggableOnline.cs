using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using Photon.Pun;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class DraggableOnline : MonoBehaviour, IPunObservable
{
	enum DragState { Idle, Dragging };

	public Action<DraggableOnline> OnDragStart = delegate { };
	public Action<DraggableOnline> OnDragStop = delegate { };

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
		if (!enabled || state != DragState.Dragging) return;

		state = DragState.Idle;
		OnDragStop(this);
	}

	void OnMouseDown()
	{
		if (!GameControllerOnline.Instance.GameInProgress) return;
		if (!enabled || state != DragState.Idle) return;
		if (gameObject.GetComponent<Matryoshka>().Owner != TurnControllerOnline.Instance.GetCurrentPlayer()) return;
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
		if (gameObject.GetComponent<Matryoshka>().Owner != TurnControllerOnline.Instance.GetCurrentPlayer()) return;
		if (!GetComponent<PhotonView>().IsMine) return;

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 currentPos = transform.position;

		currentPos.y = mousePos.y;
		currentPos.x = mousePos.x;

		transform.position = dragOffset + currentPos;
	}

	[PunRPC]
	public void SetParent(int viewID)
    {
		this.transform.SetParent(GetByViewID.Instance.GetById(viewID).gameObject.transform);
	}

	//этот метод вызывает не-мастер что бы что?
	[PunRPC]
	public void ChangeTransform(float x, float y, float z)
    {
		transform.localPosition = new Vector3(x,y,z);
    }

	//Эта штука отображает фишки на устройстве??????
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		if (stream.IsWriting)	//отправляет мастер
			stream.SendNext(new float[] { transform.position.x, transform.position.y, transform.position.z });

        if (stream.IsReading) //принимает не-мастер
        {
			//Vector3 recieved = (Vector3)stream.ReceiveNext();

			float[] recieved = (float[])stream.ReceiveNext();

			ChangeTransform(recieved[0], recieved[1], recieved[2]);
        }
		
    }
}
