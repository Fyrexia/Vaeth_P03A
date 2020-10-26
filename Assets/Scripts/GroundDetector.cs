using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GroundDetector : MonoBehaviour
{

    public event Action GroundDetected = delegate { };
    public event Action GroundVanished = delegate { };
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ice" || other.tag == "IceWall")
        {


            Debug.Log("ground entered");
            GroundDetected?.Invoke();
        }

    }

    private void OnTriggerStay(Collider other)
    {
      if(other.tag=="Ice" || other.tag == "IceWall")
        GroundDetected?.Invoke();


    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("ground vanished");
        GroundVanished?.Invoke();

    }



}
