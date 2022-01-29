using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool isPaused;

    public TextMeshProUGUI pauseLabel;

    // Start is called before the first frame update
    void Start()
    {
        pauseLabel.enabled = false;
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
