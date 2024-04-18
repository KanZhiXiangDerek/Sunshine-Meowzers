using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotateToWaypoints : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private int currentIndex;
    //[SerializeField] private GameObject enemySprite;
    [SerializeField] private float rotateSpeed = 5.0f;
    //[SerializeField] private bool doesItRandomlyRotate;
    [SerializeField] private bool isRotating;
    private bool canRotate;

    // Cooldown duration (in seconds)
    float cooldownDuration = 5.0f; // Adjust as needed

    // Flag to track if cooldown is active
    bool isCooldownActive = false;

    void Start()
    {
        canRotate = true;
        currentIndex = 0;
        isRotating = false;
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
        float offset = 90f;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);

        if (!isRotating)
        {
            StartCoroutine(LerpRotation(rotation, rotateSpeed));
        }

        //Quaternion playerRotation = transform.rotation;
        ///enemySprite.transform.rotation = Quaternion.Slerp(enemySprite.transform.rotation, rotation, rotateSpd * Time.deltaTime);
        float closeAngleThreshold = 1.0f; // In degrees

        // Calculate the angle between current rotation and target rotation
        float angleDifference = Quaternion.Angle(transform.rotation, rotation);

        if (angleDifference <= closeAngleThreshold && !isCooldownActive)
        {
            canRotate = false;
            Invoke("ChangeDirection", 1.0f);
            StartCoroutine(StartCooldown());

        }
    }

    IEnumerator LerpRotation(Quaternion rotation, float duration)
    {
        isRotating = true;
        float time = 0;
        Quaternion startValue = transform.rotation;

        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startValue, rotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = rotation;
        isRotating = false;
    }

    void ChangeDirection()
    {
        currentIndex = (currentIndex + 1) % wayPoints.Length;
        canRotate = true;
    }

    IEnumerator StartCooldown()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldownActive = false;
        ChangeDirection(); // Change direction after cooldown
    }



}
