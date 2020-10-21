using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFind : MonoBehaviour
{
    #region Singleton
    public static GroundFind instance;
    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion
}
