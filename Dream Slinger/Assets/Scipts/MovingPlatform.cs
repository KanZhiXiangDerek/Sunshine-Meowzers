using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    Vector2 targetPos;
    [SerializeField] private float movSpeed;
    private int currentIndex;
    private int nextIndex;
    [SerializeField] private bool isWaiting;
   


    private bool searchNewWayPoint = false;
    private void Start()
    {
        isWaiting = false;
        currentIndex = 0;
        nextIndex = 1;
        targetPos = new Vector2(wayPoints[currentIndex].position.x, wayPoints[currentIndex].position.y);
    }
    private void Update()
    {
        //if (searchNewWayPoint)
        //{
        //    if (isWaiting)
        //    {
        //        targetPos = transform.position;
        //        Invoke("IsNotWaiting", waitTime);
        //    }
        //    else
        //    {
        //        NextWayPoint();
        //    }
        //}

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
        Invoke("SetIsWaitingFalse", 1.0f);
    }
    public void MoveToWayPoint()
    {
        if (!isWaiting)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, movSpeed * Time.deltaTime);
        }
        if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
        {
            isWaiting = true;
            NextWayPoint();
            return;
        }
    }

    private void SetIsWaitingFalse()
    {
        isWaiting = false;
    }

    public bool GetWait()
    {
        return isWaiting;
    }
    public void IsNotWaiting()
    {
        isWaiting = false;
    }
}
