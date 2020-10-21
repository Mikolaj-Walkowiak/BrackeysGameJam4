using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthSlot : MonoBehaviour
{
    public Image icon;
    public void AddHealth()
    {
        icon.enabled = true;
    }

    public void ClearHealth()
    {
        icon.enabled = false;
    }
}
