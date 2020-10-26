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
    [SerializeField] TextMesh MeshCurrentScore;
    [SerializeField] TextMesh MeshHighscore;
    [SerializeField] AudioSource MusicPlayer = null;
    [SerializeField] AudioClip Music = null;
    public int currentScore;
    public int HighscoreFinish;
    public bool FinishLineCrossed = false;
    //public int PC1.HP = 5;
    public int HealthDif = 5;
    public PlayerController PC1;
    public bool EnemyIsPlaying = true;

    public void Awake()
    {
        MusicPlayer.clip = Music;
        MusicPlayer.Play();
    }


    private void Update()
    {
        MeshCurrentScore.text = "Score Achieved: " + currentScore;
        if (FinishLineCrossed == true)
        {
          
            int highscore = PlayerPrefs.GetInt("HighScore");
            MeshHighscore.text = "Current HighScore: " + highscore;
            Debug.Log(highscore);
            if (HighscoreFinish > highscore)
            {
                PlayerPrefs.SetInt("HighScore", HighscoreFinish);
                MeshHighscore.text = "Current HighScore: " + highscore;
                Debug.Log("New high score: " + highscore);
            }
        }
        //PC1.HP=PC1.ReturnHealth();
        //This controls the GUI Health
        if(PC1.HP <= HealthDif)
        {
            HealthChange(PC1.HP);
        }
        if(PC1.CheckIsPlaying()==true)
        {
            Cursor.visible = false;
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
            Debug.Log("hitting escape");
            //ExitLevel();
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            PauseMenu.SetActive(true);
            PC1.GetIsPlaying(false);
            EnemyIsPlaying = false;
          
        }
        //This controls the GUI Health
        HealthDif = PC1.HP;
    }

    public void LockGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PC1.GetIsPlaying(true);
        EnemyIsPlaying = true;
        
    }

    public bool GetEnemyIsPlaying()
    {
        return EnemyIsPlaying;
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
        Debug.Log(highscore);
        /*if(HighscoreFinish>highscore)
        {
            PlayerPrefs.SetInt("HighScore", HighscoreFinish);
            Debug.Log("New high score: " + HighscoreFinish);
        }*/
        SceneManager.LoadScene("MainMenu");
    }
}
