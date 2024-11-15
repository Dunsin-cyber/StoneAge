using UnityEngine;

public class RegrowbaleHarvestBehaviour : InteractableObject
{
    CropBehaviour parentCrop;


    // Sets the paarent crop
    public void SetParent(CropBehaviour parentCrop)
    {
        this.parentCrop = parentCrop;
    }
    public override void Pickup()
    {
        //Set the player's inventory to the item
        InventoryManager.Instance.EquipHandSlot(item);

        //update the changes in the scene
        InventoryManager.Instance.RenderHand();


        //set the parent crop back to seedling to regrow it
        parentCrop.Regrow();

    }
}
