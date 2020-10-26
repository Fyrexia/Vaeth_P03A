using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineChecker : MonoBehaviour
{
    private bool StartTimer;
    private float LavaStartTimer=0f;
    [SerializeField] GameObject LavaFlow = null;
    private RisingLava LavaCube = null;

    private void Awake()
    {
        LavaCube = LavaFlow.GetComponent<RisingLava>();
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerController player1
             = other.gameObject.GetComponent<PlayerController>();

        if(player1 !=null)
        {
            Debug.Log("start rising");
            StartTimer = true;

        }
    }

    private void Update()
    {
        if(StartTimer==true)
        {
            LavaStartTimer += Time.deltaTime;
            if(LavaStartTimer>=10f)
            {
                LavaCube.StartRising = true;
            }
        }
    }



}

