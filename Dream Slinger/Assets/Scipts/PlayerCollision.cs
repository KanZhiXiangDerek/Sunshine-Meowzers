using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerSoundManager playerSM;
    [SerializeField] private Rigidbody2D rb;
    private Vector2 spawnPos;

    private void Start()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            playerSM.PlayerLandSFX(new Vector3(transform.position.x, transform.position.y, -10));
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
            playerController.ReflectForce(100f);
            //rb.velocity = Vector2.zero;
            //transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 50f * Time.deltaTime);
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                Destroy(collision.gameObject);
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
        if (collision.gameObject.tag == "Enemy")
        {
            rb.gravityScale = 0;
            //playerController.SetGravityScale(0f);
            playerController.TempExtendDisableCounterForce(1.5f);
            playerController.SetZeroGravity(1.0f);
            playerController.ExtraJump();
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
                transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 100f * Time.deltaTime);

        }


        if (collision.gameObject.tag == "EndPoint")
        {
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
                transform.position = collision.transform.position;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            playerController.SetZeroGravity(1.0f);
            rb.velocity = Vector2.zero;
            playerController.ExtraJump();
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.SetEnemyAbleToDestroy(true);
        }


    }

    private void Respawn()
    {
        GameMan.instance.SetLevel();
        transform.position = spawnPos;
    }
}
