using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int totalScore;
    public Text ScoreText;
    public Text DialogText1;
    public Text DialogText2;


    public GameObject GameOverPanel;
    public GameObject NextLevelPanel;

    public static GameController Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }
    public void updateTotalScore()
    {
        this.ScoreText.text = totalScore.ToString();
    }

    public void showGameOverPanel()
    {
        GameOverPanel.SetActive(true);
    }

    public void changeText()
    {
        DialogText1.gameObject.SetActive(false);
        DialogText2.gameObject.SetActive(true);
    }
    public void restartLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void showNextLevelPanel()
    {
            NextLevelPanel.SetActive(true);
    }

    public void goNextLevel(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
    }

}
