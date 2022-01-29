using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BigBadController : MonoBehaviour
{
	private Vector3 _targetDestination;
	public NavMeshAgent bigBad;

	void Start ()
	{
		Target = Vector3.zero;
	}

	public Vector3 Target {
		set {
			if (value == null) {
				bigBad.isStopped = true;
			}
			else {
				// if the target destination has a z value that doesn't match the navmesh, it won't move.
				_targetDestination = value;
				_targetDestination.z = 0;
				bigBad.SetDestination (_targetDestination);
			}
		}
	}

}
