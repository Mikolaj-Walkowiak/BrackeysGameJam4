using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Alarm : MonoBehaviour
{
    private AudioSource audio;
    private NPCManager npc;
    
    #region Singleton
    public static Alarm instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    private bool alarmSwitch;

    public bool AlarmSwitch
    {
        get
        {
            return alarmSwitch;
        }
        set
        {
            if (value)
            {
                alarmSwitch = value;
                audio.Play();
            }
            else
            {
                alarmSwitch = value;
                audio.Stop();
            }
        }
    }

    void Start()
    {
        npc = NPCManager.instance;
        audio = GetComponent<AudioSource>();
        alarmSwitch = false;
    }

    private void Update()
    {
        if (npc.GetComponent<InMemoryVariableStorage>().GetValue("$alarm").AsNumber == 1f && alarmSwitch != true)
        {
            AlarmSwitch = true;
        }
        else if(npc.GetComponent<InMemoryVariableStorage>().GetValue("$alarm").AsNumber == 0f && alarmSwitch != false)
        {
            AlarmSwitch = false;
        }
    }
}
