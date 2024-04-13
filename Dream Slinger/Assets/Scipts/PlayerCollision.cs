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
            rb.velocity = rb.velocity / 10f;
            playerController.SetGravityScale(0f);
        }
        if (collision.gameObject.tag == "Obstacle")
        {
            Respawn();
        }

        if (collision.gameObject.tag == "Enemy")
        {
            transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, 50f * Time.deltaTime);
            if (collision.gameObject.GetComponent<EnemyScipt>().IsAbleToBeDestroyed())
            {
                Destroy(collision.gameObject);
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
        if (collision.gameObject.tag == "Enemy")
        {
            rb.velocity = Vector2.zero;
            rb.gravityScale = 0;
            playerController.SetGravityScale(0f);
            playerController.TempExtendDisableCounterForce(1.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Area")
        {
            AreaManager areaMan = collision.gameObject.GetComponent<AreaManager>();
            areaMan.SetCamera(true);
        }

        if (collision.gameObject.tag == "Checkpoint")
        {

            spawnPos = collision.gameObject.transform.position;
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.SetEnemyAbleToDestroy(false);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Area")
        {
            AreaManager areaMan = collision.gameObject.GetComponent<AreaManager>();
            areaMan.SetCamera(false);
        }

        if (collision.gameObject.tag == "EnemyDetection")
        {
            EnemyDetect enemyDetect = collision.gameObject.GetComponent<EnemyDetect>();
            enemyDetect.SetEnemyAbleToDestroy(true);
        }


    }

    private void Respawn()
    {
        transform.position = spawnPos;
    }
}
