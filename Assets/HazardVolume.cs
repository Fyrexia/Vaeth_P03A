using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardVolume : MonoBehaviour
{
    [SerializeField] GameObject visualsToDeactivate = null;

    Collider colliderToDeactivate = null;
   

    private void Awake()
    {
        colliderToDeactivate = GetComponent<Collider>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player1
            = other.gameObject.GetComponent<PlayerController>();

        if (player1 != null)
        {
            player1.UpdateHealth(-1);
            DisableObject();
           
        }

       

    }

    public void DisableObject()
    {
        colliderToDeactivate.enabled = false;
        visualsToDeactivate.SetActive(false);
    }
}
