using UnityEngine;

public class EnemyPrediction : MonoBehaviour
{
    //Stores Player Info
    [SerializeField] GameObject Player = null;
    [SerializeField] GameObject ProjectionCube;
    //Intergers and inside units
    [SerializeField] int Health = 5;
    bool IsFreezeOn = false;
    bool beginUnfreeze = false;
    private int frameUnFreeze = 0;
    private int frameStartUnFreeze = 0;

    Rigidbody rb = null;
    Rigidbody TurretRb = null;

    //Shooting Info
    [SerializeField] GameObject ShootingArea;
    private Vector3 targetpos;
    private float distanceToTarget;
    private Vector3 interceptPoint;
    private Vector3 shooterVelocity;
    private Vector3 targetVelocity;
    public GameObject projectile;
    [SerializeField] float shootSpeed;
    [SerializeField] float ShootCoolDown = 0;
    private bool targetingOn = false;
    private float timerShoot;




    //How long they are Frozen
    [SerializeField] float FreezeTime = 3f;
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
        TurretRb = this.GetComponent<Rigidbody>();
    }

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetingOn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        targetingOn = false;
    }



    private void Update()
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
            Vector3 shooterPosition = ShootingArea.transform.position;
            Vector3 targetPosition = Player.transform.position;
            Vector3 shooterVelocity = TurretRb.velocity;
            Vector3 targetVelocity = rb.velocity;

            interceptPoint = FirstOrderIntercept(shooterPosition,shooterVelocity,shootSpeed,targetPosition,targetVelocity);

            if (timerShoot >= ShootCoolDown)
            {
                // Get target info

                ShootPlayer();
            }

        }
    }

    //first-order intercept using absolute target position
public static Vector3 FirstOrderIntercept
(
    Vector3 shooterPosition,
    Vector3 shooterVelocity,
    float shotSpeed,
    Vector3 targetPosition,
    Vector3 targetVelocity
)
{
    Vector3 targetRelativePosition = targetPosition - shooterPosition;
    Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
    float t = FirstOrderInterceptTime
    (
        shotSpeed,
        targetRelativePosition,
        targetRelativeVelocity
    );
    return targetPosition + t * (targetRelativeVelocity);
}
//first-order intercept using relative target position
public static float FirstOrderInterceptTime
(
    float shotSpeed,
    Vector3 targetRelativePosition,
    Vector3 targetRelativeVelocity
)
{
    float velocitySquared = targetRelativeVelocity.sqrMagnitude;
    if (velocitySquared < 0.001f)
        return 0f;

    float a = velocitySquared - shotSpeed * shotSpeed;

    //handle similar velocities
    if (Mathf.Abs(a) < 0.001f)
    {
        float t = -targetRelativePosition.sqrMagnitude /
        (
            2f * Vector3.Dot
            (
                targetRelativeVelocity,
                targetRelativePosition
            )
        );
        return Mathf.Max(t, 0f); //don't shoot back in time
    }

    float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
    float c = targetRelativePosition.sqrMagnitude;
    float determinant = b * b - 4f * a * c;

    if (determinant > 0f)
    { //determinant > 0; two intercept paths (most common)
        float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
        if (t1 > 0f)
        {
            if (t2 > 0f)
                return Mathf.Min(t1, t2); //both are positive
            else
                return t1; //only t1 is positive
        }
        else
            return Mathf.Max(t2, 0f); //don't shoot back in time
    }
    else if (determinant < 0f) //determinant < 0; no intercept path
        return 0f;
    else //determinant = 0; one intercept path, pretty much never happens
        return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
}

public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log("Health for " + this.transform.name + " is " + Health);
        if (Health == 0)
        {
            DisableObject();
            Debug.Log(this.transform.name + " is " + "dead");
        }
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
            Debug.Log("Freezing of " + this.transform.name + "is at " + FreezeContainer + " of " + FreezeLimit + " Limit ");
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
            Debug.Log("was unable to freeze...");
            Debug.Log("FreezeContainer at " + FreezeContainer);
            Debug.Log("FreezeLimit is at: " + FreezeLimit);
            Debug.Log("beguinUnfreeze is: " + beginUnfreeze);
            Debug.Log("ISFreezeOn is: " + IsFreezeOn);
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
        if (beginUnfreeze != true && FreezeContainer > 0)
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

            Debug.Log("unfreezing at : " + FreezeContainer);
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
        //float projectileTimeToTarget = distanceToTarget / shootSpeed;
        //Vector3 projectedTarget = targetpos + targetVelocity * projectileTimeToTarget;
        ProjectionCube.transform.position = interceptPoint;
        GameObject bullet1 = Instantiate(projectile, ShootingArea.transform.position, Quaternion.identity);
        bullet1.transform.LookAt(interceptPoint);
        Rigidbody rb = bullet1.GetComponent<Rigidbody>();
        rb.velocity = bullet1.transform.forward * shootSpeed;


        timerShoot = 0f;

    }





}

