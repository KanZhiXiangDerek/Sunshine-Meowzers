using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] GameObject camera;
    void Start()
    {
        SetCamera(false);
    }

    public void SetCamera(bool boolean)
    {
        camera.SetActive(boolean);
    }
}
