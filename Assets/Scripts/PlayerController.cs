using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Camera cam;
    private Vector3 fallVector;
    private float verticalVel;
    private bool isGrounded;
    private Vector3 moveVector;
    public Vector2 moveAxis;

    [SerializeField] float movementSpeed = 10;
    [SerializeField] float rotationSpeed = 0.1f;
    [SerializeField] float fallSpeed = .2f;
    [SerializeField] private float jumpForce = 100;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    void Update()
    {

        isGrounded = controller.isGrounded;

        if (isGrounded)
            verticalVel -= 0;
        else
            verticalVel -= 1;



        fallVector = new Vector3(0, verticalVel * fallSpeed * Time.deltaTime, 0);
        //moveVector = new Vector3(moveAxis.x, 0, moveAxis.y) * Time.deltaTime * movementSpeed;



        var forward = cam.transform.forward;
        var right = cam.transform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        float inputMagnitude = new Vector2(moveAxis.x, moveAxis.y).sqrMagnitude;
        moveVector = forward * moveAxis.y + right * moveAxis.x;

        if (inputMagnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVector), rotationSpeed);
        }
        controller.Move(fallVector + (moveVector * Time.deltaTime * movementSpeed));
    }


    #region Input
    void OnMove(InputValue value)
    {
        moveAxis = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (isGrounded)
            verticalVel = jumpForce;
    }

    #endregion

}
