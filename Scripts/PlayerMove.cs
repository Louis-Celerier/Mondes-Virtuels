using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float masse;
    public float accelMax;
    public float velocityMax;
    private Vector3 currentForce;
    private Vector3 acceleration;
    private Vector3 velocity;
    private Rigidbody rb;
    public bool isGrounded;
    private Collision currentCollision;
    public GameObject currentPlanet;

    // Start is called before the first frame update
    void Start()
    {
        currentForce = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        GameObject[] planetGos = GameObject.FindGameObjectsWithTag("Planet");
        currentForce = Vector3.zero;
        // Compute final gravity after gravity from all planets is applied
        foreach (var planetGo in planetGos)
        {
            float masseGo = planetGo.GetComponent<PlanetGravity>().masse;
            Vector3 posGo = planetGo.transform.position;
            float distance = Vector3.Distance(posGo, transform.position);
            if (currentPlanet == null 
                || Vector3.Distance(
                    currentPlanet.transform.position, 
                    transform.position) > distance)
            {
                currentPlanet = planetGo.gameObject;
            }
            // direction of the force (towards the planet)
            Vector3 direction = posGo - transform.position;
            // Add the force
            currentForce +=
                // Gravity
                (6.67f * masse * masseGo / (distance * distance))
                * direction.normalized;
            
        }
        acceleration += currentForce * Time.deltaTime;
        if (acceleration.magnitude >= accelMax)
        {
            acceleration = acceleration.normalized * accelMax;
        }

        if (isGrounded)
        {
            Vector3 direction = currentCollision.transform.position - transform.position;
            acceleration -= Vector3.Dot(acceleration, direction) * direction.normalized;
        }
        velocity += acceleration * Time.deltaTime;
        if (velocity.magnitude >= velocityMax)
        {
            velocity = velocity.normalized * velocityMax;
        }

        float step = speed * Time.deltaTime;

        if (isGrounded)
        {
            float horizontalMovement = Input.GetAxis("Horizontal") * step;
            float verticalMovement = Input.GetAxis("Vertical") * step;

            if (horizontalMovement > 0 || verticalMovement > 0)
            {
                Vector3 targetVelocity =
                    new Vector3(horizontalMovement, rb.velocity.y, verticalMovement);
                velocity += Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, 0.05f);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = velocity;

        Vector3 dir = transform.position - currentPlanet.transform.position;
        transform.up = Time.deltaTime * dir + (1 - Time.deltaTime) * transform.up;
        Debug.Log(velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        acceleration = Vector3.zero;
        velocity = Vector3.zero;
        isGrounded = true;
        currentCollision = collision;

    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        currentCollision = null;
    }
}
