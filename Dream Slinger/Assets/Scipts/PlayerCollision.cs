using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerSoundManager playerSM;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private GameObject landEffect;
    private Vector2 spawnPos;

    [SerializeField] MMF_Player landFeedback;
    [SerializeField] MMF_Player deathFeedback;
    [SerializeField] MMF_Player hitEnemyFeedback;
    private void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            landFeedback.PlayFeedbacks();
            SpawnLandEffect(playerController.GetDustDir());
            playerController.AnimTrigger("IsLanding");
            playerController.ResetAnimTrigger("IsJumping");


            rb.velocity = Vector2.zero;

            playerController.SetGravityScale(0f);
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            deathFeedback.PlayFeedbacks();
            Invoke("Respawn", 0.2f);
        }

        if (collision.gameObject.tag == "Enemy")
        {      
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                hitEnemyFeedback.PlayFeedbacks();
                Destroy(collision.gameObject);
                playerController.ExtraJump();
                
            }
            else
            {
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
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
                landFeedback.PlayFeedbacks();
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 100f * Time.deltaTime);
            }
        
        }


        if (collision.gameObject.tag == "EndPoint")
        {
            playerSM.PlayerPortalSFX(transform.position);
            GameMan.instance.NextLevel();
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            //Respawn();
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
                transform.position = collision.transform.position;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            landFeedback.PlayFeedbacks();
            //playerSM.PlayerPlatformSFX(transform.position);
            rb.velocity = Vector2.zero;
            playerController.SetZeroGravity(3.0f);
            playerController.ExtraJump();
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.PostDetect();
        }


    }

    private void Respawn()
    {
        GameMan.instance.SetLevel();
    }

    public void SpawnLandEffect(Quaternion dir)
    {
        //GameObject deathEff = Instantiate(landEffect, transform.position, dir);
        //Destroy(landEffect, 3.0f);
    }
}
