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
    //[SerializeField] ParticleSystem MuzzleFlash = null;
    //[SerializeField] AudioSource GunShot = null;
    [SerializeField] Text YouLose = null;
    [SerializeField] Image HealthGUI = null;
    float OGmoveSpeed=0;

    private void Awake()
    {
        input = GetComponent<FPSInput>();
        motor = GetComponent<FPSMotor>();
        OGmoveSpeed = moveSpeed;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    private void Update()
    {
        if (IsPlaying == true)
        {

            if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W) == false && isRolling!=true)
                {
                    //motor.Dodge();
                    moveSpeed = DodgeSpeed;
                    isRolling = true;
                    DelayHelper.DelayAction(this, RegressSpeed, .25f);
                    
                }

            }
            else if (Input.GetKey(KeyCode.LeftShift) && isRolling!=true)
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

        if(HP==0)
        {
            IsPlaying = false;
            YouLose.text = "You Lose! Press Escape and click restart!";
        }

    }

    void RegressSpeed()
    {
        moveSpeed = DodgeSpeed*.25f;
        DelayHelper.DelayAction(this, ChangeSpeedToNormal, .25f);
    }

    void ChangeSpeedToNormal()
    {
        moveSpeed = OGmoveSpeed;
        DelayHelper.DelayAction(this, RollingCooldown, DodgeCooldown);
    }

    void RollingCooldown()
    {
        isRolling = false;
    }

    public void UpdateHealth(int HPupdate)
    {
       
        HP += HPupdate;
        int HealhGUISize = HP * 100;
        HealthGUI.rectTransform.sizeDelta= new Vector2(HealhGUISize, 50);

    }
    public int ReturnHealth()
    {
        return HP;
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
