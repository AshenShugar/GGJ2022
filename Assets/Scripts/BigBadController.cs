using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BigBadController : MonoBehaviour
{
	private Vector3 _targetDestination;
	public NavMeshAgent bigBad;
	public float SlowSpeed = 1.0f;
	public float FastSpeed = 10.0f;
	[SerializeField]
	private GameObject PlayerCharacter;
	private float distanceFromPlayer;
	public float HeartBeatUpdateTime = 2.0f;	// How often to calculate how far Big Bad is from player
	void Start ()
	{
		StartCoroutine (HeartBeatControl());
	}

	public IEnumerator HeartBeatControl ()
	{
		distanceFromPlayer = (PlayerCharacter.transform.position - transform.position).magnitude;

		// adjust timing/volume of heartbeat sound and any visual effects that match it.

		yield return new WaitForSeconds(HeartBeatUpdateTime);
	}

	public Vector3 Target {
		set {
			if (value == null) {
				bigBad.isStopped = true;
				bigBad.speed = SlowSpeed;
			}
			else {
				// if the target destination has a z value that doesn't match the navmesh, it won't move.
				_targetDestination = value;
				_targetDestination.z = 0;
				bigBad.SetDestination (_targetDestination);
				bigBad.speed = FastSpeed;
			}
		}
	}

}
