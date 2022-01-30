using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
	public enum Scene {
		GoodEnd,
		BadEnd,
		MainMenu
	}

    public static bool isPaused;

    public TextMeshProUGUI pauseLabel;

    // Start is called before the first frame update
    void Start()
    {
        pauseLabel.enabled = false;
    }

	public void ChangeScene (Scene targetScene)
	{
		if (targetScene == Scene.GoodEnd) {
			SceneManager.LoadScene ("MenuScene");
		} else if (targetScene == Scene.BadEnd) {
			SceneManager.LoadScene ("BadEnd");
		} else if (targetScene == Scene.MainMenu) {
			SceneManager.LoadScene ("GoodEnd");
		}
	}

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            //AudioListener.pause = true;

            pauseLabel.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            //AudioListener.pause = false;

            pauseLabel.enabled = false;
        }
    }
}
