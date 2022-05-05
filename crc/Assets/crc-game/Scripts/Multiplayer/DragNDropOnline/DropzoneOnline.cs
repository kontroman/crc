using UnityEngine;
using System.Collections;
using Photon.Pun;

[RequireComponent(typeof(Collider2D))]
public class DropzoneOnline : MonoBehaviour
{
	public Collider2D _collider;
	public DropableOnline _ref = null;

	void Start () 
	{
		_collider = GetComponent<Collider2D> ();
	}

	public System.Action<DropzoneOnline> OnDrop = delegate {};
	public System.Action<DropzoneOnline> OnLift = delegate {};

    public bool IsFull { get { return _ref != null; } }

    private void Update()
    {
        if (IsFull)
        {
			PhotonView.Get(_ref).RPC("ChangeTransform", RpcTarget.All, 0f, 0f, 0f);
		}
    }

    public bool CanDrop(Collider2D dropCollider)
	{
		return enabled 
			&& (dropCollider.bounds.Intersects(_collider.bounds) 
				||  _collider.bounds.Contains(dropCollider.transform.position));
	}

	public void Drop(DropableOnline obj)
	{
		if (!DeckOnline.Instance.AvailableTile(this)) return;

		PhotonView.Get(this).RPC("SetRef", RpcTarget.All, PhotonView.Get(obj).ViewID);
		//_ref = obj;

		StartCoroutine(SendTransformWithDelay());

		PhotonView.Get(FindObjectOfType<DeckOnline>()).RPC("AddMarkToTile", RpcTarget.All, PhotonView.Get(this).ViewID);

		OnDrop (this);
	}

	[PunRPC]
	private void SetRef(int viewID)
    {
		_ref = GetByViewID.Instance.GetById(viewID).gameObject.GetComponent<DropableOnline>();
    }

	IEnumerator SendTransformWithDelay()
    {
		PhotonView.Get(_ref).RPC("SetParent", RpcTarget.All, PhotonView.Get(this).ViewID);
		yield return new WaitForSeconds(0.1f);

	}

	public void Lift()
	{
		_ref = null;
		OnLift (this);
	}
}
