using UnityEngine;
using UnityEngine.UI;
public class FireWeapon : MonoBehaviour
{
    [SerializeField] Camera cameraController;
    [SerializeField] Transform rayOrigin = null;
    [SerializeField] float RegshootDistance = 10f;
    [SerializeField] float IceGunDistance = 10f;
    [SerializeField] Light LightHit;
    [SerializeField] int weaponDamage = 1;
    [SerializeField] float IceGunCooldown = .25f;
    [SerializeField] float RegGunCooldown = .75f;
    [SerializeField] LayerMask HitLayer;
    [SerializeField] LayerMask HitLayerIce;
    //[SerializeField] Text HitCircle;
    //Icebullets
    [SerializeField] GameObject IceBullet = null;
    [SerializeField] float IceshootSpeed;
    [SerializeField] GameObject IceshootTarget=null;
    [SerializeField] float IceBulletDecay = .7f;
    //Icewall
    [SerializeField] IceWallGrowth IceWall = null;
    [SerializeField] float IceWallCooldown = 1f;
    bool CanIceWall = true;


    //float NothingHitTimer = 0f;

    [SerializeField] ParticleSystem MuzzleFlash = null;
    [SerializeField] AudioSource GunShot = null;

    RaycastHit rayHit; //object hit
    RaycastHit rayIceWallHit;
    //Swapmode 1 is Regular Gun
    //Swampode 0 is Ice Gun
    int SwapMode = 0;
    int FreezeShoot = 0;
    bool CanRegShoot = true;

    private void Update()
    {

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (FreezeShoot == 0)
            {
                DelayHelper.DelayAction(this, Shoot, IceGunCooldown);
                //Shoot();
                SwapMode = 0;
                FreezeShoot = 1;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            if (CanRegShoot == true)
            {
                DelayHelper.DelayAction(this, RegGunOnCooldown, RegGunCooldown);
                SwapMode = 1;
                Shoot();
                //Debug.Log("doing regular shot");
                CanRegShoot = false;
                //noises and particles
                MuzzleFlash.Play();
                //Sound
                GunShot.Play();
                
            }
            

        }
        if(Input.GetKey(KeyCode.F))
        {
            if (CanIceWall == true)
            {
                DelayHelper.DelayAction(this, IceWallOnCooldown, IceWallCooldown);
                IceWallMaker();
                CanIceWall = false;
            }
        }

        /*
        NothingHitTimer += Time.deltaTime;
        if (NothingHitTimer >= .6f)
        {
            HitCircle.color = Color.white;
            NothingHitTimer = 0f;
        }
        */

    }

    void RegGunOnCooldown()
    {

        CanRegShoot = true;
        //Debug.Log("no longer on cooldown = " + CanRegShoot);
    }

    void IceWallOnCooldown()
    {

        CanIceWall = true;
        //Debug.Log("no longer on cooldown = " + CanRegShoot);
    }


    void Shoot()
    {
        float tempDistance = 0f;

        Vector3 rayDirection = cameraController.transform.forward;
        //Regular Gun
        if (SwapMode == 1)
        {
            Debug.DrawRay(rayOrigin.position, rayDirection * RegshootDistance, Color.red, 1f);
            tempDistance = RegshootDistance;
        }
        //Ice Gun
        else if (SwapMode == 0)
        {
            //Debug.Log("Shooting the icerays");
            tempDistance = IceGunDistance;
        }

        if (Physics.Raycast(rayOrigin.position, rayDirection, out rayHit, tempDistance, HitLayer) && SwapMode==1)
        {
            Debug.Log("Hit " + rayHit.transform.name + " at " + rayHit.transform.position);
            //LightHit.transform.position = rayHit.point;
            if(SwapMode==1)
           


            if (rayHit.transform.tag == "Enemy")
            {
                //Apply Damage if Swampmode 1
                if (SwapMode == 1)
                {
                   


                    Enemy Turret0 = rayHit.transform.gameObject.GetComponent<Enemy>();
                    if (Turret0 != null)
                    {
                        //HitCircle.color = Color.red;
                        //DelayHelper.DelayAction(this, swapToWhite, .7f);
                        Turret0.TakeDamage(weaponDamage);
                    }
                }
            }
        }
        else
        {
            //Debug.Log("Miss");
        }

        //Apply Freeze if Swampmode 0
        if (SwapMode == 0)
        {
            //If I wanted to use raycast I would use this
            /*
            Debug.DrawRay(rayOrigin.position, rayDirection * IceGunDistance, Color.blue, 1f);
            Enemy Turret0 = rayHit.transform.gameObject.GetComponent<Enemy>();
            if (Turret0 != null)
            {
                HitCircle.color = Color.blue;
                NothingHitTimer = 0f;
                Debug.Log("Freezing...");
                Turret0.FreezeAdd();
            }
            */

            //However, to make it more of an aoe range like an ice sprayer, it's more effective to make an inisible bullet thats big
            //Debug.Log("Shooting the icerays");
            GameObject Icebullet1 = Instantiate(IceBullet, rayOrigin.transform.position, Quaternion.identity);
            Icebullet1.transform.LookAt(IceshootTarget.transform.position);
            Rigidbody rb1 = Icebullet1.GetComponent<Rigidbody>();
            rb1.velocity = Icebullet1.transform.forward * IceshootSpeed;
            Destroy(Icebullet1, IceBulletDecay);


        }


      

        if (SwapMode == 0)
            FreezeShoot = 0;
    }

    void IceWallMaker()
    {
        Vector3 rayDirection = cameraController.transform.forward;
        //Debug.Log("testing to see if wall will spawn");
        Debug.DrawRay(rayOrigin.position, rayDirection * 15f, Color.green, 3f);
        if (Physics.Raycast(rayOrigin.position, rayDirection, out rayIceWallHit, 15f, HitLayerIce))
        {
            IceWall.EnableObject();
            //Debug.Log("Hit " + rayIceWallHit.transform.name + " at " + rayIceWallHit.transform.position);
            IceWall.transform.position = rayIceWallHit.point;
            IceWall.transform.LookAt(rayOrigin.transform);

            IceWall.transform.eulerAngles = new Vector3
                (
                IceWall.transform.eulerAngles.x * 0,
                IceWall.transform.eulerAngles.y,
                IceWall.transform.eulerAngles.z * 0
                );

            //Debug.Log(IceWall.name + " spawned at " + IceWall.transform.position);
        }
    }












    /*
    void swapToWhite()
    {
        HitCircle.color = Color.white;
    }*/








}
