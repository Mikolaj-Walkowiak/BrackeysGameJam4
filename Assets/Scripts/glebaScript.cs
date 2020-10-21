using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
public class glebaScript : Interactable
{

    public InteractTeleport doors;
    public Rigidbody2D guard;
    public InMemoryVariableStorage npcMemory;
    public NPC NPC;
    private AudioSource audio;

    void Start()
    {
        audio = GetComponentInParent<AudioSource>();
    }
    void Update()
    {
        if (inRange && Input.GetKeyDown("e"))
        {
            audio.clip = Resources.Load<AudioClip>("SFX/wyjebka");
            audio.Play();
            doors.UnlockDoors();
            npcMemory.SetValue("$wheelchairGleba",1);
            npcMemory.SetValue("$alarm",1);
            GetComponentInParent<NPC>().TakeDamage(1);
            NPC.moveSpeed = 4f;
            guard.mass = 0.5f;
        }
    }
}
