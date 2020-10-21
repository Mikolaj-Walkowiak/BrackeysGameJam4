using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text ammoText; 
    public GameObject ammoUI;
    private Inventory inventory;
    private PlayerShooting playerShooting;

    private Item item;
    // Start is called before the first frame update
    void Start()
    {
     inventory = Inventory.instance;
     playerShooting = FindObjectOfType<PlayerShooting>();
     ammoUI.SetActive(false);
     inventory.onItemChangedAmmoCallback += UpdateAmmoUI;
     inventory.onItemScrolledCallback += UpdateAmmoUI;
     playerShooting.onShootAmmoChangedCallback += UpdateAmmoUI;
     UpdateAmmoUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateAmmoUI()
    {
        if (inventory.items.Count - 1 >= inventory.activeItem &&
            inventory.items[inventory.activeItem].itemType == ItemType.Gun)
        {
            ammoUI.SetActive(true);
            ammoText.text = inventory.items[inventory.activeItem].currentAmmo.ToString();
        }
        else
        {
            ammoUI.SetActive(false);
        }
    }
}
