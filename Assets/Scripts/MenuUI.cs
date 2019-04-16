using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour {

    public GameObject forceField;
    public HordeBehaviour horde;
    public GameObject columnPairs;
    public CanvasGroup canvasGroup;
    public Image logo;

    public Text start;
    public Text help;

    public Text gameOver;
    public Text score;

    public Text selectedButton;

    private Color defaultColour;

    public bool invalid;
  

	// Use this for initialization
	void Start () {
        defaultColour = start.color;
        invalid = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HighlightText(Text text)
    {
        if(text = start)
        {
            start.color = Color.red;
            help.color = defaultColour;
            selectedButton = start;
        }
        else if(text = help)
        {
            help.color = Color.red;
            start.color = defaultColour;
            selectedButton = help;
            ShowHelp();
        }
    }

    public void RemoveTextHighlights()
    {
        start.color = defaultColour;
        help.color = defaultColour;
        selectedButton = null;
    }

    public void Select(Text text)
    {
        if ((text = start) && !invalid)
        {
            horde.StartHorde();
            invalid = true;
            StartCoroutine("FadeOut");
        }
    }

    public void ShowHelp()
    {

    }

    public void StartGame()
    {

        horde.StartHorde();
        StartCoroutine("FadeOut");

       
    }

    IEnumerator FadeOut()
    {
        invalid = true;
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        this.gameObject.SetActive(false);
    }

    IEnumerator FadeIn()
    {
        while (canvasGroup.alpha <= 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        invalid = false;
    }

    public void HandleTrigger()
    {
        if (!invalid)
        {
            if(logo.isActiveAndEnabled)
            {
                StartGame();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    public void EndGame(string numDefeated, string survivalTime, int finalScore)
    {
        horde.gameObject.SetActive(false);
        columnPairs.SetActive(false);

        gameOver.gameObject.SetActive(true);
        forceField.SetActive(false);
        logo.gameObject.SetActive(false);
        help.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
        score.text = "numDefeated: " + numDefeated + "\n" + "survivalTime: " + survivalTime;
        start.text = "Press the TRIGGER to restart";
        StartCoroutine("FadeIn");
    }
}
