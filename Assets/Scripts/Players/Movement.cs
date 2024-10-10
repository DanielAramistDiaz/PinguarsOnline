using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float maxVelChange = 10f;
    [Space]
    public float airControl = 0.5f;

    [Space]
    public float jumpHeight = 5f;

    private Vector2 input;
    private Rigidbody rb;

    private bool sprinting;
    private bool jumping;

    private bool grounded = false;

    [Space]
    public GameObject camerasGO;
    public float inclinacionH;
    public float multiplicador;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input.Normalize();

        inclinacionH = Input.GetAxis("Horizontal");

        camerasGO.transform.localRotation = Quaternion.Euler(0, 0, inclinacionH * -1 * 2);
        //jumpaction condicional


        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");
        
    }

    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }

    private void FixedUpdate()
    {

        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpHeight, rb.velocity.z);
            }
            else if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : walkSpeed), ForceMode.VelocityChange);

            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.deltaTime, velocity1.y, velocity1.z * 0.2f * Time.deltaTime);
                rb.velocity = velocity1;
            }
        }
        else
        {
            if (input.magnitude > 0.5f)
            {
                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : walkSpeed * airControl), ForceMode.VelocityChange);

            }
            else
            {
                var velocity1 = rb.velocity;
                velocity1 = new Vector3(velocity1.x * 0.2f * Time.deltaTime, velocity1.y, velocity1.z * 0.2f * Time.deltaTime);
                rb.velocity = velocity1;
            }
        }

        grounded = false;
    }

    Vector3 CalculateMovement (float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x,0,input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);

        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if (input.magnitude > 0.5f)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelChange, maxVelChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelChange, maxVelChange);

            velocityChange.y = 0;

            return (velocityChange);
        }

        else
        {
            return new Vector3();
        }
    }
}
