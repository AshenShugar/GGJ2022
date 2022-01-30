using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControls : MonoBehaviour {
	public string SceneToPlayName = "IntroCutscene";

	// Start is called before the first frame update
	void Start ()
	{

	}

	public void PlayButton ()
	{
		SceneManager.LoadScene (SceneToPlayName);
	}

	public void QuitButton ()
	{
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
