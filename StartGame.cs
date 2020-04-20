using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject player;
    public Transform spawn;
    private void Start()
    {
        try
        {
            if (GameObject.Find("OVRPlayerController") == null)
            {
                Debug.Log("Creating player");
                Instantiate(player, spawn.position, spawn.rotation);
            }
        }catch(Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
