using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class BadEndScript : MonoBehaviour
{
	[SerializeField]
	private AudioSource _as;
	private float _timer = 0;
	[SerializeField]
	private float FadeInTime = 4.0f;
	[SerializeField]
	private string MenuSceneName = "MenuScene";

    // Start is called before the first frame update
    void Start()
    {
		_as.Play ();
		_as.SetScheduledEndTime (AudioSettings.dspTime + _as.clip.length - 1.0f);
		_timer = _as.clip.length;

		StartCoroutine (SoundPlaying ());
    }

	private IEnumerator SoundPlaying ()
	{
		
		/* Was thinking about trying to increase the volume as it plays
		float timer = 0;
		while (timer < _as.clip.length) {
			timer += Time.deltaTime;
			_as.volume *= 1.1f;
			yield return null;
		}
		*/

		yield return new WaitForSeconds (_as.clip.length);

		StartCoroutine (FadeIn ());

	}

	private IEnumerator FadeIn ()
	{
		float timer = 0;
		while (timer < FadeInTime) {
			timer += Time.deltaTime;
			// Do fade in some more.
			yield return null;
		}
		SceneManager.LoadScene (MenuSceneName);
	}

}
