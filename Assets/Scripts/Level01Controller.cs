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
    //public int PC1.HP = 5;
    public int HealthDif = 5;
    public PlayerController PC1;

    private void Update()
    {
        
        //PC1.HP=PC1.ReturnHealth();
        //This controls the GUI Health
        if(PC1.HP <= HealthDif)
        {
            HealthChange(PC1.HP);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            IncreaseScore(5);
        }
        /*
        if(Input.GetKeyDown(KeyCode.F))
        {
            PC1.UpdateHealth(-1);
            HealthChange(PC1.HP);
        }
        */
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //ExitLevel();
            Cursor.lockState = CursorLockMode.Confined;
            PauseMenu.SetActive(true);
            PC1.GetIsPlaying(false);
        }
        //This controls the GUI Health
        HealthDif = PC1.HP;
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

    //This controls the GUI Health
    public void HealthChange(int HealthChange)
    {
        PC1.HP = HealthChange;
        Healthtxt.text = "Health: " + PC1.HP.ToString();
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
