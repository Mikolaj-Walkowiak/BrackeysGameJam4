using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Path", menuName="NPC Paths")]
public class Path : ScriptableObject
{
    public List<Vector2> positions = new List<Vector2>();
}
