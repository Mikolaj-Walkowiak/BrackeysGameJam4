using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Yarn.Unity;

public class RewindLogic : MonoBehaviour
{
    private NPCManager npcManager;
    private int isAlarmRinging;
    private RewindManager rewind;
    LinkedList<ObjectInTime> positions;
    Rigidbody2D rigidbody;
    public bool isReallyKinematic;

    private Health health;
    
    public struct ObjectInTime
    {
        public Vector3 position;
        public int currentHealth;
        public Yarn.Value isAlarmRinging;

        public ObjectInTime(Vector3 _position, int _currentHealth, Yarn.Value _isAlarmRinging)
        {
            position = _position;
            currentHealth = _currentHealth;
            isAlarmRinging = _isAlarmRinging;
        }
    }
    
    public delegate void OnRewindUsed();

    public OnRewindUsed onRewindUsedCallback;
    
    void Start()
    {
        npcManager = NPCManager.instance;
        rewind = RewindManager.instance;
        positions = new LinkedList<ObjectInTime>();
        rigidbody = GetComponent<Rigidbody2D>();
        isReallyKinematic = rigidbody.isKinematic;
        health = GetComponent<Health>();
    }
    void FixedUpdate()
    {
        if (rewind.IsRewinding) Rewind();
        else Record();
    }
    
    void Record()
    {
        if (positions.Count < Mathf.Round(rewind.rewindTime / Time.fixedDeltaTime))
        {
            positions.AddFirst(new ObjectInTime(transform.position, health.CurrentHealth, npcManager.GetComponent<InMemoryVariableStorage>().GetValue("$alarm")));
        }

        else
        {
            positions.AddFirst(new ObjectInTime(transform.position, health.CurrentHealth, npcManager.GetComponent<InMemoryVariableStorage>().GetValue("$alarm")));
            positions.RemoveLast();
        }
    }

    void Rewind()
    {
        if (positions.Count > 1)
        {
            //camera.m_Lens = new LensSettings(0f, 6.75f + (10 / (positions.Count / 10f)), 0.3f, 1000f, 180f - 5* positions.Count);
            transform.position = positions.First.Value.position;
            health.CurrentHealth = positions.First.Value.currentHealth;
            npcManager.GetComponent<InMemoryVariableStorage>().SetValue("$alarm", positions.First.Value.isAlarmRinging);
            positions.RemoveFirst();
            positions.RemoveFirst();
        }
        else{
            StopRewind();
            //camera.m_Lens = new LensSettings(0f, 6.75f, 0.3f, 1000f, 0f);
        } 
    }

    public void StartRewind()
    {
        
        if (onRewindUsedCallback != null)
        {
            Debug.Log("onRewindUsedCallback.Invoke");
            onRewindUsedCallback.Invoke();
        }
    }
    public void StopRewind()
    {
        rewind.IsRewinding = false;
    }
}
