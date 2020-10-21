using UnityEngine;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    //Stores Player Info
    public GameObject Player = null;
    public Vector3 PlayerPos;
    //[SerializeField] GameObject ProjectionCube = null;
    [SerializeField] Text HitCircle;
    //Intergers and inside units
    public int Health = 5;
    bool IsFreezeOn = false;
    bool beginUnfreeze = false;
    private int frameUnFreeze = 0;
    private int frameStartUnFreeze = 0;
    private float NothingHitTimer = 0;

    public Rigidbody rb = null;
    //Shooting Info
    [SerializeField] GameObject ShootingArea;
    private Vector3 targetpos;
    private float distanceToTarget;
    private Vector3 targetDirection;
    private float targetSpeed;
    private Vector3 targetVelocity;
    public GameObject projectile;
    [SerializeField] float shootSpeed;
    [SerializeField] float ShootCoolDown=0;
    public bool targetingOn = false;
    private float timerShoot;




    
    //How many freezing hits does it take to hit FreezeTime
    [SerializeField] float FreezeLimit = 1.5f;
    //What the current Freeze is at
    [SerializeField] float FreezeContainer = 0f;
    //How much freezing is given
    [SerializeField] float FreezeDamage = .1f;
    //The rate the Enemy unfreezes
    [SerializeField] float Unfreeze = .1f;


    //Colliders and Visuals
    [SerializeField] Collider colliderToDeactivate1 = null;
    [SerializeField] Collider colliderToDeactivate2 = null;
    [SerializeField] Collider SphereCollider = null;
    [SerializeField] GameObject visualsToDeactivate = null;
    [SerializeField] GameObject IceCube = null;

    private Vector3 scaleChange, positionChange, ogScale, ogPosition;
    private void Awake()
    {
        IceCube.SetActive(false);
        EnableObject();
        ogScale = IceCube.transform.localScale;
        ogPosition = IceCube.transform.position;
        rb = Player.GetComponent<Rigidbody>();
        
    }

    private void Start()
    {

    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered");
            targetingOn = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Player is staying");
        targetingOn = true;
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player has left");
        targetingOn = false;
    }
    */


    private void Update()
    {
        if (Health >= 1)
        {
            //checks to see if they are frozen
            if (IsFreezeOn == true)
            {
                //Begins the unfreeze effect after 2 seconds
                if (frameStartUnFreeze == 0)
                {
                    DelayHelper.DelayAction(this, FreezeDelay, 2f);
                    frameStartUnFreeze = 1;
                }
                //Unfreezing and updates every unfreeze rate
                if (frameUnFreeze == 0)
                {
                    DelayHelper.DelayAction(this, UnfreezingAction, Unfreeze);
                    frameUnFreeze = 1;
                }
            }
            else
            {
                timerShoot += Time.deltaTime;
                targetVelocity = rb.velocity;
                //targetpos = Player.transform.position;
                targetpos = PlayerPos;
                //distanceToTarget = Vector3.Distance(SphereCollider.transform.position, Player.transform.position);
                distanceToTarget = Vector3.Distance(SphereCollider.transform.position, PlayerPos);
                if (timerShoot >= ShootCoolDown)
                {
                    // Get target info

                    ShootPlayer();
                }

            }
        }

        NothingHitTimer += Time.deltaTime;
        if (NothingHitTimer >= .6f)
        {
            HitCircle.color = Color.white;
            NothingHitTimer = 0f;
        }

    }



    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log("Health for " + this.transform.name + " is " + Health);

        HitCircle.color = Color.red;
        DelayHelper.DelayAction(this, swapToWhite, .7f);


        if (Health <= 0)
        {
            DisableObject();
            Debug.Log(this.transform.name + " is " + "dead");
        }
    }
    void swapToWhite()
    {
        HitCircle.color = Color.white;
    }


    public void DisableObject()
    {
        colliderToDeactivate1.enabled = false;
        colliderToDeactivate2.enabled = false;
        visualsToDeactivate.SetActive(false);
    }

    public void EnableObject()
    {
        colliderToDeactivate1.enabled = true;
        colliderToDeactivate2.enabled = true;
        visualsToDeactivate.SetActive(true);
    }


    public void FreezeAdd()
    {
        //Possible Noise here

        //Code



        if (FreezeContainer < FreezeLimit && beginUnfreeze != true)
        {
            //Debug.Log("Freezing of " + this.transform.name + "is at " + FreezeContainer + " of " + FreezeLimit + " Limit ");
            //adds the freezedamage to the container
            FreezeContainer += FreezeDamage;
            //turns on the icecube
            IceCube.SetActive(true);
            //If the cube is still under the limit, then make it bigger
            if (FreezeContainer < FreezeLimit)
            {
                scaleChange = new Vector3(0f, FreezeDamage, 0f);
                positionChange = new Vector3(0.0f, FreezeDamage / 2, 0.0f);

                IceCube.transform.localScale += scaleChange;
                IceCube.transform.position += positionChange;

                HitCircle.color = Color.blue;
                NothingHitTimer = 0f;

            }
            //If it goes over the limit, then begin the frozen and unfreeze mechanics
            if (FreezeContainer >= FreezeLimit)
            {
                Debug.Log(this.transform.name + " is Frozen!");
                FreezeContainer = FreezeLimit;
                IceAge();
            }
        }
        else
        {
            /*
            Debug.Log("was unable to freeze...");
            Debug.Log("FreezeContainer at " + FreezeContainer);
            Debug.Log("FreezeLimit is at: " + FreezeLimit);
            Debug.Log("beguinUnfreeze is: " + beginUnfreeze);
            Debug.Log("ISFreezeOn is: " + IsFreezeOn);*/
        }

    }

    //stops shooting, sets enemy hp to 1, makes the complete_freeze enemy sound and makes sure certain variables are off and on
    void IceAge()
    {
        //Play sound here

        //Turns on/off variables
        IsFreezeOn = true;
        beginUnfreeze = false;

        //Hp is set to 1
        Health = 1;

        //Shooting turns off


    }
    //causes a delay before unfreezing
    void FreezeDelay()
    {
        if (beginUnfreeze != true && FreezeContainer>0)
        {
            Debug.Log("turning on beginunfreeze");
            beginUnfreeze = true;
        }
    }

    //Once the delay is over, it begins to unfreeze and can't be stopped
    void UnfreezingAction()
    {
        if (beginUnfreeze == true)
        {
            scaleChange = new Vector3(0f, Unfreeze, 0f);
            positionChange = new Vector3(0.0f, Unfreeze / 2, 0.0f);

            IceCube.transform.localScale -= scaleChange;
            IceCube.transform.position -= positionChange;
            FreezeContainer -= Unfreeze;

            //Debug.Log("unfreezing at : " + FreezeContainer);
            if (FreezeContainer <= 0)
            {
                ResetIceCube();
            }

        }
        frameUnFreeze = 0;
    }

    //Makes sure all variable are reset and ready for any freezes
    void ResetIceCube()
    {
        Debug.Log("unfrozen");
        beginUnfreeze = false;
        IsFreezeOn = false;
        IceCube.SetActive(false);
        FreezeContainer = 0f;
        frameUnFreeze = 0;
        frameStartUnFreeze = 0;
        IceCube.transform.localScale = ogScale;
        IceCube.transform.position = ogPosition;

    }

    void ShootPlayer()
    {
        if (targetingOn == true && IsFreezeOn!=true)
        {
            float projectileTimeToTarget = distanceToTarget / shootSpeed;
            Vector3 projectedTarget = targetpos + targetVelocity * projectileTimeToTarget;
            GameObject bullet1 = Instantiate(projectile, ShootingArea.transform.position, Quaternion.identity);
            bullet1.transform.LookAt(projectedTarget);
            Rigidbody rb1 = bullet1.GetComponent<Rigidbody>();
            rb1.velocity = bullet1.transform.forward * shootSpeed;
           
        }

        timerShoot = 0f;

    }





}

