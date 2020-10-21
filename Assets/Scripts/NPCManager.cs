using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    #region Singleton
    public static NPCManager instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    
    public List<GameObject> npcs = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        npcs.AddRange(GameObject.FindGameObjectsWithTag("NPC").ToList());
        npcs.AddRange(GameObject.FindGameObjectsWithTag("Enemy").ToList());

    }
    
}
