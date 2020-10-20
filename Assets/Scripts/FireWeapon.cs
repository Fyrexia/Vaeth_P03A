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
    //[SerializeField] Text HitCircle;
    [SerializeField] GameObject IceBullet = null;
    [SerializeField] float IceshootSpeed;
    [SerializeField] GameObject IceshootTarget=null;
    [SerializeField] float IceBulletDecay = .7f;

    //float NothingHitTimer = 0f;

    [SerializeField] ParticleSystem MuzzleFlash = null;
    [SerializeField] AudioSource GunShot = null;

    RaycastHit rayHit; //object hit
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
                Shoot();
                //Debug.Log("doing regular shot");
                CanRegShoot = false;
                //noises and particles
                MuzzleFlash.Play();
                //Sound
                GunShot.Play();
            }
            SwapMode = 1;

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

    void Shoot()
    {
        float tempDistance = 0f;

        Vector3 rayDirection = cameraController.transform.forward;
        //Regular Gun
        if (SwapMode == 1)
        {

            tempDistance = RegshootDistance;
        }
        //Ice Gun
        if (SwapMode == 0)
        {
            //Debug.Log("Shooting the icerays");
            tempDistance = IceGunDistance;
        }

        if (Physics.Raycast(rayOrigin.position, rayDirection, out rayHit, tempDistance, HitLayer))
        {
            Debug.Log("Hit " + rayHit.transform.name + " at " + rayHit.transform.position);
            //LightHit.transform.position = rayHit.point;
            if(SwapMode==1)
            Debug.DrawRay(rayOrigin.position, rayDirection * RegshootDistance, Color.red, 1f);


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
            Debug.Log("Miss");
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
            Debug.Log("Shooting the icerays");
            GameObject Icebullet1 = Instantiate(IceBullet, rayOrigin.transform.position, Quaternion.identity);
            Icebullet1.transform.LookAt(IceshootTarget.transform.position);
            Rigidbody rb1 = Icebullet1.GetComponent<Rigidbody>();
            rb1.velocity = Icebullet1.transform.forward * IceshootSpeed;
            Destroy(Icebullet1, IceBulletDecay);


        }


      

        if (SwapMode == 0)
            FreezeShoot = 0;
    }
    /*
    void swapToWhite()
    {
        HitCircle.color = Color.white;
    }*/








}
