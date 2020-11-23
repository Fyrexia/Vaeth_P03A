using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundUltimateDetector : MonoBehaviour
{

    public bool GroundEntered = false;
    
    
    
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ice" || other.tag == "IceWall")
        {


            Debug.Log("ground entered ultimate");
            GroundEntered = true;
        }

    }

    public bool GetGroundEntered()
    {
        return GroundEntered;
    }


}
