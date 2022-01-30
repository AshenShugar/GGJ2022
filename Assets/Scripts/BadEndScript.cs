using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BadEndScript : MonoBehaviour
{
	[SerializeField]
	private AudioSource _as;
	private float _timer = 0;
	[SerializeField]
	private float FadeInTime = 4.0f;
	[SerializeField]
	private string MenuSceneName = "MenuScene";
	[SerializeField]
	private Image Img;

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
		
		float timer = 0;
		while (timer < _as.clip.length) {
			timer += Time.deltaTime;
			_as.volume = 0.25f + (0.75f * (timer / _as.clip.length));
			yield return null;
		}

		// yield return new WaitForSeconds (_as.clip.length);

		StartCoroutine (FadeIn ());

	}

	private IEnumerator FadeIn ()
	{
		float timer = 0;
		Color tmpColour = Img.color;
		while (timer < FadeInTime) {
			timer += Time.deltaTime;
			// Do fade in some more.
			tmpColour.a = timer / (FadeInTime * 0.5f);
			Img.color = tmpColour;

			yield return null;
		}
		SceneManager.LoadScene (MenuSceneName);
	}

}
