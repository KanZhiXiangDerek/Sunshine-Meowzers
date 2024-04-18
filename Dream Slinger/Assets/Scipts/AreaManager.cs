using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class AreaManager : MonoBehaviour
{
    [SerializeField] GameObject cam;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] bool followPlayer;
    [SerializeField] Transform checkPoint;
    GameObject player;


    void Start()
    {
        SetCamera(true);
        if (followPlayer)
        {
            Invoke("SetFollowPlayer", 0.1f);
        }
    }

    private void Update()
    {
        if(followPlayer & vcam.Follow == null)
        {
            SetFollowPlayer();
        }
    }

    public void SetCamera(bool boolean)
    {
        cam.SetActive(boolean);
    }

    public void SetFollowPlayer()
    {
        player = GameMan.instance.GetPlayerObj();
        vcam.Follow = player.transform;
    }

    public Vector2 GetCheckPointPos()
    {
        return checkPoint.transform.position;
    }
}
