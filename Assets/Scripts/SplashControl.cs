using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SplashControl : MonoBehaviour
{
	public Sprite GGJ2022img;
	public Sprite GGJ2022Ausimg;
	public Sprite HouseOfHoraceimg;
	public Image splashImg;
	public string MenuSceneName = "MenuScene";
	public float delayBetweenImageChanges = 4;

    // Start is called before the first frame update
    void Start()
    {
		splashImg.sprite = GGJ2022img;

		StartCoroutine (GGJSplashDelay (delayBetweenImageChanges));
    }

	public IEnumerator GGJSplashDelay (float timer)
	{
		yield return new WaitForSeconds (timer);
		splashImg.sprite = GGJ2022Ausimg;
		StartCoroutine (GGJAusDelay (delayBetweenImageChanges));

	}
	public IEnumerator GGJAusDelay (float timer)
	{
		yield return new WaitForSeconds (timer);
		splashImg.sprite = HouseOfHoraceimg;
		StartCoroutine (HoHDelay (delayBetweenImageChanges));
	}

	public IEnumerator HoHDelay (float timer)
	{
		yield return new WaitForSeconds (timer);
		SceneManager.LoadScene (MenuSceneName);

	}

}
