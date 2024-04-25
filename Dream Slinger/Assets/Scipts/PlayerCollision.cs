using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PlayerCollision : MonoBehaviour
{
    [Header("References"), Space(10)]
    [SerializeField] private PlayerController playerController;
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
            rb.velocity = Vector2.zero;
            landFeedback.PlayFeedbacks();
            playerController.AnimTrigger("IsLanding");
            playerController.ResetAnimTrigger("IsJumping");    
            playerController.SetGravityScale(0f);
        }

        if (collision.gameObject.tag == "Obstacle")
        {
            rb.velocity = Vector2.zero;
            deathFeedback.PlayFeedbacks();
            Invoke("Respawn", 0.2f);
        }

        if (collision.gameObject.tag == "Enemy")
        {      
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                rb.velocity = Vector2.zero;
                //playerController.SetGravityScale(0f);
                hitEnemyFeedback.PlayFeedbacks();
                Destroy(collision.gameObject);
                playerController.ExtraJump();
            }
            else
            {
                rb.velocity = Vector2.zero;
                deathFeedback.PlayFeedbacks();
                Invoke("Respawn", 0.2f);
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
                hitPlatformFeedback.PlayFeedbacks();
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 100f * Time.deltaTime);
            }
        
        }

        if (collision.gameObject.tag == "EndPoint")
        {
            hitPortalFeedback.PlayFeedbacks();
            rb.velocity = Vector2.zero;
            GameMan.instance.NextLevel();
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.SetEnemyAbleToDestroy(false);
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
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 100f * Time.deltaTime);
                    
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            hitPlatformFeedback.PlayFeedbacks();
            rb.velocity = Vector2.zero;
            playerController.SetZeroGravity(0.1f);
            playerController.ExtraJump();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.PostDetect();
        }


    }

    private void Respawn()
    {
        rb.velocity = Vector2.zero;
        GameMan.instance.SetLevel();
    }
}
