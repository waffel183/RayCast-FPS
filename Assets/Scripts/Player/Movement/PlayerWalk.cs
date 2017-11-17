using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour {

    public float speed;
    public float sensitivity;
    public CharacterController player;

    public GameObject playerCamera;

    float moveFB;
    float moveLR;

    float rotX;
    float rotY;

    public float jumpForce;
    private bool hasJumped;
    private float vertVelocity;

    private void Start()
    {
        player = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Movement();

        if (Input.GetKeyDown("space"))
        {
            hasJumped = true;
        }
        ApplyGravity();
    }

    void Movement()
    {
        if (GameObject.Find("SniperHolder").GetComponent<Scope>().isScoped == true) sensitivity = 0.5f;
        else sensitivity = 2f;

        moveFB = Input.GetAxis("Vertical") * speed;
        moveLR = Input.GetAxis("Horizontal") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        Vector3 movement = new Vector3(moveLR, vertVelocity, moveFB);
        transform.Rotate(0, rotX, 0);
        playerCamera.transform.Rotate(-rotY, 0, 0);

        movement = transform.rotation * movement;
        player.Move(movement * Time.deltaTime);
    }

    void ApplyGravity()
    {
        if (player.isGrounded)
        {
            if (!hasJumped)
            {
                vertVelocity = Physics.gravity.y;
            }
            else
            {
                vertVelocity = jumpForce;
            }
        }
        else
        {
            vertVelocity += Physics.gravity.y * Time.deltaTime;
            vertVelocity = Mathf.Clamp(vertVelocity, -50f, jumpForce);
            hasJumped = false;
        }
    }
}
