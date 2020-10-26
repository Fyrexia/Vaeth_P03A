using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineChecker : MonoBehaviour
{
    [SerializeField] Level01Controller LvController;
    [SerializeField] AudioClip FinishMusic;
    private AudioSource confetti = null;
    private void Awake()
    {
        confetti = this.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player1
           = other.gameObject.GetComponent<PlayerController>();

        if (player1 != null)
        {
            AudioSource Finisher = LvController.GetComponent<AudioSource>();
            Finisher.clip=FinishMusic;
            Finisher.Stop();
            Finisher.Play();
            confetti.Play();
            LvController.FinishLineCrossed = true;
            LvController.HighscoreFinish = LvController.currentScore;
        }
    }
}

