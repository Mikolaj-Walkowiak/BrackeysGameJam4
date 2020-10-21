using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int damage = 10;
    public float initialSpeed = -20f;
    public float range = 0f;
    public Rigidbody2D rb;
    private Inventory inventory;
    private Item currentItem;
    public GameObject shotBy;
    private ParticleSystem particles;
    private bool hitEnemy;
    void Start()
    {
	    hitEnemy = false;
	    
	    inventory = Inventory.instance;
	    currentItem = inventory.items[inventory.activeItem];
	    particles = GetComponent<ParticleSystem>();
	    
	    damage = currentItem.damage;
	    range = Mathf.Abs(currentItem.range/initialSpeed);
	    
	    rb.velocity = transform.right * initialSpeed;
	    Destroy(gameObject, range);
    }

    void OnTriggerEnter2D (Collider2D hitInfo){
	    if (hitInfo.CompareTag("Enemy"))
	    {
		    NPC enemy = hitInfo.GetComponent<NPC>();
		    if (enemy != null)
		    {
			    enemy.TakeDamage(damage);
			    hitEnemy = true;
		    }
	    }

	    if (hitInfo.gameObject == shotBy || hitInfo.CompareTag("ItemOnGround"))
		    return;
	    particles.Emit(10);
	    rb.velocity = Vector3.zero;
	    gameObject.GetComponent<SpriteRenderer>().enabled = false;
	    Destroy(gameObject, 0.3f);
    }

    public void Move()
    {
	    rb.velocity = transform.right * initialSpeed;
	    //Destroy(gameObject, range);
    }
}
