using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Properties : MonoBehaviour
{
    public float weight;
    public AudioClip itemNarration;
    private bool hasPlayedAudio = false;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        //Checks if object is parented to one of the hands and if its sound has been played yet
        if(transform.parent != null && (transform.parent.tag == "Left hand" || transform.parent.tag == "Right hand") && hasPlayedAudio == false)
        {
            //Tries to play the assigned audio clip through the players audio component and then sets the has played audio to true
            try
            {
                transform.GetComponentInParent<Player>().PlayAudio(itemNarration);
            }catch(Exception ex)
            {
                Debug.Log(ex);
            }
            hasPlayedAudio = true;
        } //Checks if the object doesn't have a parent and if its played audio and resets it to false
        else if(transform.parent == null && hasPlayedAudio == true)
        {
            hasPlayedAudio = false;
        }
    }

    //Checks if the gameobject was hit the floor and resets it within the players reach
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Floor"))
        {
            this.transform.position = startPos;
            this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
