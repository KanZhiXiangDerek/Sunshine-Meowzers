using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGround : MonoBehaviour
{
    [SerializeField] private float groundMovSpeed;
    [SerializeField] private float startDelay = 2.0f;
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    bool isMoving;
    void Start()
    {
        isMoving = false;
        Invoke("SetIsMovingTrue", startDelay);
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPos, groundMovSpeed * Time.deltaTime);
        }
    }

    void SetIsMovingTrue()
    {
        isMoving = true;
    }
}
