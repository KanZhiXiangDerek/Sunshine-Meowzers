using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ChaseEnemyScipt : MonoBehaviour
{
    [SerializeField] private NavMeshAgent nma;

    [Space(5)]
    [Header("Chase Player")]
    [Space(20)]
    [SerializeField] bool doesItTrackPlayer = false;
    [SerializeField] private Vector3 playerPosition;
    [SerializeField] private LayerMask whatIsPlayer;
    private bool isPlayerInDetectionRange;
    [SerializeField] private float detectionRange;

    [Space(5)]
    [Header("Patrol")]
    [Space(20)]
    [SerializeField] bool doesItPatrol = true;
    [SerializeField] private Vector3 targetPos;
    [SerializeField] private float patrolRange = 30f;



    void Start()
    {  
        nma.GetComponent<NavMeshAgent>();
        nma.updateRotation = false;
        nma.updateUpAxis = false;

        if (doesItPatrol)
        {
            targetPos = GetRandomNearbyPosition();
        }
    }

    void Update()
    {
        if (isPlayerInDetectionRange && doesItTrackPlayer)
        {
            MoveToPlayerLocation();

        }
        else if (doesItPatrol)
        {
            Patrol();
        }
    }

    Vector2 GetPlayerLocation()
    {
        Debug.Log("Get Player Location");
        Transform playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        Vector2 playerPos = new Vector2(playerTransform.position.x, playerTransform.position.y);
        return playerPos;
    }

    void MoveToPlayerLocation()
    {
        RotateToPosition(playerPosition);
        playerPosition = GetPlayerLocation();
        Debug.Log("Move to player location");
        nma.SetDestination(playerPosition);

    }

    void RotateToPosition(Vector3 pos)
    {
        Vector3 rotateAngle = pos - transform.position;
        float angle = Mathf.Atan2(rotateAngle.y, rotateAngle.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    Vector2 GetRandomNearbyPosition()
    {
        float rngX = Random.Range(playerPosition.x - patrolRange, playerPosition.x + patrolRange);
        float rngY = Random.Range(playerPosition.y - patrolRange, playerPosition.y + patrolRange);
        if (rngX >= 80)
        {
            rngX = 80;
        }
        else if (rngX <= -80)
        {
            rngX = -80;
        }

        if (rngY >= 80)
        {
            rngY = 80;
        }
        else if (rngY <= -80)
        {
            rngY = -80;
        }
        Vector2 rngPos = new Vector2(rngX, rngY);
        return rngPos;
    }

    void Patrol()
    {
        RotateToPosition(targetPos);
        if (Vector2.Distance(transform.position, targetPos) <= 0.1f)
        {
            targetPos = GetRandomNearbyPosition();
        }
        nma.SetDestination(targetPos);
    }
}
