using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWallGrowth : MonoBehaviour
{
    Vector3 CurrentPos;
    Vector3 FuturePos;
    Vector3 scaleChange;
    Vector3 positionChange;
    Vector3 OGscale;
    
    
    bool BeginChange=false;
    bool ResetScale = false;
    bool StartDecay = false;
    bool EndChange = false;
    float TimerDecay = 0f;
    int TimerBuilding = 0;
    int TimerDescaling = 0;
    [SerializeField] float TimeTillDecay = 3f;
    [SerializeField] Collider colliderToDeactivate1 = null;
    [SerializeField] GameObject visualsToDeactivate = null;

    //sounds
    private AudioSource Audiowall = null;
    [SerializeField] AudioClip AudioWallFormed = null;
    [SerializeField] AudioClip AudioWalldestroyed = null;


    private void Awake()
    {
        Audiowall = this.GetComponent<AudioSource>();
        FuturePos = this.transform.position;
        CurrentPos = this.transform.position;
        OGscale = this.transform.localScale;
      
    }
    void Update()
    {
        if(BeginChange==false)
        FuturePos = this.transform.position;
        if(CurrentPos!=FuturePos)
        {
            Debug.Log("it moved!");
            
            BeginChange = true;
            if(BeginChange==true)
            {
                if(ResetScale==false)
                {
                    Audiowall.volume = 1f;
                    Audiowall.clip = AudioWallFormed;
                    Audiowall.Play();
                    this.transform.localScale = OGscale;
                    Debug.Log("enable the object");
                    EnableObject();
                    ResetScale = true;
                }
                if (TimerBuilding < 100)
                {
                    scaleChange = new Vector3(0f, .008f, 0f);
                    positionChange = new Vector3(0.0f, .008f / 2, 0.0f);

                    this.transform.localScale += scaleChange;
                    this.transform.position -= positionChange;
                    TimerBuilding++;
                }
                else
                {
                  
                    StartDecay = true;
                }
            }
           
        }
        if(StartDecay==true)
        {
            if(TimerDescaling==1)
            {
                Audiowall.volume = .1f;
                Audiowall.clip = AudioWalldestroyed;
                Audiowall.Play();
            }
            TimerDecay += Time.deltaTime;
            if(TimerDecay>=TimeTillDecay)
            {
                Debug.Log("decaying..");
                if (TimerDescaling < 100)
                {
                    scaleChange = new Vector3(0f, .008f, 0f);
                    positionChange = new Vector3(0.0f, .008f / 2, 0.0f);

                    this.transform.localScale -= scaleChange;
                    this.transform.position += positionChange;
                    TimerDescaling++;
                }
                else
                {
                    DisableObject();
                    
                    StartDecay = false;
                    this.transform.localScale = OGscale;
                    BeginChange = false;
                    ResetScale = false;
                    FuturePos = this.transform.position;
                    CurrentPos = FuturePos;
                    TimerDecay = 0f;
                    TimerBuilding = 0;
                    TimerDescaling = 0;
                }
            }
        }
    }

    public void DisableObject()
    {
        colliderToDeactivate1.enabled = false;
        
        visualsToDeactivate.SetActive(false);
    }

    public void EnableObject()
    {
        colliderToDeactivate1.enabled = true;
       
        visualsToDeactivate.SetActive(true);
    }



}
