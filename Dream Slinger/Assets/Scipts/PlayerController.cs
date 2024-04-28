using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MoreMountains.Feedbacks;
using System.Runtime.InteropServices;
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
    private Quaternion dustDir;

    [Header("Drag & Shoot"), Space(10)]
    [SerializeField] private Trajectory trajectory;
    [SerializeField] private float projectileSpeed;
    private float dragDistance;
    [SerializeField] float xShootScale;
    [SerializeField] float yShootScale;
    [SerializeField] float minProjectileSpd;
    [SerializeField] float maxProjectileSpd;
    [SerializeField] float enemyDashSpeed = 100f;
    [SerializeField] float extraJumpTime = 3.0f;
    [SerializeField] Vector2 enemyCheckBox;
    [SerializeField] float enemyCheckRadius = 5.0f;
    [SerializeField] float counterForceScale = 0.3f;
    [SerializeField] float jumpCD = 0.2f;
    float currentJumpCD;

    [Header("Player Feedback"), Space(10)]
    [SerializeField] MMF_Player playerPreJumpFeedback;
    [SerializeField] MMF_Player playerPreJumpChargeFeedback;
    [SerializeField] MMF_Player playerJumpFeedback;
    [SerializeField] MMF_Player playerPreEnemyJumpFeedback;
    [SerializeField] MMF_Player playerEnemyJumpFeedback;
    [SerializeField] MMF_Player playerCannotJump;
    [SerializeField] MMF_Player playerCanExtraJump;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject strongDashEffect;
    [SerializeField] private GameObject dustParticle;

    [Header("GravityScale"), Space(10)]
    [SerializeField] float gravityScale;
    [SerializeField] float maxGravityScale;
    [SerializeField] float gravityGainSpeed = 2.5f;
    [SerializeField] float reduceGravityDivider = 4.0f;
    float currentGravityGainSpeed;
    [SerializeField] float groundCheckRadius = 0.5f;

    [Header("Bool Check"), Space(10)]
    [SerializeField] bool isGrounded;
    [SerializeField] bool isAbleToExtraJump;
    [SerializeField] bool isNearEnemy;
    [SerializeField] bool canCounterForce;

    //[Header("Others"), Space(10)]
    Vector2 mousePos;
    Vector2 startPoint;
    Vector2 currentPoint;
    Vector2 endPoint;
    Vector2 direction;
    Vector3 preJumpPos;


    private void Awake()
    {
        cam = Camera.main.GetComponent<Camera>();
        currentGravityGainSpeed = gravityGainSpeed;
        rb.gravityScale = 0.5f;
        gravityScale = maxGravityScale;
        currentJumpCD = jumpCD;
        //Cursor.visible = false;

    }
    //void Start()
    //{
    //    rb.velocity = Vector2.zero;
    //    currentGravityGainSpeed = gravityGainSpeed;
    //    rb.gravityScale = 0.5f;
    //    gravityScale = maxGravityScale;
    //}

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        isNearEnemy = Physics2D.OverlapCircle(transform.position, enemyCheckRadius, whatIsEnemy);
        currentJumpCD -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.P))
        {
            GameMan.instance.SetMissionSelectionScreen(true);
        }


        if (!isGrounded)
        {
            float rotationSpeed = 10f + (projectileSpeed / 10);
            float offset = 90f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
            dustParticle.transform.rotation = rotation;
            playerSprite.transform.rotation = Quaternion.Slerp(playerSprite.transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            dustParticle.transform.rotation = Quaternion.Slerp(dustParticle.transform.rotation, rotation, rotationSpeed * 1.5f * Time.deltaTime);
            dustDir = Quaternion.Euler(dustParticle.transform.rotation.x, dustParticle.transform.rotation.y, dustParticle.transform.rotation.z);
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
            dustParticle.transform.rotation = Quaternion.Slerp(dustParticle.transform.rotation, rotation, rotationSpeed * 1.5f * Time.deltaTime);
            dustDir = Quaternion.Euler(dustParticle.transform.rotation.x, dustParticle.transform.rotation.y, dustParticle.transform.rotation.z);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.lockState = CursorLockMode.None;
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            playerAnim.ResetTrigger("IsLanding");
            playerAnim.SetTrigger("IsAiming");
            trajectory.ShowDot();
            playerPreJumpFeedback.PlayFeedbacks();

        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            //Cursor.lockState = CursorLockMode.None;
            playerPreJumpChargeFeedback.PlayFeedbacks();
            preJumpPos = transform.position;
            currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            direction = (currentPoint - startPoint).normalized; // Calculate the direction vector
            float tempDragDistance = Vector2.Distance(currentPoint, startPoint);
            tempDragDistance = Mathf.Clamp(tempDragDistance, 6, 16);
            float currentForce = tempDragDistance * 2.5f;
            trajectory.UpdateDots(transform.position, (-direction * currentForce), 3f);

            if (isNearEnemy && !isGrounded)
            {
                RaycastHit2D hit = Physics2D.BoxCast(rayCastPos.position, enemyCheckBox, 0f, -direction, enemyCheckRadius, whatIsEnemy);
                if (hit.collider != null)
                {
                    playerPreEnemyJumpFeedback.PlayFeedbacks();
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

            if (hit.collider && isNearEnemy || hit.collider && isGrounded || hit.collider && isAbleToExtraJump)
            {
                gravityScale = 0;
                isAbleToExtraJump = false;

                Vector2 enemyDirection = (transform.position - hit.transform.position).normalized; // Calculate the direction vector
                rb.velocity = rb.velocity * 0.01f;
                rb.AddForce(-enemyDirection * enemyDashSpeed, ForceMode2D.Impulse);

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
                dustDir = rotation;
                SpawnStrongDashEffect(rotation);
                playerEnemyJumpFeedback.PlayFeedbacks();

            }
            else if (!hit && isGrounded && currentJumpCD <= 0 || !hit && isAbleToExtraJump && currentJumpCD <= 0)
            {
                gravityScale = 0;
                isAbleToExtraJump = false;
                adjustedDir = new Vector2(direction.x * xShootScale, direction.y * yShootScale);

              
                StartCoroutine(CounterForce(adjustedDir, projectileSpeed));
                rb.velocity = rb.velocity * 0.01f;
                rb.AddForce(-adjustedDir * projectileSpeed, ForceMode2D.Impulse);
                Debug.Log("Projectile Speed " + projectileSpeed);
               

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
                dustDir = rotation;
                SpawnDashEffect(rotation, 1 + (projectileSpeed / 10));
                playerJumpFeedback.PlayFeedbacks();
                currentJumpCD = jumpCD;
            }
            else
            {
                playerCannotJump.PlayFeedbacks();
            }
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
    public void ExtraJump()
    {
        StartCoroutine(EnableExtraJump(extraJumpTime));
    }
    public IEnumerator EnableExtraJump(float timePeriod)
    {
        isAbleToExtraJump = true;
        yield return new WaitForSeconds(0.5f);
        playerCanExtraJump.PlayFeedbacks();
        yield return new WaitForSeconds(timePeriod - 0.5f);
        isAbleToExtraJump = false;
    }

    private IEnumerator TempZeroGravity(float timePeriod)
    {
        gravityScale = 0;
        yield return new WaitForSeconds(timePeriod);
        gravityScale = 0.5f;
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
    public Quaternion GetDustDir()
    {
        return dustDir;
    }
}
