using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMan : MonoBehaviour
{
    [SerializeField] private float slowDownFactor = 0.5f;
    [SerializeField] private float slowDownLength = 2.0f;
    private float reduceSlowNo = 1.0f;
    float originalFixedDeltaTime;
    [SerializeField] private bool isTimeSlow;
    private void Start()
    {
        isTimeSlow = false;
        Time.timeScale = 1;
        originalFixedDeltaTime = Time.fixedDeltaTime;
        Debug.Log(originalFixedDeltaTime);
    }

    void Update()
    {
        Time.timeScale += (1 / (slowDownLength / reduceSlowNo)) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0, 1);

        if (Time.timeScale >= 1)
        {
            Time.timeScale = 1;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
    }

    IEnumerator LerpTime()
    {
        isTimeSlow = true;
        float time = 0;
        float startTimeScale = 0;

        while (time < slowDownLength)
        {
            Time.timeScale = Mathf.Lerp(startTimeScale, 1f, time / slowDownLength);
            time += Time.deltaTime;
        }

        if (Time.timeScale >= 1.0f)
        {
            Time.timeScale = 1.0f;
            isTimeSlow = false;
            yield return null;
        }
    }

    public void StartSlowTime()
    {
        if (!isTimeSlow)
        {
            StartCoroutine(LerpTime());
        }
          
    }


    public void TimeSlowDown(float slowDownNumber)
    {
        slowDownFactor = slowDownNumber;
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
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }

    public void GamePause()
    {
        Time.timeScale = 0f;
        //Debug.Log(Time.timeScale);
    }
}
