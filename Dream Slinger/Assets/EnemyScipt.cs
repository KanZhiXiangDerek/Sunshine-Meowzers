using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScipt : MonoBehaviour
{
    [SerializeField] private bool ableToBeDestroyed;
    
    public bool IsAbleToBeDestroyed()
    {
        return ableToBeDestroyed;
    }

    public void SetAbleToDestroy(bool boolean)
    {
        ableToBeDestroyed = boolean;
    }
}
