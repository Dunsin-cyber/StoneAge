using System;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {

        //If there is more than one instance, destory the extra 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Tools")]
    //Tools Slots
    [SerializeField]
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    //Tool in the player's hand
    [SerializeField]
    private ItemSlotData equippedToolSlot = null;

    [Header("Items")]
    //Items Slots
    [SerializeField]
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    //Item in the player's hand
    [SerializeField]
    private ItemSlotData equippedItemSlot = null;

    public Transform handPoint;


    //Equipping
    // Handles movement of item from inventory to hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        //the slot to equip
        ItemSlotData handToEquip = equippedToolSlot;
        //the array to change
        ItemSlotData[] inventoryToAlter = toolSlots;


        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            //change slot to item
            handToEquip = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        //check if stackable
        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            //Add to the hand slot
            handToEquip.AddQuantity(slotToAlter.quantity);

            //empty the inventory slot
            slotToAlter.Empty();


        }
        else
        {
            //not stackable
            //cache the inventory itemSlotData
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            //change the inventory state to the hands
            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            EquipHandSlot(slotToEquip);

        }
        //update the changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {

            RenderHand();
        }

        //update the chnages in the UI
        UIManager.Instance.RenderInventory();


    }

    // Handles movement of item from hand to inventory
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        //the slot to move from (Tool by default)
        ItemSlotData handSlot = equippedToolSlot;
        //the array to change
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        // try stacking the hand slot
        //checked if the operation failed
        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            //find an empty slot to put the item in
            // iterate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //send equipment item over
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    //Remove the item from the hand
                    handSlot.Empty();
                    break;
                }
            }
        }


        //update the changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }

        //update changed in the inventory
        UIManager.Instance.RenderInventory();

    }


    //iterate throught each of the elements in the array to see if any can be stacked
    //will perform the stacking operation
    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                //Add to the inventory slot  stack
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                //Empty the item slot
                itemSlot.Empty();
                return true;
            }
        }
        //can't find a slot that can be stacked
        return false;


    }

    //Render the player's equipped item in the scene
    public void RenderHand()
    {
        //Reset objects on the hand
        if (handPoint.childCount > 0)
        {
            Destroy(handPoint.GetChild(0).gameObject);
        }

        //check if the player has anything equipped
        if (SlotEquipped(InventorySlot.InventoryType.Item))
        {
            //instantiate the game model on the player's hand and put it on the scene
            Instantiate(GetEquippedSlotItem(InventorySlot.InventoryType.Item).gameModel, handPoint);

        }
    }

    //Inventory Slot Data
    //Get the slot item ItemData
    #region Get and checks
    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot.itemData;
        }

        return equippedToolSlot.itemData;
    }

    //get function for the slots
    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot;
        }

        return equippedToolSlot;
    }


    //get function for the inventory slot
    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }

        return toolSlots;
    }

    //check if a hand slot has an item
    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return !equippedItemSlot.IsEmpty();
        }

        return !equippedToolSlot.IsEmpty();
    }
    #endregion



    //check if the item is a tool
    public bool IsTool(ItemData item)
    {
        //is it equipment?
        //try to cast ut as equipment data first
        EquipmentData equipment = item as EquipmentData;
        if (equipment != null)
        {
            return true;
        }

        //is it a seed?
        SeedData seed = item as SeedData;
        //if the seed is not null, it is a seed
        return seed != null;

    }
    // Equip the handslot with an Itemdata (will overwrite the slot)
    public void EquipHandSlot(ItemData item)
    {
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(item);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(item);
        }

    }

    // Equip the handslot with an Itemdata (will overwrite the slot)
    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        //Get the item data from the slot
        ItemData item = itemSlot.itemData;
        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }
    }


    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if (itemSlot.IsEmpty())
        {
            Debug.Log("There is nothing to consume");
            return;
        }
        //use up one of the item slots
        itemSlot.Remove();
        //refresh inventory
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    #region  Inventory slot validation
    private void OnValidate()
    {
        //validate the hand slot
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);

        //validate the slot in the inventory
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);

    }

    //when giving the item data value value in the inspector, automatically set quantity to 1
    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }

    }

    //validate arrays
    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }

    #endregion
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
