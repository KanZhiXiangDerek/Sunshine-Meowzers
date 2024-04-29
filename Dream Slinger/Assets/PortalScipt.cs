using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScipt : MonoBehaviour
{
    [SerializeField] bool canPortal = true;
   
    public void SetPortalBoolean(bool boolean)
    {
        canPortal = boolean;
    }

    public bool GetPortalBoolean()
    {
        return canPortal;
    }
}
