using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSInput))]
[RequireComponent(typeof(FPSMotor))]
public class PlayerController : MonoBehaviour
{



    FPSInput input = null;
    FPSMotor motor = null;

    int HP = 5;

    public bool IsPlaying = true;

    [SerializeField] float moveSpeed = .1f;
    [SerializeField] float turnSpeed = 6f;
    [SerializeField] float jumpStrength = 10f;

    [SerializeField] ParticleSystem MuzzleFlash = null;
    [SerializeField] AudioSource GunShot = null;
    [SerializeField] Text YouLose = null;
    [SerializeField] Image HealthGUI = null;


    private void Awake()
    {
        input = GetComponent<FPSInput>();
        motor = GetComponent<FPSMotor>();

    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }



    private void Update()
    {
        if (IsPlaying == true)
        {


            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed = .2f;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                moveSpeed = .1f;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Shoot!");
                MuzzleFlash.Play();
                GunShot.Play();
            }


        }

        if(HP==0)
        {
            IsPlaying = false;
            YouLose.text = "You Lose! Press Escape and click restart!";
        }

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
