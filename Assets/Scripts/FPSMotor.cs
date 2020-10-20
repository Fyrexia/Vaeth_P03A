using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class FPSMotor : MonoBehaviour
{
    [SerializeField] Camera camera = null;
    [SerializeField] float cameraAngleLimit = 70f;

    private float currentCameraRotationX = 0;

    Rigidbody rigidbody = null;

    [SerializeField] GroundDetector groundDetector = null;
    bool isGrounded = false;

    public event Action Land = delegate { };


    Vector3 movementThisFrame = Vector3.zero;
    float turnAmountThisFrame = 0;
    float lookAmountThisFrame = 0;
    //[SerializeField] float PushAmt = 3;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        ApplyMovement(movementThisFrame);
        ApplyTurn(turnAmountThisFrame);
        ApplyLook(lookAmountThisFrame);
    }

    private void OnEnable()
    {
        groundDetector.GroundDetected += OnGroundDetected;
        groundDetector.GroundVanished += OnGroundVanished;
    }

    private void OnDisable()
    {
        groundDetector.GroundDetected -= OnGroundDetected;
        groundDetector.GroundVanished -= OnGroundVanished;
    }




    public void Move(Vector3 requestedMovement)
    {
        //Debug.Log("Move: " + requestedMovement);
        movementThisFrame = requestedMovement;
    }

    public void Turn(float turnAmount)
    {
        //Debug.Log("Move: " + turnAmount);
        turnAmountThisFrame = turnAmount;
    }

    public void Look(float lookAmount)
    {
        //Debug.Log("Move: " + lookAmount);
        lookAmountThisFrame = lookAmount;
    }

    public void Jump(float jumpForce)
    {
      
        if (isGrounded == false)
            return;

        //Debug.Log("jump!");
        rigidbody.AddForce(Vector3.up * jumpForce);
    }
    /*
    public void Dodge()
    {
        movementThisFrame = movementThisFrame * PushAmt;
        rigidbody.MovePosition(rigidbody.position + movementThisFrame);
        Debug.Log("dodging!");
    }
    */


    void ApplyMovement(Vector3 moveVector)
    {
        if (moveVector == Vector3.zero)
            return;

        rigidbody.MovePosition(rigidbody.position + moveVector);
        movementThisFrame = Vector3.zero;

    }

    void ApplyTurn(float rotateAmount)
    {
        if (rotateAmount == 0)
            return;

        Quaternion newRotation = Quaternion.Euler(0, rotateAmount, 0);
        rigidbody.MoveRotation(rigidbody.rotation * newRotation);
        turnAmountThisFrame = 0;
    }

    void ApplyLook(float lookAmount)
    {
        if (lookAmount == 0)
            return;

        currentCameraRotationX -= lookAmount;
        currentCameraRotationX = Mathf.Clamp
            (currentCameraRotationX, -cameraAngleLimit, cameraAngleLimit);

        camera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0, 0);

        lookAmountThisFrame = 0;
    }

    void OnGroundDetected()
    {
        isGrounded = true;
        Land?.Invoke();
    }

    void OnGroundVanished()
    {
        isGrounded = false;
    }

    

}
