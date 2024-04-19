using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("References"), Space(10)]
    [SerializeField] private Camera cam;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform rayCastPos;
    [SerializeField] private PlayerSoundManager playerSM;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject strongDashEffect;


    [Header("Drag And Shoot Stats"), Space(10)]
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private float projectileSpeed;
    private float dragDistance;
    [SerializeField] float minProjectileSpd;
    [SerializeField] float maxProjectileSpd;
    [SerializeField] float enemyDashSpeed = 100f;
    [SerializeField] float extraJumpTime = 3.0f;
    [SerializeField] Vector2 enemyCheckBox;
    [SerializeField] float enemyCheckRadius = 5.0f;
    [SerializeField] float counterForceScale = 0.3f;

    [SerializeField] float xShootScale;
    [SerializeField] float yShootScale;

    [Header("GravityScale"), Space(10)]
    [SerializeField] float gravityScale;
    [SerializeField] float maxGravityScale;
    [SerializeField] float gravityGainSpeed = 2.5f;
    [SerializeField] float reduceGravityDivider = 4.0f;
    float currentGravityGainSpeed;
    [SerializeField] float groundCheckRadius = 0.5f;
    //[SerializeField] bool isPlayerGainGravity;

    [SerializeField] bool isGrounded;
    [SerializeField] bool isAbleToExtraJump;
    [SerializeField] bool isNearEnemy;
    [SerializeField] bool canCounterForce;
    [SerializeField] bool canTimeSlow = true;

    [Header("Others"), Space(10)]
    Vector2 mousePos;
    Vector2 startPoint;
    Vector2 currentPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector3 preJumpPos;

    float timeToNextTimeSlow = 1.0f;
    float currentTimeToNextSlow;
    float ogCamSize;


    private void Awake()
    {
        cam = Camera.main.GetComponent<Camera>();
        
    }
    void Start()
    {
        rb.velocity = Vector2.zero;
        currentGravityGainSpeed = gravityGainSpeed;
        gravityScale = maxGravityScale;
        StartCoroutine(PlayerStayInPos(transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isNearEnemy = Physics2D.OverlapCircle(transform.position, enemyCheckRadius, whatIsEnemy);

        if (!isGrounded)
        {
            float rotationSpeed = 10f + (projectileSpeed / 10);
            float offset = 90f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            playerSprite.transform.rotation = Quaternion.Slerp(playerSprite.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            if (playerSprite.transform.rotation.z >= 0.001f)
            {
                playerSprite.GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                playerSprite.GetComponent<SpriteRenderer>().flipX = false;
            }

        }
        else
        {
            float rotationSpeed = 10f;
            float offset = 0f;
            float angle = 0;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            playerSprite.transform.rotation = Quaternion.Slerp(playerSprite.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            playerAnim.ResetTrigger("IsLanding");
            playerAnim.SetTrigger("IsAiming");
            trajectory.ShowDot();

        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            preJumpPos = transform.position;
            currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = (currentPoint - startPoint).normalized; // Calculate the direction vector
            float tempDragDistance = Vector2.Distance(currentPoint, startPoint);
            tempDragDistance = Mathf.Clamp(tempDragDistance, 6, 16);
            float currentForce = tempDragDistance * 2.5f;
            trajectory.UpdateDots(transform.position, (-direction * currentForce), 3f);

            if (isNearEnemy && !isGrounded && canTimeSlow)
            {
                RaycastHit2D hit = Physics2D.BoxCast(rayCastPos.position, enemyCheckBox, 0f, -direction, enemyCheckRadius, whatIsEnemy);
                if (hit.collider != null)
                {
                    GameMan.instance.TimeSlow(0.25f);
                    canTimeSlow = false;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {

            trajectory.HideDot();
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = (endPoint - startPoint).normalized; // Calculate the direction vector
            RaycastHit2D hit = Physics2D.BoxCast(rayCastPos.position, enemyCheckBox, 0f, -direction, enemyCheckRadius, whatIsEnemy);
            playerAnim.ResetTrigger("IsAiming");
            playerAnim.SetTrigger("IsJumping");

            Vector2 adjustedDir;
            dragDistance = Vector2.Distance(endPoint, startPoint);
            Debug.DrawLine(startPoint, endPoint);
            projectileSpeed = dragDistance * 2.5f;
            projectileSpeed = Mathf.Clamp(projectileSpeed, minProjectileSpd, maxProjectileSpd);


            gravityScale = 0;

            if (hit.collider && isNearEnemy || hit.collider && isGrounded || hit.collider && isAbleToExtraJump)
            {

                isAbleToExtraJump = false;

                Vector2 enemyDirection = (transform.position - hit.transform.position).normalized; // Calculate the direction vector
                rb.velocity = rb.velocity * 0.01f;
                rb.AddForce(-enemyDirection * enemyDashSpeed, ForceMode2D.Impulse);
                playerSM.PlayerJumpSFX(new Vector3(preJumpPos.x, preJumpPos.y, -10));

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
                SpawnStrongDashEffect(rotation);
                GameMan.instance.ResetTimeScale();

            }
            else if (!hit && isGrounded || !hit && isAbleToExtraJump)
            {
                isAbleToExtraJump = false;
                adjustedDir = new Vector2(direction.x * xShootScale, direction.y * yShootScale);

              
                StartCoroutine(CounterForce(adjustedDir, projectileSpeed));
                rb.velocity = rb.velocity * 0.01f;
                rb.AddForce(-adjustedDir * projectileSpeed, ForceMode2D.Impulse);
                Debug.Log("Projectile Speed " + projectileSpeed);
                playerSM.PlayerJumpSFX(new Vector3(preJumpPos.x, preJumpPos.y, -10));

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
                SpawnDashEffect(rotation, 1 + (projectileSpeed / 10));
                GameMan.instance.ResetTimeScale();

            }

            canTimeSlow = true;
        }
    }

    private void FixedUpdate()
    {

        rb.AddForce(Physics2D.gravity * gravityScale, ForceMode2D.Force);
        gravityScale += Mathf.Pow(currentGravityGainSpeed, 6 / 2.5f) * Time.fixedDeltaTime;
        if (gravityScale >= maxGravityScale)
        {
            gravityScale = maxGravityScale;

        }

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

    IEnumerator TempDisableTimeSlow(float timer)
    {
        canTimeSlow = false;
        yield return new WaitForSeconds(timer);
        canTimeSlow = true;
        currentTimeToNextSlow = timeToNextTimeSlow;
    }
    public void ExtraJump()
    {
        StartCoroutine(EnableExtraJump(extraJumpTime));
    }
    private IEnumerator EnableExtraJump(float timePeriod)
    {
        isAbleToExtraJump = true;
        yield return new WaitForSeconds(timePeriod);
        isAbleToExtraJump = false;
    }

    private IEnumerator TempZeroGravity(float timePeriod)
    {
        gravityScale = 0;
        yield return new WaitForSeconds(timePeriod);
    }

    public void SetZeroGravity(float timePeriod)
    {
        StartCoroutine(TempZeroGravity(timePeriod));
    }

    public void SetGravityScale(float newGravityScale)
    {
        gravityScale = newGravityScale;
    }

    public void SetGravityGainSpeedOnGround()
    {
        currentGravityGainSpeed = gravityGainSpeed / reduceGravityDivider;
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

    public void AnimTrigger(string animTriggerName)
    {
        playerAnim.SetTrigger(animTriggerName);
    }

    public void ResetAnimTrigger(string animTriggerName)
    {
        playerAnim.ResetTrigger(animTriggerName);
    }

    public Quaternion GetPlayerRotation()
    {
        return playerSprite.transform.rotation;
    }
    public void SpawnDashEffect(Quaternion dir, float scaleMutipler)
    {
        scaleMutipler = Mathf.Clamp(scaleMutipler, 1.2f, 2.4f);
        GameObject effect = Instantiate(dashEffect, transform.position, dir);
        effect.transform.localScale = new Vector3(effect.transform.localScale.x * scaleMutipler, effect.transform.localScale.y * scaleMutipler, effect.transform.localScale.z);
        Destroy(effect, 3.0f);
    }

    public void SpawnStrongDashEffect(Quaternion dir)
    {
        GameObject effect = Instantiate(strongDashEffect, transform.position, dir);
        Destroy(effect, 3.0f);
    }

    public void ReflectForce(float force)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, Mathf.Infinity))
        {
            // Calculate reflection vector
            Vector2 reflection = Vector3.Reflect(transform.forward, hit.normal);

            // Apply force to the Rigidbody
            GetComponent<Rigidbody>().AddForce(reflection * force);
        }
    }

    public void PlayerToStayInSamePos(Vector2 pos)
    {
        StartCoroutine(PlayerStayInPos(pos));
    }
    IEnumerator PlayerStayInPos(Vector2 spawnPos)
    {
        float timer = 1.0f;
        timer -= Time.deltaTime;
        if (timer > 0)
        {
            transform.position = spawnPos;
        }
        yield return new WaitForSeconds(timer);
    }
}
