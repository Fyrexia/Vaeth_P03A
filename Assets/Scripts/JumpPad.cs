using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] float JumpPower = 700f;
    PlayerController player1=null;
    private void OnTriggerEnter(Collider other)
    {
        player1 = other.gameObject.GetComponent<PlayerController>();

        if (player1 != null)
        {
            Debug.Log("player is in the jumpad");
            player1.JumpPadActivation(JumpPower);
            


        }



    }

    private void OnTriggerExit(Collider other)
    {
        player1 = other.gameObject.GetComponent<PlayerController>();
        if (player1 != null)
        {
            player1.JumpPadLeave();
        }
    }


}
