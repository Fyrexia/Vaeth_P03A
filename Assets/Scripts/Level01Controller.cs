using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text currentScoreTextView;
    int currentScore;


    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(5);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitLevel();
        }
    }

    public void IncreaseScore(int scoreIncrease)
    {
        currentScore += scoreIncrease;
        currentScoreTextView.text = "Score: " + currentScore.ToString();
    }



    public void ExitLevel()
    {
        int highscore = PlayerPrefs.GetInt("HighScore");
        if(currentScore>highscore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            Debug.Log("New high score: " + currentScore);
        }
        SceneManager.LoadScene("MainMenu");
    }
}
