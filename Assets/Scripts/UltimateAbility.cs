using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateAbility : MonoBehaviour
{

    [SerializeField] GameObject visualsToDeactivate = null;

    [SerializeField] GroundUltimateDetector groundDetector = null;

    [SerializeField] ParticleSystem snow = null;

    [SerializeField] AudioSource UltSoundSource = null;
    [SerializeField] AudioClip UltBlizzard = null;

    bool isGrounded = false;
    bool BeginFreeze = false;

    Vector3 TrackPos;
    Vector3 newPosition;
    
    Collider colliderToDeactivate = null;
    Rigidbody rb = null;
    float timerDecay=0f;
    bool startDecay = false;

    private void Awake()
    {
        colliderToDeactivate = GetComponent<Collider>();
        colliderToDeactivate.enabled = false;

        rb = GetComponent<Rigidbody>();
        UltSoundSource.clip = UltBlizzard;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        Enemy Enemy1
            = other.gameObject.GetComponent<Enemy>();

        if (Enemy1 != null && other.transform.tag == "Enemy")
        {
            //Debug.Log("Hitting " + other.transform.name);
            Enemy1.FreezeAdd();
        }



    }

    private void OnTriggerStay(Collider other)
    {
        Enemy Enemy1
          = other.gameObject.GetComponent<Enemy>();

        if (Enemy1 != null && other.transform.tag == "Enemy")
        {
            //Debug.Log("Hitting " + other.transform.name);
            Enemy1.FreezeAdd();
        }
    }

    private void Update()
    {
        /*   timerDecay += Time.deltaTime;
           if (timerDecay > .7f)
               Destroy(this.gameObject);*/
        // groundDetector.GroundDetected += OnGroundDetected;
        //Debug.Log(groundDetector.GroundDetected);

        OnGroundDetected();

        if (isGrounded == true && BeginFreeze == false)
        {


            BeginFreeze = true;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            TrackPos = this.transform.position;
            newPosition = this.transform.position;

            this.transform.rotation = Quaternion.identity;

        }

        if (BeginFreeze == true)
        {
            if (newPosition.y < TrackPos.y + 1.5)
            {
                newPosition.y += .03f;

                this.transform.position = newPosition;
            }
            else
            {
                Debug.Log("Beginning Freeze");
                colliderToDeactivate.enabled = true;
                startDecay = true;
                if (snow.isPlaying != true)
                {
                    snow.Play();
                    UltSoundSource.Play();
                }
            }
        }

        if (startDecay == true)
        {
            timerDecay += Time.deltaTime;
            if(timerDecay>4f)
            {
                DisableObject();
            }
        }

    }



  


    void OnGroundDetected()
    {
       
        isGrounded = groundDetector.GetGroundEntered();
        //Land?.Invoke();
    }










    public void DisableObject()
    {
        Destroy(this.gameObject);
        colliderToDeactivate.enabled = false;
        visualsToDeactivate.SetActive(false);
    }













}
