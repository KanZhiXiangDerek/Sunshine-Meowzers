using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PlayerCollision : MonoBehaviour
{
    [Header("References"), Space(10)]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float reduceVelocityMutipler = 10f;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 spawnPos;

    [Header("Player Feedback"), Space(10)]
    [SerializeField] MMF_Player landFeedback;
    [SerializeField] MMF_Player deathFeedback;
    [SerializeField] MMF_Player hitEnemyFeedback;
    [SerializeField] MMF_Player hitPlatformFeedback;
    [SerializeField] MMF_Player hitPortalFeedback;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject landEffect;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            rb.velocity = rb.velocity/ reduceVelocityMutipler;
            landFeedback.PlayFeedbacks();
            playerController.AnimTrigger("IsLanding");
            playerController.ResetAnimTrigger("IsJumping");
            playerController.SetGravityScale(0f);
        }

        if (collision.gameObject.tag == "FallingPlatform")
        {
            rb.velocity = Vector2.zero;
            landFeedback.PlayFeedbacks();
            playerController.AnimTrigger("IsLanding");
            playerController.ResetAnimTrigger("IsJumping");
            playerController.SetGravityScale(0f);
            FallingPlatform fallingPlatform = collision.gameObject.GetComponent<FallingPlatform>();
            fallingPlatform.PlayerStepOnPlatform();
            fallingPlatform.SetPlayerOnPlatform(true);
        }

        if (collision.gameObject.tag == "Obstacle")
        {
            rb.velocity = Vector2.zero;
            deathFeedback.PlayFeedbacks();
            Invoke("ScreenFade", respawnDelay / 3);
            Invoke("Respawn", respawnDelay);
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                Destroy(collision.gameObject);
                rb.velocity = Vector2.zero;
                hitEnemyFeedback.PlayFeedbacks();
                playerController.ExtraJump();
            }
            else
            {
                rb.velocity = Vector2.zero;
                deathFeedback.PlayFeedbacks();
                Invoke("ScreenFade", respawnDelay / 3);
                Invoke("Respawn", respawnDelay);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerController.SetGravityGainSpeedOnGround();
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerController.SetGravityGainSpeedToOG();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                playerController.ExtraJump();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Area")
        {
            AreaManager areaMan = collision.gameObject.GetComponent<AreaManager>();
            spawnPos = areaMan.GetCheckPointPos();


        }

        if (collision.gameObject.tag == "Platform")
        {
            MovingPlatform plat = collision.gameObject.GetComponent<MovingPlatform>();
            if (plat.GetWait() == false)
            {
                playerController.ExtraJump();
                hitPlatformFeedback.PlayFeedbacks();
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 100f * Time.deltaTime);
            }

        }

        if (collision.gameObject.tag == "EndPoint")
        {
            if (collision.GetComponent<PortalScipt>().GetPortalBoolean())
            {
                hitPortalFeedback.PlayFeedbacks();
                rb.velocity = Vector2.zero;
                GameMan.instance.NextLevel();
            }
            else
            {
                rb.velocity = Vector2.zero;
                deathFeedback.PlayFeedbacks();
                Invoke("ScreenFade", respawnDelay / 3);
                Invoke("Respawn", respawnDelay);
            }
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.SetEnemyAbleToDestroy(false);
        }

        if (collision.gameObject.tag == "Key")
        {
            KeyScipt keyScipt = collision.gameObject.GetComponent<KeyScipt>();
            keyScipt.CollectKey();
            Destroy(collision.gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            MovingPlatform plat = collision.gameObject.GetComponent<MovingPlatform>();
            plat.MoveToWayPoint();
            if (plat.GetWait() == false)
            {
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 200f * Time.deltaTime);

            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            hitPlatformFeedback.PlayFeedbacks();
            rb.velocity = Vector2.zero;
            playerController.SetZeroGravity(1.0f);
            playerController.ExtraJump();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.PostDetect();
        }


    }

    private void ScreenFade()
    {
        GameMan.instance.ScreenFade(); 
    }
    private void Respawn()
    {
        GameMan.instance.SetLevel();
        playerController.TempDisableControls();
    }
}
