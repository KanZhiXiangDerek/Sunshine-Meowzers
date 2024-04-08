using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References"), Space(10)]
    [SerializeField] private Camera cam;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;

    [Header("Drag And Shoot Stats"), Space(10)]
    [SerializeField] private float projectileSpeed;
    private float dragDistance;
    [SerializeField] float minProjectileSpd;
    [SerializeField] float maxProjectileSpd;

    [SerializeField] float xShootScale;
    [SerializeField] float yShootScale;

    [Header("GravityScale"), Space(10)]
    [SerializeField] float gravityScale;
    [SerializeField] float maxGravityScale;
    [SerializeField] float gravityGainSpeed = 2.5f;
    [SerializeField] float groundCheckRadius = 0.5f;
    [SerializeField] bool isPlayerGainGravity;
    [SerializeField] bool isGrounded;

    [Header("Others"), Space(10)]
    Vector2 mousePos;
    Vector2 startPoint;
    Vector2 endPoint;


    void Start()
    {
        gravityScale = maxGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (isGrounded)
        {
            rb.gravityScale = 0;
            gravityScale = 0;
            isPlayerGainGravity = false;
        }
        else
        {
            isPlayerGainGravity = true;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log("Drag");
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            if (isGrounded)
            {
                rb.velocity = rb.velocity * 0.1f;
                Debug.Log("SHOOT");
                Vector2 direction = (endPoint - startPoint).normalized; // Calculate the direction vector
                Vector2 adjustedDir = new Vector2(direction.x * xShootScale, direction.y * yShootScale);
                dragDistance = Vector2.Distance(endPoint, startPoint);
                //float magnitude = (endPoint - startPoint).magnitude;
                projectileSpeed = dragDistance * 2.5f;
                projectileSpeed = Mathf.Clamp(projectileSpeed, minProjectileSpd, maxProjectileSpd);
                rb.AddForce(-adjustedDir * projectileSpeed, ForceMode2D.Impulse);
                StartCoroutine(CounterForce(adjustedDir, projectileSpeed));
                //ReduceGravity();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isPlayerGainGravity)
        {
            rb.AddForce(Physics2D.gravity * gravityScale, ForceMode2D.Force);
            gravityScale += Mathf.Pow(gravityGainSpeed, 6 / 2.5f) * Time.fixedDeltaTime;
            if (gravityScale >= maxGravityScale)
            {
                gravityScale = maxGravityScale;
                isPlayerGainGravity = false;
            }
        }
    }
    IEnumerator CounterForce(Vector2 dir, float force)
    {
        float currentCounterForce = force * 0.3f;
        yield return new WaitForSeconds(0.25f);
        Debug.Log(force + " and " + currentCounterForce);
        if(!isGrounded)
        rb.AddForce(dir * currentCounterForce, ForceMode2D.Impulse);
        yield return null;
    }

    //private void ReduceGravity()
    //{
    //    gravityScale = 0f;
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = Vector2.zero;
            Debug.Log("Collide With Ground");
        }
    }
}

