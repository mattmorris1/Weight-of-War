using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongButton : MonoBehaviour
{
    private GameManager gameManager;

    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.CompareTag("Right hand") || Other.gameObject.CompareTag("Left hand"))
        {
            gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
            gameManager.StartLongCampaign();
        }
    }
}
