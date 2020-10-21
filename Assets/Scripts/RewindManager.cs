using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity.Example;

public class RewindManager : MonoBehaviour
{
    #region Singleton
    public static RewindManager instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion
    
    public int rewindsRemaining = 5000;
    public float rewindTime = 10f;
    private bool isRewinding;
    private NPCManager npcManager;
    private AudioSource audio;
    [SerializeField] private PlayerController playerController;
    public bool IsRewinding
    {
        get { return isRewinding;}
        set
        {
            isRewinding = value;
            if (value)
            {
                audio.Play();
                playerController.GetComponent<RewindLogic>().StartRewind();
                playerController.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                foreach (GameObject npc in npcManager.npcs)
                {
                    if (npc.GetComponent<RewindLogic>() != null)
                    {
                        npc.GetComponent<RewindLogic>().StartRewind();
                        npc.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                    }
                }
            }
            else
            {
                playerController.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                foreach (GameObject npc in npcManager.npcs)
                {
                    if (npc.GetComponent<Rigidbody2D>() != null)
                    {
                        if(npc.GetComponent<RewindLogic>() != null && !npc.GetComponent<RewindLogic>().isReallyKinematic)
                            npc.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    }
                }
            }
        }
    }

    private void Start()
    {
        npcManager = NPCManager.instance;
        audio = GetComponent<AudioSource>();
        rewindsRemaining = 4;
        rewindTime = 10f;
        isRewinding = false;
    }
}
