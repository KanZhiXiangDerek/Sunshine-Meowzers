using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float platformHealthPoints = 20;
    [SerializeField] private float currentHealthPoints;
    [SerializeField] private float platformHealthDropRate = 2;
    [SerializeField] private MMF_Player platformBlinkFB;
    private bool isOnStepCountdown;
    private bool playerIsOnPlatform;
    private void Start()
    {
        currentHealthPoints = platformHealthPoints;
    }

    private void Update()
    {
        if (playerIsOnPlatform)
        {
            currentHealthPoints -= platformHealthDropRate * Time.deltaTime;
        }

        if (currentHealthPoints <= platformHealthPoints * 0.25)
        {
            platformBlinkFB.PlayFeedbacks();
        }

        if (currentHealthPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void PlayerStepOnPlatform()
    {
        if (!isOnStepCountdown)
        {
            currentHealthPoints -= 10;
            isOnStepCountdown = true;
            //StartCoroutine(StepCooldown());
        }
    }

    public void SetPlayerOnPlatform(bool boolean)
    {
        playerIsOnPlatform = boolean;
    }

    IEnumerator StepCooldown()
    {
        isOnStepCountdown = true;
        yield return new WaitForSeconds(0.1f);
        isOnStepCountdown = false;
    }
}
