using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GoodEndDialogManager : MonoBehaviour
{
	public TextMeshPro[] textDisplay;         //or TextMeshProUGUI;

	[TextArea (2, 10)]
	public string [] sentences;
	private int index;
	private int pageIndex = 0;
	public float typingSpeed;

	public Animator[] textDisplayAnim;

	private bool dialogStarted;

	public float initialDelay;
	public float backgroundFadeDuration = 2.0f;

	[SerializeField]
	private AudioSource _as;

	[SerializeField]
	private Sprite [] backGrounds;
	[SerializeField]
	private SpriteRenderer BGSpriteRenderer;

	// Start is called before the first frame update
	void Start ()
	{
		pageIndex = 0;

		BGSpriteRenderer.sprite = backGrounds [0];

		StartCoroutine (Type ());

		//StopAllCoroutines();
	}

	IEnumerator Type ()
	{
		if (!dialogStarted) {
			dialogStarted = true;
			yield return new WaitForSeconds (initialDelay);
		}

		for (int i = 0; i < sentences.Length; i++) {
			foreach (char letter in sentences [i].ToCharArray ()) {
				textDisplay [pageIndex].text += letter;
				yield return new WaitForSeconds (typingSpeed);
			}

			StartCoroutine (FadeText (pageIndex));

			pageIndex = pageIndex ^ 1;
		}

		StartCoroutine (ChangeBackgrounds ());

	}

	IEnumerator ChangeBackgrounds ()
	{
		float timer = 0;
		Color BGColour = BGSpriteRenderer.color;

		_as.time = 0;
		_as.Play ();

		// fade out
		while (timer < backgroundFadeDuration) {
			timer += Time.deltaTime;
			BGColour.a = 1 - (timer / backgroundFadeDuration);
			BGSpriteRenderer.color = BGColour;
			yield return null;
		}
		BGColour.a = 0;
		BGSpriteRenderer.color = BGColour;
		BGSpriteRenderer.sprite = backGrounds [1];

		//fade in
		timer = 0;
		while (timer < backgroundFadeDuration) {
			timer += Time.deltaTime;
			BGColour.a = (timer / backgroundFadeDuration);
			BGSpriteRenderer.color = BGColour;
			yield return null;
		}
		BGColour.a = 1;
		BGSpriteRenderer.color = BGColour;

		if(_as.isPlaying) {
			yield return new WaitForSeconds (_as.clip.length - _as.time);
		}

		BGSpriteRenderer.sprite = backGrounds [2];

		yield return new WaitForSeconds (0.5f);

		SceneManager.LoadScene ("MenuScene");

	}

	IEnumerator FadeText(int pageI)
	{
		yield return new WaitForSeconds (1.5f);
		textDisplayAnim [pageI].SetTrigger ("TextChange");
		yield return new WaitForSeconds (1.0f);

		textDisplayAnim [pageI].SetTrigger ("TextDefault");
		textDisplay [pageI].text = "";
	}




}
