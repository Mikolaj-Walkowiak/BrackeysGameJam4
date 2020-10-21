using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool inRange;
    protected GameObject otherObject;
    protected RewindManager rewind;
    private void Start()
    {
        rewind = RewindManager.instance;
        inRange = false;
        otherObject = null;
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown("e"))
        {
            Debug.Log("Interaction");
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            otherObject = other.gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            otherObject = null;
        }
    }
}
