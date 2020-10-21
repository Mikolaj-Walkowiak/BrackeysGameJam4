using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
        void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();

    public delegate void OnItemScrolled();

    public delegate void OnItemChangedAmmo();
    

    public OnItemChanged onItemChangedCallback;
    public OnItemScrolled onItemScrolledCallback;
    public OnItemChangedAmmo onItemChangedAmmoCallback;
    
    public List<Item> items = new List<Item>();
    public int inventorySize = 5;
    public int activeItem = 0;
    
    public SpriteRenderer gunRenderer;
    public GameObject Ground;

    private PlayerShooting playerShooting;
    public Transform shootPosition;
    public AudioSource audio;
    private void Start()
    {
        playerShooting = PlayerShooting.instance;
        audio = GetComponent<AudioSource>();
        audio.clip = Resources.Load<AudioClip>("SFX/Throw");
    }

    public bool Add(Item item)
    {
        if (items.Count >= inventorySize)
        {
            Debug.Log("Inventory full");
            return false;
        }

        items.Add(item);
        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        if (onItemChangedAmmoCallback != null && items.IndexOf(item) == activeItem)
        {
            onItemChangedAmmoCallback.Invoke();
        }
        return true;
    }

    public void Remove(Item item, Vector2 mousePosition)
    {
        //Debug.Log("Active item: " + activeItem);
        DropItem(item, mousePosition);
        items.Remove(item);
        ChangeActiveItem(-1);
        RemoveWeaponSprite();
        Debug.Log(items.Count);
        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
        
        //Debug.Log("Active item: " + activeItem);
    }
    
    public void ChangeActiveItem(int value)
    {
        //Debug.Log("ChangeActiveItem");
        if (activeItem + value < inventorySize)
        {
            if (activeItem + value >= 0)
            {
                activeItem += value;
                if (onItemScrolledCallback != null)
                    onItemScrolledCallback.Invoke();
            }
            else
            {
                activeItem = inventorySize - 1;
                if (onItemScrolledCallback != null)
                    onItemScrolledCallback.Invoke();
            }
        }
        else
        {
            activeItem = 0;
            if (onItemScrolledCallback != null)
                onItemScrolledCallback.Invoke();
        }
        if (onItemChangedAmmoCallback != null)
            onItemChangedAmmoCallback.Invoke();

        //Debug.Log("ChangeActiveItem done");
    }

    
    public void SetWeaponSprite(Sprite sprite, Vector2 weaponPosition)
    {
        gunRenderer.sprite = sprite;
        shootPosition.localPosition = new Vector3(weaponPosition.x, weaponPosition.y, 0f);
    }

    public void RemoveWeaponSprite()
    {
        gunRenderer.sprite = null;
    }
    private void Update() {
        
    }
    public void DropItem(Item item, Vector2 mousePosition)
    {
        //GameObject droppedItem = Instantiate(Resources.Load(item.itemName + "_OnGround")) as GameObject;
        //droppedItem.transform.parent = Ground.transform;
        GameObject droppedItem;
        
        if (Ground.transform.Find(item.gameObject.name) == null)
        {
            droppedItem = Instantiate(item.gameObject,
                new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            droppedItem.transform.parent = Ground.transform;
            
        }
        else
        {
            droppedItem = item.gameObject;
            droppedItem.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            droppedItem.SetActive(true);
        }
        droppedItem.GetComponent<PickupObject>().item = item;
        //necessary or it will add up
        droppedItem.GetComponent<Rigidbody2D>().angularVelocity = 0;
        droppedItem.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, 0, -1);
        
        ThrowObject(droppedItem, mousePosition);
    }
    void ThrowObject(GameObject droppedItem, Vector2 mousePosition){
        float initialSpeed = 1;
        droppedItem.GetComponent<Rigidbody2D>().velocity = new Vector3 ((mousePosition.x - transform.position.x) * initialSpeed, (mousePosition.y - transform.position.y) * 1.8f * initialSpeed, 0);
        droppedItem.GetComponent<Rigidbody2D>().angularVelocity = 1000;
        audio.Play();
        //droppedItem.GetComponent<Rigidbody2D>().angularVelocity = 0;
        
       
        
    }
}
