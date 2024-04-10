using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMan : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.5f;
    [SerializeField] private float slowDownLength = 2.0f;
    private float reduceSlowNo = 1.0f;

    private void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        Time.timeScale += (1 / (slowDownLength / reduceSlowNo)) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);

        if (Time.timeScale >= 1)
        {
            Time.timeScale = 1;
        }
    }

    public void TimeSlowDown()
    {
        reduceSlowNo = 1.0f;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.2f;
    }

    public void SlowDownLengthReduce(float reduceDivNo)
    {
        reduceSlowNo = reduceDivNo;
    }

    public void ResetTimeScale()
    {
        Time.timeScale = 1f;
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        Debug.Log(Time.timeScale);
    }
}
