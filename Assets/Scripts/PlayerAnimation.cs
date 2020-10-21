using System;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private bool move;
    [SerializeField] private float mouseRotation;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    

    private void Update()
    {
        anim.SetBool("move", move);
        anim.SetFloat("mouseRotation", mouseRotation);
    }

    public void SetValues(bool _move, float _mouseRotation)
    {
        move = _move;
        mouseRotation = _mouseRotation;
    }

    public void SetAnimator(string type)
    {
        Debug.Log("setting controller" + type);
        if (type == "police")
        {
            Debug.Log("Police");
            anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/Policja1");
        }
        
        else anim.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Animators/PlayerNew");
    }
}
