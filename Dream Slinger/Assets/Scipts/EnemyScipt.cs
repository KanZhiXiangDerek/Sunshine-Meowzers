using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScipt : MonoBehaviour
{
    [SerializeField] private bool ableToBeDestroyed;
    [SerializeField] private bool doesItChase;
    [SerializeField] private float chaseSpd = 5.0f;
    [SerializeField] private float chaseTime = 4.0f;

    [SerializeField] private bool doesItShoot;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform sp;
    [SerializeField] private float projectileSpd = 8.0f;
    [SerializeField] private float shootPerXSeconds = 1.5f;
    bool isFiring;

    private void Start()
    {
        ableToBeDestroyed = true;
    }

    private void Update()
    {
        if (!ableToBeDestroyed && doesItChase)
        {
            GameObject player = GameMan.instance.GetPlayerObj();
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, chaseSpd * Time.deltaTime);
        }

        if(!ableToBeDestroyed && doesItShoot)
        {
            if (!isFiring)
                StartCoroutine(ShootProjectile());
           
        }
    }

    private IEnumerator ShootProjectile()
    {
        isFiring = true;
        Debug.Log("Enemy  Shoot");
        Transform player = GameMan.instance.GetPlayerObj().transform;

        GameObject enemyProjectile = Instantiate(projectile, sp.position, Quaternion.identity);
        Vector2 direction = (player.position - sp.position).normalized; // Calculate the direction vector
        Rigidbody2D rb = enemyProjectile.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * projectileSpd, ForceMode2D.Impulse);
        yield return new WaitForSeconds(shootPerXSeconds);
        isFiring = false;
    }
    public bool IsAbleToBeDestroyed()
    {
        return ableToBeDestroyed;
    }

    public void SetAbleToDestroy(bool boolean)
    {
        ableToBeDestroyed = boolean;
    }

    public void PostDetect()
    {
        if (doesItChase)
        {
            SetAbleToDestroyAfterXSeconds(true);
        }
        else
        {
            SetAbleToDestroy(true);
        }
    }
    public void SetAbleToDestroyAfterXSeconds(bool boolean)
    {
        float timer = chaseTime;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            ableToBeDestroyed = boolean;
        }
    }


}
