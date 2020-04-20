using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPacking : MonoBehaviour
{
    public GameManager gameManager;

    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.CompareTag("Right hand") || Other.gameObject.CompareTag("Left hand"))
        {
            gameManager.StopPacking();
        }
    }
}
