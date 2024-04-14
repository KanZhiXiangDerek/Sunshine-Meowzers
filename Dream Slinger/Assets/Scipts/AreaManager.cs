using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AreaManager : MonoBehaviour
{
    [SerializeField] GameObject camera;
    [SerializeField] Transform checkPoint;
    
    void Start()
    {
        SetCamera(false);
    }

    public void SetCamera(bool boolean)
    {
        camera.SetActive(boolean);
    }

    public Vector2 GetCheckPointPos()
    {
        return checkPoint.transform.position;
    }
}
