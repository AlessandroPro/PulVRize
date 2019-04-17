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
    public Image helpPage;

    public Text start;
    public Text help;
    public Text restart;

    public Text gameOver;
    public Text score;

    public Text selectedButton;

    private Color defaultColour;

    public bool invalid;
    public bool gameEnded;
  

	// Use this for initialization
	void Start () {
        defaultColour = start.color;
        invalid = false;
        gameEnded = false;
	}
	
	// Update is called once per frame
	void Update () {
		
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
            canvasGroup.alpha += Time.deltaTime / 2;
            yield return new WaitForEndOfFrame();
        }
        invalid = false;
    }

    public void HandleTrigger()
    {
        if (!gameEnded && !invalid)
        {
            if(helpPage.gameObject.activeSelf)
            {
                helpPage.gameObject.SetActive(false);
            }
            else
            {
                helpPage.gameObject.SetActive(true);
            }
        }
    }

    public void HandleGripDown()
    {
        if (!invalid)
        {
            if (!gameEnded)
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
        gameEnded = true;

        horde.gameObject.SetActive(false);
        columnPairs.SetActive(false);

        GameObject[] particleObjs = GameObject.FindGameObjectsWithTag("Particle");
        foreach(GameObject particle in particleObjs)
        {
            if(particle)
            {
                Destroy(particle);
            }
        }

        gameOver.gameObject.SetActive(true);
        forceField.SetActive(false);
        logo.gameObject.SetActive(false);
        help.gameObject.SetActive(false);
        helpPage.gameObject.SetActive(false);
        start.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
        //restart.gameObject.SetActive(true);
        score.text = "Creatures Defeated:      " + numDefeated + "\n" + "Minutes Survived:        " + survivalTime + "\n\n\n";
        score.text = "FINAL SCORE:\n" + finalScore.ToString();
        StartCoroutine("FadeIn");
    }
}
