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
    private Vector2 spawnPos;

    [SerializeField] MMF_Player landFeedback;
    private void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            landFeedback.PlayFeedbacks();
            playerController.AnimTrigger("IsLanding");
            playerController.ResetAnimTrigger("IsJumping");


            rb.velocity = Vector2.zero;

            playerController.SetGravityScale(0f);
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            Respawn();
        }

        if (collision.gameObject.tag == "Enemy")
        {      
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                Destroy(collision.gameObject);
                playerSM.PlayerKillSFX(transform.position);
                playerController.ExtraJump();
                
            }
            else
            {
                Respawn();
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
            playerSM.PlayerPlatformSFX(transform.position);
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
        playerSM.PlayerDieSFX(spawnPos);
        SpawnDeathEffect();
        GameMan.instance.SetLevel();
        transform.position = spawnPos;
    }

    public void SpawnDeathEffect()
    {
        GameObject deathEff = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(deathEff, 3.0f);
    }
}
