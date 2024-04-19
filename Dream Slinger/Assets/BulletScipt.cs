using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScipt : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Obstacle" || collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
