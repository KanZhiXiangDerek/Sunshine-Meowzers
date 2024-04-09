using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingScipt : MonoBehaviour
{
    [SerializeField] private float rotateSpd;
    [SerializeField] private int rotateDir;

    [SerializeField] private bool doItPause;
    private float currentTimeToNextPause;
    [SerializeField] private float pauseEveryXSecond;
    [SerializeField] private float pauseTime;
    private bool isRotating;

    void Start()
    {
        isRotating = true;
        currentTimeToNextPause = pauseEveryXSecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            Rotate();
        }

        if (currentTimeToNextPause <= 0)
        {
            isRotating = false;
            currentTimeToNextPause = pauseEveryXSecond + pauseTime;
            Invoke("IsRotatingSetToTrue", pauseTime);
        }

        currentTimeToNextPause -= Time.deltaTime;
    }

    void Rotate()
    {
        transform.Rotate(0, 0, rotateSpd * rotateDir * Time.deltaTime);
    }

    void IsRotatingSetToTrue()
    {
        isRotating = true;
    }
}