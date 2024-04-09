using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMan : MonoBehaviour
{
    public static GameMan instance = null;
    [SerializeField] private TimeMan timeManager;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TimeSlow()
    {
        timeManager.TimeSlowDown();
    }

    public void ResetTimeScale()
    {
        timeManager.ResetTimeScale();
    }

    public void SlowDownLengthReduce(float reduceDivNo)
    {
        timeManager.SlowDownLengthReduce(reduceDivNo);
    }
}

