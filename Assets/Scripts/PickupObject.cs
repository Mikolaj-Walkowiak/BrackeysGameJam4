using System.Collections;
using UnityEngine;

public class PickupObject : Interactable
{
    private Inventory inventory;
    public Item item;
    public AudioSource audio;
    public bool canPickup;
    private void Start()
    {
        inventory = Inventory.instance;
        audio = GetComponent<AudioSource>();
        inRange = false;
        audio.clip = Resources.Load<AudioClip>("SFX/GunPickup");
    }

    void Update()
    {
        if (inRange && Input.GetKeyDown("e"))
        {
            if(canPickup)
                StartCoroutine(PickUp());
        }
    }
    
    IEnumerator PickUp()
    {
        item = GetComponent<Item>();
        Debug.Log("Picking up" + item.itemName);
        bool pickedUp = inventory.Add(item);
        if (pickedUp)
        {
            canPickup = false;
            audio.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            while (audio.isPlaying)
                yield return null;
            gameObject.SetActive(false);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            canPickup = true;
        }

        //Destroy(gameObject);
    }

}
