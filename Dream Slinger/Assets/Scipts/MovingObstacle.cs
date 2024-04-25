using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    Vector2 targetPos;
    [SerializeField] private float movSpeed;
    private int currentIndex;
    private int nextIndex;
    [SerializeField] private bool containsWaitTime;
    private bool isWaiting;
    [SerializeField] private float waitTime;


    private bool searchNewWayPoint = false;
    private void Start()
    {
        if (!containsWaitTime)
        {
            waitTime = 0;
        }
        isWaiting = false;
        currentIndex = 0;
        nextIndex = 1;
        targetPos = new Vector2(wayPoints[currentIndex].position.x, wayPoints[currentIndex].position.y);
    }
    private void Update()
    {
        if (searchNewWayPoint)
        {
            if (isWaiting)
            {
                targetPos = transform.position;
                Invoke("IsNotWaiting", waitTime);
            }
            else
            {
                NextWayPoint();
            }
        }
        MoveToWayPoint();
    }


    public void NextWayPoint()
    {
        if (currentIndex < wayPoints.Length - 1)
        {
            currentIndex = nextIndex;
        }
        else
        {
            currentIndex = 0;
        }

        nextIndex = currentIndex + 1;
        targetPos = new Vector2(wayPoints[currentIndex].position.x, wayPoints[currentIndex].position.y);
        searchNewWayPoint = false;
    }
    public void MoveToWayPoint()
    {
        if (Vector2.Distance(transform.position, targetPos) <= 0.5f)
        {
            searchNewWayPoint = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, movSpeed * Time.deltaTime);
    }

    public void IsNotWaiting()
    {
        isWaiting = false;
    }
}
