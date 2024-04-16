using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotateToWaypoints : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private int currentIndex;
    [SerializeField] private GameObject enemySprite;
    [SerializeField] private float rotateSpd;
    //[SerializeField] private bool doesItRandomlyRotate;
    [SerializeField] private bool isRotating;
    private bool canRotate;
    void Start()
    {
        canRotate = true;
        currentIndex = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRotate)
        {
            RotateToDirection(wayPoints[currentIndex]);
        }
    }

    void RotateToDirection(Transform wayPoint)
    {
        Vector2 direction = wayPoint.position;
        isRotating = true;
        float offset = 90f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);


        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotateSpd * Time.deltaTime);
        enemySprite.transform.rotation = Quaternion.Slerp(enemySprite.transform.rotation, rotation, rotateSpd * Time.deltaTime);
        float angleDiff = Vector2.Angle(transform.position, direction);
        Debug.Log("Enemy Rotate :" + angleDiff);
        if (angleDiff <= 0.1f && angleDiff >= -0.1f) 
        {
           
            //isRotating = false;
            //canRotate = false;
            //ChangeDirection();
        }
    }

    void ChangeDirection()
    {
        if(currentIndex < wayPoints.Length)
        {
            currentIndex += 1;
        }
        else if (currentIndex >= wayPoints.Length)
        {
            currentIndex = 0;
        }
        canRotate = true;
    }


}
