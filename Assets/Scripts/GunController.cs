using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(PlayerController))]
public class GunController : MonoBehaviour
{
    private Vector2 direction;
    private PlayerController pc;
    private void Awake()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    void faceMouse()
    {
        Vector2 mousePosition = pc.mousePosition;
        
        direction = new Vector2(
            mousePosition.x - transform.position.x, 
            mousePosition.y - transform.position.y
            );
    }
    
    void FixedUpdate()
    {
        faceMouse();
    }

    private void Update()
    {
        transform.right = -direction;
    }
}
