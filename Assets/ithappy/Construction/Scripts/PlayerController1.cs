using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    CharacterController playerController;

    Vector3 direction;

    public float speed = 2;
    public float jumpPower = 2;
    public float gravity = 2f;

    public float mousespeed = 2f;


    public float minmouseY = -45f;
    public float maxmouseY = 45f;

    float RotationY = 0f;
    float RotationX = 90.0f;

    // Use this for initialization
    void Start()
    {
        playerController = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float _horizontal = Input.GetAxis("Horizontal");
        float _vertical = Input.GetAxis("Vertical");

        if (playerController.isGrounded)
            direction = new Vector3(_horizontal, 0, _vertical);
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
            direction.y = jumpPower;
        direction.y -= gravity * Time.deltaTime;
        playerController.Move(playerController.transform.TransformDirection(direction * Time.deltaTime * speed));

        RotationX += Input.GetAxis("Mouse X") * mousespeed;

        RotationY -= Input.GetAxis("Mouse Y") * mousespeed;
        RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);

        this.transform.eulerAngles = new Vector3(RotationY, RotationX, 0);

    }
}