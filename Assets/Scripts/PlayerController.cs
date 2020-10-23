using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSInput))]
[RequireComponent(typeof(FPSMotor))]
public class PlayerController : MonoBehaviour
{



    FPSInput input = null;
    FPSMotor motor = null;

    public int HP = 5;
    private bool isRolling = false;
    public bool IsPlaying = true;

    [SerializeField] float moveSpeed = .1f;
    [SerializeField] float turnSpeed = 6f;
    [SerializeField] float jumpStrength = 10f;
    [SerializeField] float DodgeTimer = 3f;
    [SerializeField] float DodgeSpeed = .3f;
    [SerializeField] float DodgeCooldown = 3f;
    private bool IsImmune = false;
 
    [SerializeField] Text YouLose = null;
    [SerializeField] Image HealthGUI = null;
    [SerializeField] Image RedBoxLeft = null;
    [SerializeField] Image RedBoxRight = null;
    float OGmoveSpeed = 0;
    //Dodge
    [SerializeField] Text CounterDodge;
    [SerializeField] RawImage BackgroundDodgeCounter;
    //sounds
    [SerializeField] AudioSource AudioMovement = null;
    [SerializeField] AudioSource AudioHits = null;
    [SerializeField] AudioClip AudioWalk = null;
    [SerializeField] AudioClip AudioSprint = null;
    [SerializeField] AudioClip AudioDodge = null;
    [SerializeField] AudioClip AudioDeath = null;
    [SerializeField] AudioClip AudioDamaged = null;

    private void Awake()
    {
        input = GetComponent<FPSInput>();
        motor = GetComponent<FPSMotor>();
        OGmoveSpeed = moveSpeed;
        CounterDodge.text = "";
        BackgroundDodgeCounter.enabled = false;
        RedBoxLeft.enabled = false;
        RedBoxRight.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;


    }

    private void Start()
    {
       
    }



    private void Update()
    {
        if (IsPlaying == true)
        {


            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W) == false && isRolling != true)
                {
                    //motor.Dodge();
                    moveSpeed = DodgeSpeed;
                    isRolling = true;
                    IsImmune = true;
                    DelayHelper.DelayAction(this, RegressSpeed, .5f);
                    AudioMovement.clip = AudioDodge;
                    AudioMovement.Play();

                }

            }
            else if (Input.GetKey(KeyCode.LeftShift) && isRolling != true)
            {
                moveSpeed = .2f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                moveSpeed = OGmoveSpeed;


            /*
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                //Debug.Log("Shoot!");
                //MuzzleFlash.Play();
                //Sound
                //GunShot.Play();
            }*/


        }

        if (HP == 0)
        {
            if (IsPlaying == true)
            {
                IsImmune = true;
                AudioHits.clip = AudioDeath;
                AudioHits.Play();
            }
            IsPlaying = false;
            YouLose.text = "You Lose! Press Escape and click restart!";
        }

        if (isRolling == true)
        {
            BackgroundDodgeCounter.enabled = true;
            DodgeTimer += Time.deltaTime;
            int DodgeTimerConverted = (int)DodgeTimer;
            DodgeTimerConverted = (int)DodgeCooldown - DodgeTimerConverted;
            CounterDodge.text = DodgeTimerConverted.ToString();
            if (DodgeTimer >= DodgeCooldown)
            {
                RollingCooldown();
            }
        }









    }

    void RegressSpeed()
    {
        moveSpeed = DodgeSpeed * .25f;
        DelayHelper.DelayAction(this, ChangeSpeedToNormal, .25f);

    }

    void ChangeSpeedToNormal()
    {
        IsImmune = false;
        moveSpeed = OGmoveSpeed;

    }

    void RollingCooldown()
    {
        isRolling = false;

        BackgroundDodgeCounter.enabled = false;

        DodgeTimer = 0f;
        CounterDodge.text = "";


    }

    public void UpdateHealth(int HPupdate)
    {
        if (IsImmune == false)
        {
            HP += HPupdate;
            if (HPupdate < 0)
            {
                AudioHits.clip = AudioDamaged;
                AudioHits.Play();
                RedBoxLeft.enabled = true;
                RedBoxRight.enabled = true;
                RedBoxLeft.CrossFadeAlpha(0, .5f,false);
                RedBoxRight.CrossFadeAlpha(0, .5f, false);
                DelayHelper.DelayAction(this,Resetboxes, .5f);
            }
            int HealhGUISize = HP * 100;
            HealthGUI.rectTransform.sizeDelta = new Vector2(HealhGUISize, 50);
        }

    }
    public int ReturnHealth()
    {
        return HP;
    }

    private void Resetboxes()
    {
        RedBoxLeft.CrossFadeAlpha(1, .001f, false);
        RedBoxRight.CrossFadeAlpha(1, .001f, false);
        RedBoxLeft.enabled = false;
        RedBoxRight.enabled = false;

    }


    public void GetIsPlaying(bool Check)
    {
        IsPlaying = Check;
    }


    private void OnEnable()
    {

        input.MoveInput += OnMove;
        input.RotateInput += OnRotate;
        input.JumpInput += OnJump;

    }

    private void OnDisable()
    {

        input.MoveInput += OnMove;
        input.RotateInput += OnRotate;
        input.JumpInput += OnJump;

    }

    void OnMove(Vector3 movement)
    {
        if (IsPlaying == true)
        {
            // Debug.Log("Move: " + movement);
            motor.Move(movement * moveSpeed);


            if (moveSpeed == .1f)
            {
                if (AudioMovement.isPlaying == false || AudioMovement.clip==AudioSprint && AudioMovement.isPlaying==true)
                {
                    if (AudioMovement.clip == AudioSprint)
                        AudioMovement.Stop();
                    AudioMovement.clip = AudioWalk;
                    AudioMovement.Play();
                }
            }
            else
            {
                if (AudioMovement.isPlaying == false || AudioMovement.clip == AudioWalk && AudioMovement.isPlaying == true)
                {
                    if (AudioMovement.clip == AudioWalk)
                        AudioMovement.Stop();
                    AudioMovement.clip = AudioSprint;
                    AudioMovement.Play();
                }
            }


        }
    }




    void OnRotate(Vector3 rotation)
    {
        if (IsPlaying == true)
        {
            //Debug.Log("Rotate: " + rotation);
            motor.Turn(rotation.y * turnSpeed);
            motor.Look(rotation.x * turnSpeed);
        }
    }


    void OnJump()
    {
        if (IsPlaying == true)
        {
            //Debug.Log("Jump!");
            motor.Jump(jumpStrength);
        }
    }




}
