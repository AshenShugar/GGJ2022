using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {
	public string SceneToPlayName = "IntroCutscene";
	[SerializeField]
	private AudioSource _as;

	// Start is called before the first frame update
	void Start ()
	{

	}

	private IEnumerator SceneChange ()
	{
		yield return new WaitForSeconds (_as.clip.length);
		SceneManager.LoadScene (SceneToPlayName);
	}

	public void PlayButton ()
	{
		StartCoroutine (SceneChange ());
	}

	public void QuitButton ()
	{
		StartCoroutine (QuitAction ());
	}

	private IEnumerator QuitAction()
	{
		yield return new WaitForSeconds (_as.clip.length);

#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit ();
#endif
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
