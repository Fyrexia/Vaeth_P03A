using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] AudioClip _startingSong;
    [SerializeField] Text highScoreTextView;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore");
        highScoreTextView.text = highScore.ToString();
        
            if(_startingSong != null)
            {
                AudioManager.Instance.PlaySong(_startingSong);
            }
        
    }





}
