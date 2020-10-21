using System;
using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "New Item";
    public Sprite icon = null;
    public ItemType itemType = ItemType.None;
    public int damage = 0;
    public int range = 0;
    public int magSize = 0;
    public int currentAmmo = 0;
    public float shootingDelay = 0f;
    public AudioClip shotSound = null;
    public float positionX;
    public float positionY;
    public bool shot = false;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!other.gameObject.CompareTag("Walls"))
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
    }

    public IEnumerator ShootDelay()
    {
        shot = true;
        yield return new WaitForSeconds(shootingDelay);
        shot = false;
    }
}
