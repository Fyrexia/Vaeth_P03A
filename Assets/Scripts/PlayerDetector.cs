using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] Enemy Enemy1 = null;





    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered");
            Enemy1.targetingOn = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Player is staying");
        Enemy1.targetingOn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has left");
        Enemy1.targetingOn = false;
    }














}
