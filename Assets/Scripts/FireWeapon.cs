using UnityEngine;
using UnityEngine.UI;
public class FireWeapon : MonoBehaviour
{
    [SerializeField] Camera cameraController;
    [SerializeField] Transform rayOrigin = null;
    [SerializeField] float RegshootDistance = 10f;
    [SerializeField] float IceGunDistance = 10f;

    [SerializeField] int weaponDamage = 1;
    [SerializeField] ParticleSystem GunHitImpact = null;
    [SerializeField] float IceGunCooldown = .25f;
    [SerializeField] float RegGunCooldown = .75f;
    [SerializeField] LayerMask HitLayer;
    [SerializeField] LayerMask HitLayerIce;
    //[SerializeField] Text HitCircle;
    //Icebullets
    [SerializeField] GameObject IceBullet = null;
    [SerializeField] float IceshootSpeed;
    [SerializeField] GameObject IceshootTarget = null;
    [SerializeField] float IceBulletDecay = .7f;
    //Icewall
    [SerializeField] IceWallGrowth IceWall = null;
    [SerializeField] float IceWallCooldown = 1f;
    [SerializeField] Text CounterIceWall;
    [SerializeField] RawImage BackgroundIceCounter;

    private float IceWallTimer = 0;
    bool CanIceWall = true;

    //Sounds and particles
    private AudioSource audioSwapGun = null;
    [SerializeField] AudioClip audioClipRegGun = null;
    [SerializeField] AudioClip audioClipFreezeGun = null;
    [SerializeField] ParticleSystem MuzzleFlash = null;
    [SerializeField] AudioSource GunShot = null;
    [SerializeField] ParticleSystem IceGunParticle = null;

    //Player controller
    private PlayerController PL = null;
  

    RaycastHit rayHit; //object hit
    RaycastHit rayIceWallHit;
    //Swapmode 1 is Regular Gun
    //Swampode 0 is Ice Gun
    int SwapMode = 0;
    int FreezeShoot = 0;
    bool CanRegShoot = true;

    public void Awake()
    {
        CounterIceWall.text = "";
        BackgroundIceCounter.enabled = false;
        audioSwapGun = GunShot.GetComponent<AudioSource>();
        audioSwapGun.clip = audioClipFreezeGun;
       
        PL = this.GetComponent<PlayerController>();


    }

    private void Update()
    {
      
        if (PL.CheckIsPlaying() == true)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
                IceGunParticle.Emit(0);

            if (Input.GetKey(KeyCode.Mouse0))
            {
                IceGunParticle.Emit(1);
                if (FreezeShoot == 0)
                {

                    DelayHelper.DelayAction(this, Shoot, IceGunCooldown);
                    //Shoot();
                    SwapMode = 0;
                    FreezeShoot = 1;

                    //play sound
                    if (audioSwapGun.isPlaying == false)
                    {
                        // ... play them.
                        if (audioSwapGun.clip != audioClipFreezeGun)
                            audioSwapGun.clip = audioClipFreezeGun;
                        audioSwapGun.Play();
                    }



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

                    if (audioSwapGun.clip != audioClipRegGun)
                        audioSwapGun.clip = audioClipRegGun;
                    audioSwapGun.Play();

                }


            }
            else
            {
                if (audioSwapGun.clip == audioClipFreezeGun)
                {
                    if (SwapMode == 0)
                        audioSwapGun.Stop();
                }
            }
            if (Input.GetKey(KeyCode.F))
            {
                if (CanIceWall == true)
                {
                    //DelayHelper.DelayAction(this, IceWallOnCooldown, IceWallCooldown);
                    IceWallMaker();
                    CanIceWall = false;

                    //Play sound


                }

            }

            if (CanIceWall == false)
            {
                BackgroundIceCounter.enabled = true;
                IceWallTimer += Time.deltaTime;
                int IceWallTimerConverted = (int)IceWallTimer;
                IceWallTimerConverted = (int)IceWallCooldown - IceWallTimerConverted;
                CounterIceWall.text = IceWallTimerConverted.ToString();
                if (IceWallTimer >= IceWallCooldown)
                {
                    IceWallOnCooldown();
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
    }

    void RegGunOnCooldown()
    {

        CanRegShoot = true;
        //Debug.Log("no longer on cooldown = " + CanRegShoot);
    }

    void IceWallOnCooldown()
    {
        BackgroundIceCounter.enabled = false;
        CanIceWall = true;
        IceWallTimer = 0f;
        CounterIceWall.text = "";
       
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

        if (Physics.Raycast(rayOrigin.position, rayDirection, out rayHit, tempDistance, HitLayer) && SwapMode == 1)
        {
            Debug.Log("Hit " + rayHit.transform.name + " at " + rayHit.transform.position);
            //LightHit.transform.position = rayHit.point;
            if (SwapMode == 1)



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
            GunHitImpact.transform.position = rayHit.point;
            GunHitImpact.Play();
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
        else
        {
            Debug.Log("icewall didn't hit anything..");
            IceWallTimer = IceWallCooldown;
            CanIceWall = true;


        }
    }












    /*
    void swapToWhite()
    {
        HitCircle.color = Color.white;
    }*/








}
