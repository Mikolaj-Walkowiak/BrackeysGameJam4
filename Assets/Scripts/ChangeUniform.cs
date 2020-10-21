using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class ChangeUniform : Interactable
{
   
    public InMemoryVariableStorage npcMemory;
    public PlayerController player;
    private bool changed;
    public GameObject OpenExitObject;
    void Start()
    {
        changed = false;
        player = PlayerController.instance;
    }
    void Update()
    {
        if (inRange && Input.GetKeyDown("e") && !changed)
        {
            npcMemory.SetValue("$isInUniform",1);
            
            Debug.Log("SetAnimator(police)");
            player.GetComponentInChildren<PlayerAnimation>().SetAnimator("police");
            changed = true;
            //player.ChangeUniform("policjant");

            Destroy(gameObject, 3f);
            Destroy(OpenExitObject);
        }
    }
}
