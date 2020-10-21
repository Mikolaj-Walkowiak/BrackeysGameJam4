using System;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
public class PlayerMovement : MonoBehaviour
{

    private RewindManager rewind;
    
    private Rigidbody2D rb2d;
    private PlayerAnimation playerAnim;
    private PlayerController pc;
    
    [SerializeField] private float moveSpeed = 1f;
    public Vector2 movement;
    [SerializeField] private bool animMove;
    void Start()
    {
        rewind = RewindManager.instance;
        
        rb2d = GetComponent<Rigidbody2D>();
        playerAnim = GetComponentInChildren<PlayerAnimation>();
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        animMove = movement.x != 0 || movement.y != 0;
        SetPlayerAnims();
    }

    void SetPlayerAnims()
    {
        if (!pc.IsBusy())
            playerAnim.SetValues(animMove, pc.mouseAngle);
        
        else if(pc.isInDialogue && !rewind.IsRewinding)
            playerAnim.SetValues(false, pc.mouseAngle);
        
    }
   
    void FixedUpdate()
    {
        if (!pc.IsBusy())
            rb2d.MovePosition(rb2d.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
