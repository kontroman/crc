using UnityEngine;
using System.Collections;
using System;

public class MoveToTargetOnline : MonoBehaviour 
{
	const float default_speed = 3.0f;
	const float default_arrival_threshold = 0.08f;
	
	public float arrivalThreshold = MoveToTargetOnline.default_arrival_threshold;
	
	public float speed = MoveToTargetOnline.default_speed;

	public static MoveToTargetOnline Go(
		GameObject toMove, 
		Vector3 pos, 
		float arrivalThreshold = default_arrival_threshold, 
		float speed = default_speed
	)
	{
		MoveToTargetOnline moveTo = toMove.GetComponent<MoveToTargetOnline>();

		if(moveTo == null)
			moveTo = toMove.AddComponent<MoveToTargetOnline>();

		moveTo.arrivalThreshold = arrivalThreshold;
		moveTo.speed = speed;
		moveTo.Go(pos);

		return moveTo;
	}
	
	public Func<MoveToTargetOnline, bool> OnArrival = delegate {return true;};
	
	Vector3 destination;
	
	bool inMotion = false;

	public void Go()
	{
		Go (destination);
	}

	public void Go(Vector3 pos)
	{
		if(Vector3.Distance(transform.position, pos) > arrivalThreshold)
		{
			destination = pos;
			inMotion = true;
		}

	}

	public void Wait()
	{
		inMotion = false;
	}
	
	void Update()
	{
		if(inMotion)
		{
			transform.position = Vector3.Lerp(transform.position, destination, speed * Time.fixedDeltaTime);
			
			if(Vector3.Distance(transform.position, destination) < arrivalThreshold)
			{
				transform.position = destination;
				inMotion = false;

				if(OnArrival(this))
					Destroy(gameObject.GetComponent<MoveToTargetOnline>());
			}
		}
	}
}
