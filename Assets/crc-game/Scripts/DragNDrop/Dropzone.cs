using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class Dropzone : MonoBehaviour
{
	public Collider2D _collider;
	public Dropable _ref = null;

	void Start () 
	{
		_collider = GetComponent<Collider2D> ();
	}

	public System.Action<Dropzone> OnDrop = delegate {};
	public System.Action<Dropzone> OnLift = delegate {};

	public bool IsFull { get { return _ref != null; } }
	
	public bool CanDrop(Collider2D dropCollider)
	{
		return enabled 
			&& (dropCollider.bounds.Intersects(_collider.bounds) 
				||  _collider.bounds.Contains(dropCollider.transform.position));
	}

	public void Drop(Dropable obj)
	{
		if (!Deck.Instance.AvailableTile(this)) return;

        _ref = obj;

		Deck.Instance.AddMarkToTile(this);

		OnDrop (this);
	}

	public void Lift()
	{
		_ref = null;
		OnLift (this);
	}
}
