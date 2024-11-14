using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //the item information the GameObject is suppose to represent
    public ItemData item;


    public virtual void Pickup()
    {
        //Set the player's inventory to the item
        InventoryManager.Instance.equippedItem = item;

        //update the changes in the scene
        InventoryManager.Instance.RenderHand();
        //Destroy this instance so as to not have multiple copies
        Destroy(gameObject);
    }
}
