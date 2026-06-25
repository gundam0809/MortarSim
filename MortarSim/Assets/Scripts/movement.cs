using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public class movement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    public float speed = 3f;
    public float turnSpeed = 1f;
    public Rigidbody rb;
    public float currentYRotation;
    Vector3 currentRotation;
    public float horizontatInput;
    public Rigidbody Rb;
    public Transform orientation;
    Vector3 moveDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    void Movement()
        
    {


        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            horizontatInput = Input.GetAxisRaw("Horizontal");
            moveDirection = orientation.forward * horizontatInput;
            Rb.AddForce(moveDirection.normalized * moveSpeed * -5, ForceMode.Force);

            //clamp the velocity

            Debug.Log("Crabs games are good");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {

            transform.Rotate(0, 5 * turnSpeed, 0);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Rotate(0, -5 * turnSpeed, 0);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

            transform.Rotate(5 * turnSpeed, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Rotate(-5 * turnSpeed, 0, 0);
        }

    }
}
