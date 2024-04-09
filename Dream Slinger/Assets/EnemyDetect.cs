using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    [SerializeField] private EnemyScipt enemyScipt;

    public void SetEnemyAbleToDestroy(bool boolean)
    {
        enemyScipt.SetAbleToDestroy(boolean);
    }
}
