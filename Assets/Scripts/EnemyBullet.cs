using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage = 10;
    public float initialSpeed = -20f;
    public float range = 0f;
    public Rigidbody2D rb;
    public Item item;
    public GameObject shotBy;
    private ParticleSystem particles;
    private bool hitEnemy;
    void Start()
    {
        hitEnemy = false;
        particles = GetComponent<ParticleSystem>();
	    
        damage = item.damage;
        range = Mathf.Abs(item.range/initialSpeed);
	    
        rb.velocity = transform.right * initialSpeed;
        Destroy(gameObject, range);
    }

    void OnTriggerEnter2D (Collider2D hitInfo){
        if (hitInfo.CompareTag("Player") && !hitEnemy)
        {
            PlayerHealth enemy = hitInfo.GetComponent<PlayerHealth>();
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
