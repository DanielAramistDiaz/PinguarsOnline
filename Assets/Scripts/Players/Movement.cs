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

    public bool grounded = false;

    [Space]
    public GameObject camerasGO;
    public float inclinacionH;
    public float multiplicador;

    [Space]

    public bool isMetal;
    public bool isGrass;
    public bool isConcrete;
    public bool isOther;

    public AudioClip[] footStepsMetal;
    public AudioClip[] footStepsGrass;
    public AudioClip[] footStepsConcrete;
    public AudioClip[] footStepsOther;

    private AudioSource m_AudioSource;

    public GameObject pisando;
    public Vector3 posicionLaser;

    public enum CurrentFootStep
    {
        Metal,
        Grass,
        Concrete,
        Other
    }

    public CurrentFootStep footStepSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
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

        //cosas lasericas

        // Esta cosa sirve para que el rasho laser ignore cosas en una capa
        int layerMask = 1 << 2;

        /* La ~ esta wea que se saca con el coso derecho del espacio y pulsando el asterisco debajo del interrogador invertido
        sirve para invertir la operación, es decir que ahora solo detecta la capa 3*/
        layerMask = ~layerMask;

        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.1f, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
            pisando = hit.collider.gameObject;
            posicionLaser = hit.point;
            //Debug.Log("Le pego a algo");

        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 1, Color.red);
            pisando = this.gameObject;
            //Debug.Log("Le di al puro aire");

        }

        switch (pisando.gameObject.tag)
        {
            case "Metal":
                isMetal = true;
                isGrass = false;
                isConcrete = false;
                isOther = false;

                footStepSound = CurrentFootStep.Metal;
                break;

            case "Grass":
                isGrass = true;
                isMetal = false;
                isConcrete = false;
                isOther = false;

                footStepSound = CurrentFootStep.Grass;
                break;

            case "Concrete":
                isMetal = false;
                isGrass = false;
                isConcrete = true;
                isOther = false;

                footStepSound = CurrentFootStep.Concrete;
                break;

            

            default:
                isOther = true;
                isMetal = false;
                isGrass = false;
                isConcrete = false;

                footStepSound = CurrentFootStep.Other;
                break;
        }


        PlayFootStepAudio();
        
        
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

    private void PlayFootStepAudio()
    {
        if (grounded = true && rb.velocity.magnitude > 0.1f && !m_AudioSource.isPlaying)
        {
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, 2);


            switch (footStepSound)
            {
                case CurrentFootStep.Metal:
                    m_AudioSource.clip = footStepsMetal[n];
                    Debug.Log("sonido");
                    break;

                case CurrentFootStep.Grass:
                    m_AudioSource.clip = footStepsGrass[n];
                    break;

                case CurrentFootStep.Concrete:
                    m_AudioSource.clip = footStepsConcrete[n];
                    break;

                case CurrentFootStep.Other:
                    m_AudioSource.clip = footStepsOther[n];
                    break;

            }




            //m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time


            switch (footStepSound)
            {
                case CurrentFootStep.Metal:
                    footStepsMetal[n] = footStepsMetal[0];
                    footStepsMetal[0] = m_AudioSource.clip;
                    break;

                case CurrentFootStep.Grass:
                    footStepsGrass[n] = footStepsGrass[0];
                    footStepsGrass[0] = m_AudioSource.clip;
                    break;

                case CurrentFootStep.Concrete:
                    footStepsConcrete[n] = footStepsConcrete[0];
                    footStepsConcrete[0] = m_AudioSource.clip;
                    break;

                case CurrentFootStep.Other:
                    footStepsOther[n] = footStepsOther[0];
                    footStepsOther[0] = m_AudioSource.clip;
                    break;

            }

            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            return;
        }


    }
}
