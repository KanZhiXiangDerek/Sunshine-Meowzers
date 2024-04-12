using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References"), Space(10)]
    [SerializeField] private Camera cam;
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform rayCastPos;

    [Header("Drag And Shoot Stats"), Space(10)]
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private float projectileSpeed;
    private float dragDistance;
    [SerializeField] float minProjectileSpd;
    [SerializeField] float maxProjectileSpd;
    [SerializeField] float enemyDashMutipler = 4.0f;
    [SerializeField] Vector2 enemyCheckBox;
    [SerializeField] float enemyCheckRadius = 5.0f;
    [SerializeField] float counterForceScale = 0.3f;

    [SerializeField] float xShootScale;
    [SerializeField] float yShootScale;

    [Header("GravityScale"), Space(10)]
    [SerializeField] float gravityScale;
    [SerializeField] float maxGravityScale;
    [SerializeField] float gravityGainSpeed = 2.5f;
    float currentGravityGainSpeed;
    [SerializeField] float groundCheckRadius = 0.5f;
    //[SerializeField] bool isPlayerGainGravity;

    [SerializeField] bool isGrounded;
    [SerializeField] bool isAbleToExtraJump;
    [SerializeField] bool isNearEnemy;


    [Header("Others"), Space(10)]
    Vector2 mousePos;
    Vector2 startPoint;
    Vector2 currentPoint;
    Vector2 endPoint;

    bool canTimeSlow = true;


    float timeToNextTimeSlow = 0.5f;
    float currentTimeToNextSlow;

    bool canCounterForce;
    void Start()
    {
        currentGravityGainSpeed = gravityGainSpeed;
        gravityScale = maxGravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isNearEnemy = Physics2D.OverlapCircle(transform.position, enemyCheckRadius, whatIsEnemy);



        currentTimeToNextSlow -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);

            trajectory.ShowDot();
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (currentPoint - startPoint).normalized; // Calculate the direction vector
            float tempDragDistance = Vector2.Distance(currentPoint, startPoint);
            tempDragDistance = Mathf.Clamp(tempDragDistance, 6, 16);
            float currentForce = tempDragDistance * 2.5f;
            trajectory.UpdateDots(transform.position, (-direction * currentForce), 3f);

            if (isNearEnemy && currentTimeToNextSlow <= 0 && !isGrounded)
            {
                RaycastHit2D hit = Physics2D.BoxCast(rayCastPos.position, enemyCheckBox, 1f, -direction, whatIsEnemy);
                if (hit.collider != null)
                {
                    GameMan.instance.TimeSlow();
                    currentTimeToNextSlow = timeToNextTimeSlow;
                }

            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            trajectory.HideDot();
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (endPoint - startPoint).normalized; // Calculate the direction vector
            RaycastHit2D hit = Physics2D.BoxCast(rayCastPos.position, enemyCheckBox, 90f, -direction, enemyCheckRadius, whatIsEnemy);

            if (isGrounded || isAbleToExtraJump || isNearEnemy)
            {
                Vector2 adjustedDir;
                rb.velocity = rb.velocity * 0.01f;
                dragDistance = Vector2.Distance(endPoint, startPoint);
                Debug.DrawLine(startPoint, endPoint);
                projectileSpeed = dragDistance * 2.5f;
                projectileSpeed = Mathf.Clamp(projectileSpeed, minProjectileSpd, maxProjectileSpd);
                GameMan.instance.SlowDownLengthReduce(2f);

                gravityScale = 0;

                if (hit && isNearEnemy && !isGrounded)
                {
                    Vector2 enemyDirection = (transform.position - hit.transform.position).normalized; // Calculate the direction vector
                    Debug.Log(enemyDirection);
                    adjustedDir = enemyDirection;
                    projectileSpeed *= enemyDashMutipler;
                    projectileSpeed = Mathf.Clamp(projectileSpeed, 48f, 60f);
                    StartCoroutine(TempDisableCounterForce(0.8f));
                }
                else
                {
                    adjustedDir = new Vector2(direction.x * xShootScale, direction.y * yShootScale);
                }
                StartCoroutine(CounterForce(adjustedDir, projectileSpeed));
                rb.AddForce(-adjustedDir * projectileSpeed, ForceMode2D.Impulse);
            }
        }
    }

    private void FixedUpdate()
    {
        //if (isPlayerGainGravity)
        //{
        rb.AddForce(Physics2D.gravity * gravityScale, ForceMode2D.Force);
        gravityScale += Mathf.Pow(currentGravityGainSpeed, 6 / 2.5f) * Time.fixedDeltaTime;
        if (gravityScale >= maxGravityScale)
        {
            gravityScale = maxGravityScale;
            //isPlayerGainGravity = false;
        }
        //}
        //else
        //{
        //    gravityScale = 0;
        //}
    }
    IEnumerator CounterForce(Vector2 dir, float force)
    {
        float counterForce = force * counterForceScale;
        float timeDelay = 5f / force;
        timeDelay = Mathf.Clamp(timeDelay, 0.18f, 0.3f);

        yield return new WaitForSeconds(timeDelay);

        if (!isGrounded && canCounterForce)
        {
            rb.AddForce(dir * counterForce, ForceMode2D.Impulse);
        }
        yield return null;


    }

    IEnumerator TempDisableCounterForce(float timer)
    {
        canCounterForce = false;
        yield return new WaitForSeconds(timer);
        canCounterForce = true;
    }
    public void ExtraJump()
    {
        StartCoroutine(EnableExtraJump(1.0f));
    }
    private IEnumerator EnableExtraJump(float timePeriod)
    {
        isAbleToExtraJump = true;
        GameMan.instance.TimeSlow();
        yield return new WaitForSeconds(timePeriod);
        GameMan.instance.SlowDownLengthReduce(3f);
        isAbleToExtraJump = false;
    }

    public void SetGravityScale(float newGravityScale)
    {
        gravityScale = newGravityScale;
    }

    public void SetGravityGainSpeedOnGround()
    {
        currentGravityGainSpeed = gravityGainSpeed / 2;
    }

    public void SetGravityGainSpeedToOG()
    {
        currentGravityGainSpeed = gravityGainSpeed;
    }

    public bool GetIsGrounded()
    {
        return isGrounded;
    }

    public void TempExtendDisableCounterForce(float timer)
    {
        StartCoroutine(TempDisableCounterForce(timer));
    }
}