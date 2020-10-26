using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong;
    [SerializeField] Text highScoreTextView;

    
    private int highScore;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreTextView.text = highScore.ToString();
        
            if(_startingSong != null)
            {
               // AudioManager.Instance.PlaySong(_startingSong);
            }
        
    }

    public void ResetData()
    {
        highScore = 0;
        highScoreTextView.text = highScore.ToString();
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
