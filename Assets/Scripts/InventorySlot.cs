using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    private Item item;
    public Image icon;
    public Image activeImage;

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.GetComponent<Image>().enabled = true;

    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.GetComponent<Image>().enabled = false;
    }

    public void SetActive()
    {
        //Debug.Log("ItemSetActive()");
        activeImage.GetComponent<Image>().enabled = true;
    }
    
    public void ClearActive()
    {
        activeImage.GetComponent<Image>().enabled = false;
    }
}
