using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardVolume : MonoBehaviour
{
    [SerializeField] GameObject visualsToDeactivate = null;
    [SerializeField] float DisableAtTime = 0f;
    [SerializeField] bool BeDisabled = false;
    [SerializeField] int Damage = -1;
    private float Timer1 = 0f;
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
            Debug.Log("player has been hit");
            player1.UpdateHealth(Damage);
            if(BeDisabled==true)
            DisableObject();
           
        }
        else if(other.transform.tag == "IceWall" && BeDisabled==true || other.transform.tag == "Ice" && BeDisabled==true)
        {
            DisableObject();    
        }

        if(BeDisabled==true)
        {
            Timer1 += Time.deltaTime;
            if(Timer1>DisableAtTime)
            {
                DisableObject();
            }
        }
       

    }

    public void DisableObject()
    {
        colliderToDeactivate.enabled = false;
        visualsToDeactivate.SetActive(false);
    }
}
