using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingLava : MonoBehaviour
{
    public bool StartRising = false;
    [SerializeField] float RateOfRise = .01f;
    private Vector3 LavaPos;

    private void Awake()
    {
        LavaPos = this.transform.position;
    }




    void Update()
    {
        if(StartRising == true)
        {
            if (LavaPos.y < 115f)
            {
                LavaPos.y += RateOfRise;
                this.transform.position = LavaPos;
            }
           
        }
    }
}
