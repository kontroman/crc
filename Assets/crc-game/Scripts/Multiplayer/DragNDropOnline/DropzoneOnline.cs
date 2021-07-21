using UnityEngine;
using System.Collections;
using Photon.Pun;

[RequireComponent(typeof(Collider2D))]
public class DropzoneOnline : MonoBehaviour, IPunObservable
{
	public Collider2D _collider;
	public DropableOnline _ref = null;

	void Start () 
	{
		_collider = GetComponent<Collider2D> ();
	}

	public System.Action<DropzoneOnline> OnDrop = delegate {};
	public System.Action<DropzoneOnline> OnLift = delegate {};

    public void Update()
    {
		PhotonView.Get(this).RPC("Check", RpcTarget.All);
    }

	[PunRPC]
	private void Check()
    {
		if (_ref != null)
			Debug.LogError(_ref.name);
	}

    public bool IsFull { get { return _ref != null; } }
	
	public bool CanDrop(Collider2D dropCollider)
	{
		return enabled 
			&& (dropCollider.bounds.Intersects(_collider.bounds) 
				||  _collider.bounds.Contains(dropCollider.transform.position));
	}

	public void Drop(DropableOnline obj)
	{
		if (!DeckOnline.Instance.AvailableTile(this)) return;

        _ref = obj;

		PhotonView.Get(FindObjectOfType<TestRPC>()).RPC("AddOpp", RpcTarget.All);

		DeckOnline.Instance.AddMarkToTile(this);

		OnDrop (this);
	}

	public void Lift()
	{
		_ref = null;
		OnLift (this);
	}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
		if (stream.IsWriting) {
			
		}
        if (stream.IsReading)
        {
			_ref = (DropableOnline)stream.ReceiveNext();
        }
    }
}
