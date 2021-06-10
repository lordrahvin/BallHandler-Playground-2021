using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 offset = Vector3.zero;

    private void Start()
    {
        if (FindPlayer() == null) { return;}
    }

    private GameObject FindPlayer()
    {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                offset = transform.position - player.transform.position ;
            }
        }

        return player;
    }

    void Update()
    {
        if (FindPlayer() == null) { return;}

        transform.position = player.transform.position + offset;
        


    }
}
