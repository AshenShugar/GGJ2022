using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class IntroDialog : MonoBehaviour
{
    public TextMeshPro textDisplay;         //or TextMeshProUGUI;

    [TextArea(2, 10)]
    public string[] sentences;
    private int index;
    public float typingSpeed;

    public Animator textDisplayAnim;

    private bool dialogStarted;

    public float initialDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Type());


        //StopAllCoroutines();
    }

    IEnumerator Type()
    {
        if (!dialogStarted)
        {
            dialogStarted = true;
            yield return new WaitForSeconds(initialDelay);
        }

        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(1.5f);

        textDisplayAnim.SetTrigger("TextChange");

        yield return new WaitForSeconds(1.0f);

        NextSentence();
    }

    public void NextSentence()
    {
        textDisplayAnim.SetTrigger("TextDefault");

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            textDisplay.text = "";

            if (SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1)
            { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); }//load next scene }
            else
            { SceneManager.LoadScene(0); }
        }
    }
}
