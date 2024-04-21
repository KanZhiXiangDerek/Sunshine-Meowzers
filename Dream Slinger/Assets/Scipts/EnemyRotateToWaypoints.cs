using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRotateToWaypoints : MonoBehaviour
{
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private Quaternion[] rotationPoints;
    [SerializeField] private int currentIndex;
    //[SerializeField] private GameObject enemySprite;
    [SerializeField] private float rotateSpeed = 5.0f;
    //[SerializeField] private bool doesItRandomlyRotate;
    [SerializeField] private bool isRotating;

    // Cooldown duration (in seconds)
    float cooldownDuration = 1.0f; // Adjust as needed

    // Flag to track if cooldown is active
    bool isCooldownActive = false;

    void Start()
    {
        currentIndex = 0;
        isRotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateToDirection(rotationPoints[currentIndex]);
    }

    void RotateToDirection(Quaternion rotation)
    {
        if (!isRotating)
        {
            StartCoroutine(LerpRotation(rotation, rotateSpeed));
        }

        float closeAngleThreshold = 0.1f; // In degrees
        float angleDifference = Quaternion.Angle(transform.rotation, rotation);

        if (angleDifference <= closeAngleThreshold && !isCooldownActive)
        {
            ChangeDirection();
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
    }

    IEnumerator StartCooldown()
    {
        isCooldownActive = true;
        yield return new WaitForSeconds(cooldownDuration);
        isCooldownActive = false;
    }



}
