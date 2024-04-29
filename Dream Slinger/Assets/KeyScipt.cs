using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScipt : MonoBehaviour
{
    [SerializeField] PortalScipt assignPortal;

    public void CollectKey()
    {
        assignPortal.SetPortalBoolean(true);
    }
}
