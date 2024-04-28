using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionButton : MonoBehaviour
{
    [SerializeField] LevelContainer levelPrefab;
    float timeTaken;
    [SerializeField] TMP_Text timeTMP;

    void Start()
    {
        if (levelPrefab.levelIsCompleted)
        {
            timeTaken = levelPrefab.bestCompletionTime;
            SetTime(levelPrefab.bestCompletionTime);
        }
        else 
        {
            timeTMP.text = "NA";
        }
    }

    // Update is called once per frame
    void Update()
    {
       if(timeTaken < levelPrefab.bestCompletionTime)
        {
            SetTime(levelPrefab.bestCompletionTime);
        }
    }

    void SetTime(float time)
    {
        //Calculate the time in minutes and seconds.
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;

        //Update the duration text.
        timeTMP.text = minutes.ToString() + ":" + ((seconds < 10) ? ("0") : ("")) + seconds.ToString();
    }
}
