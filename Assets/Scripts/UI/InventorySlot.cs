using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    ItemData itemToDisplay;
    int quantity;

    public Image itemDisplayImage;
    public TMP_Text quantityText;


    public enum InventoryType
    {
        Item, Tool
    }

    public InventoryType inventoryType;

    int slotIndex;
    public void Display(ItemSlotData itemSlot)
    {
        //set the variables accordingly
        itemToDisplay = itemSlot.itemData;
        quantity = itemSlot.quantity;

        //y default, qunatity text will not show anything
        quantityText.SetText("");

        // check if there is an item to display
        if (itemToDisplay != null)
        {

            // switch the thumbnail over
            itemDisplayImage.sprite = itemToDisplay.thumbnail;

            //display the stack qunatity if it is more than 1
            if (quantity > 1)
            {
                quantityText.SetText(quantity.ToString());
            }

            itemDisplayImage.gameObject.SetActive(true);


            return;
        }

        itemDisplayImage.gameObject.SetActive(false);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //move item from inventory to hand
        InventoryManager.Instance.InventoryToHand(slotIndex, inventoryType);
    }

    //set the slot index
    public void AssignIndex(int slotIndex)
    {
        this.slotIndex = slotIndex;
    }

    //Display the item info in the item info box when player mouses over
    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemToDisplay);
    }


    //Reset the item info in the item info box when player mouses over

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }

    public void AddMoreToolsQuantity()
    {
        Debug.Log("i have increased the quantity");
    }
}
