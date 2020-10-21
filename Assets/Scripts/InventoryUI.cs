using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    private Inventory inventory;
    public InventorySlot[] slots;
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        inventory.onItemScrolledCallback += UpdateActiveItem;
        
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        UpdateActiveItem();
    }

    void UpdateUI()
    {
        //Debug.Log("Updating UI");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                //Debug.Log("Adding item to slot" + i);
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                //Debug.Log("Clearing slot" + i);
                slots[i].ClearSlot();
            }

            if (i == inventory.inventorySize - 1)
            {
                UpdateActiveItem();
            }
        }
        //Debug.Log("Updating UI done");
        
    }

    void UpdateActiveItem()
    {
        //Debug.Log("UpdateActiveItem()");
        for (int i = 0; i < slots.Length; i++)
        {
            if (i == inventory.activeItem)
            {
                slots[inventory.activeItem].SetActive();
                if (inventory.items.Count - 1 >= inventory.activeItem)
                {
                    inventory.SetWeaponSprite(inventory.items[i].icon, new Vector2(inventory.items[i].positionX, inventory.items[i].positionY));
                }

                else
                {
                    inventory.RemoveWeaponSprite();
                }
            }
            else
            {
                slots[i].ClearActive();
            }
        }
        
        //Debug.Log("onItemScrolledCallback() done");
    }
    
 
}
