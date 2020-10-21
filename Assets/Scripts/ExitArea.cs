using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitArea : MonoBehaviour
{
    public bool inRange;
    public GameManager gameManager;
    private void Start()
    {
        inRange = false;
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
            gameManager.isInExitArea = true;
        }
    }
}
