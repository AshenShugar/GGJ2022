using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;


public class BigBadController : MonoBehaviour
{
	private Vector3 _targetDestination;
	public NavMeshAgent bigBad;
	public float SlowSpeed = 1.0f;
	public float FastSpeed = 10.0f;
	[SerializeField]
	private GameObject PlayerCharacter;
	private float distanceFromPlayer;
	public float PitchRange = 3.0f;
	public float HeartBeatUpdateTime = 1.0f;    // How often to calculate how far Big Bad is from player
	public float SafeDistanceFromPlayer = 10.0f;	// Any further than this and the heart beat stays the same.
	[SerializeField]
	private AudioMixer _HBMixer;
	[SerializeField]
	private AudioSource _HBSource;
	private GameManager _gm;

	public bool GameRunning = true;

	float[] HeartBeatTimeArray = {0f,0.7f,1.3f,1.8f,2.35f,2.8f,3.2f,3.6f,4.0f,4.4f,4.8f,5.2f };

	void Start ()
	{
		if (PlayerCharacter == null)
			PlayerCharacter = FindObjectOfType<PlayerMovement> ().gameObject;

		if(PlayerCharacter == null)
			Debug.Log ("Player doesn't exist!");

		if (_gm == null)
			_gm = FindObjectOfType<GameManager> ();
		
		StartCoroutine (HeartBeatControl2());
	}

	public void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.tag == "Player") {
			// Game over
			_gm.ChangeScene (GameManager.Scene.BadEnd);
		}
	}

	public IEnumerator HeartBeatControl ()
	{
		while (GameRunning) {
			distanceFromPlayer = (PlayerCharacter.transform.position - transform.position).magnitude;

			// adjust timing/volume of heartbeat sound and any visual effects that match it.
			if (distanceFromPlayer > SafeDistanceFromPlayer) {
				_HBSource.pitch = 1;
				_HBMixer.SetFloat ("pitchBend", 1 / _HBSource.pitch);
			} else {
				_HBSource.pitch = 1 + PitchRange - ((PitchRange * distanceFromPlayer) / SafeDistanceFromPlayer );
				_HBMixer.SetFloat ("pitchBend", 1 / _HBSource.pitch);
			}

			yield return new WaitForSeconds (HeartBeatUpdateTime);
		}
	}

	// Not the right way to do this, but I was interested in seeing if I could
	public IEnumerator HeartBeatControl2 ()
	{
		int index;
		while (GameRunning) {
			distanceFromPlayer = (PlayerCharacter.transform.position - transform.position).magnitude;

			if (distanceFromPlayer > SafeDistanceFromPlayer) {
				index = 0;
			} else {
				index = 10 - Mathf.RoundToInt (distanceFromPlayer);
			}
			_HBSource.time = HeartBeatTimeArray [index];
			_HBSource.Play ();
			_HBSource.SetScheduledEndTime (AudioSettings.dspTime + HeartBeatTimeArray [index + 1] - HeartBeatTimeArray [index]);

			yield return new WaitForSeconds( HeartBeatTimeArray [index + 1] - HeartBeatTimeArray [index] );
		}
	}

	public bool HasTarget {
		set {
			if (bigBad != null) {
				if (value)
					bigBad.speed = FastSpeed;
				else
					bigBad.speed = SlowSpeed;
			}
		}
	}

	public Vector3 Target {
		set {
				// if the target destination has a z value that doesn't match the navmesh, it won't move.
				_targetDestination = value;
				_targetDestination.z = 0;
				bigBad.SetDestination (_targetDestination);
				bigBad.speed = FastSpeed;
			}
	}

}
