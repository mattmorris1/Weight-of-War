using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    private float totalWeight = 0f;
    List<GameObject> currentObjects = new List<GameObject>();
    List<string> objectNames = new List<string>();
    //private GameObject[] currentObjects = new GameObject[10];
    //private string[] objectNames;

    //When an object is placed in the bag area, adds the object to the items list and adds its weight to the total
    void OnTriggerEnter(Collider Other)
    {
        if (Other.gameObject.CompareTag("Packable item"))
        {
            try
            {
                totalWeight += Other.gameObject.GetComponent<Item_Properties>().weight;
                currentObjects.Add(Other.gameObject);
                objectNames.Add(Other.gameObject.GetComponent<Item_Properties>().name);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            Debug.Log("total weight is: " + totalWeight);
            Debug.Log("Items:");
            foreach (string item in objectNames) { Debug.Log(item); }
        }
    }

    //Removes objects that are packable from the object list and subtracts their weight if the item is removed from the bag area
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Packable item")
        {
            try
            {
                totalWeight -= other.gameObject.GetComponent<Item_Properties>().weight;
                currentObjects.Remove(other.gameObject);
                objectNames.Remove(other.gameObject.GetComponent<Item_Properties>().name);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            Debug.Log("total weight is: " + totalWeight);
            Debug.Log("Items:");
            foreach (string item in objectNames) { Debug.Log(item); };
        }
    }

    //Used to get the list of objects that have been packed
    public List<string> GetItems()
    {
        return (objectNames);
    }

    //Used to get the weight of all the packed items
    public float GetWeight()
    {
        return (totalWeight);
    }

}
