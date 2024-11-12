using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    ItemData itemToDisplay;

    public Image itemDisplayImage;


    public void Display(ItemData itemToDisplay)
    {

        // check if there is an item to display
        if (itemToDisplay != null)
        {

            // switch the thumbnail over
            itemDisplayImage.sprite = itemToDisplay.thumbnail;
            this.itemToDisplay = itemToDisplay;

            itemDisplayImage.gameObject.SetActive(true);


            return;
        }

        itemDisplayImage.gameObject.SetActive(false);
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

}
