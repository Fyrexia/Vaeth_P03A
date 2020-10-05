using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level01Controller : MonoBehaviour
{
    [SerializeField] Text currentScoreTextView;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Text Healthtxt;
    int currentScore;
    public int HealthScore = 5;
    public int HealthDif = 5;
    public PlayerController PC1;

    private void Update()
    {
        
        HealthScore=PC1.ReturnHealth();
        if(HealthScore <= HealthDif)
        {
            HealthChange(HealthScore);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(5);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            PC1.UpdateHealth(-1);
            HealthChange(HealthScore);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ExitLevel();
            Cursor.lockState = CursorLockMode.Confined;
            PauseMenu.SetActive(true);
            PC1.GetIsPlaying(false);
        }

        HealthDif = HealthScore;
    }

    public void LockGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PC1.GetIsPlaying(true);
    }

    public void IncreaseScore(int scoreIncrease)
    {
        currentScore += scoreIncrease;
        currentScoreTextView.text = "Score: " + currentScore.ToString();
    }

    public void HealthChange(int HealthChange)
    {
        HealthScore = HealthChange;
        Healthtxt.text = "Health: " + HealthScore.ToString();
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
