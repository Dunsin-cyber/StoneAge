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
    public ItemData[] tools = new ItemData[8];
    //Tool in the player's hand
    public ItemData equippedTool = null;

    [Header("Items")]
    //Items Slots
    public ItemData[] items = new ItemData[8];
    //Item in the player's hand
    public ItemData equippedItem = null;

    public Transform handPoint;


    //Equipping
    // Handles movement of item from inventory to hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            //cache the inventory slot itemData from InventoryManager and store it
            ItemData itemToEquip = items[slotIndex];

            //change the inventory slot to the hands'
            items[slotIndex] = equippedItem;

            //change the hand slot to the inventory slot
            equippedItem = itemToEquip;


            //Update the changes in the scene
            RenderHand();

        }
        else
        {
            //cache the inventory slot itemData from InventoryManager and store it
            ItemData toolToEquip = tools[slotIndex];

            //change the inventory slot to the hands'
            tools[slotIndex] = equippedTool;

            //change the hand slot to the inventory slot
            equippedTool = toolToEquip;
        }

        //update the changes
        UIManager.Instance.RenderInventory();

    }

    // Handles movement of item from hand to inventory
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            // iterate through each inventory slot and find an empty slot
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    //send equipment item over
                    items[i] = equippedItem;
                    //Remove the item from the hand
                    equippedItem = null;
                    break;
                }
            }
            //update the chnages in the scene
            RenderHand();
        }
        else
        {
            // iterate through each inventory slot and find an empty slot
            for (int i = 0; i < tools.Length; i++)
            {
                if (tools[i] == null)
                {
                    //send equipment item over
                    tools[i] = equippedTool;
                    //Remove the item from the hand
                    equippedTool = null;
                    break;
                }
            }

        }
        //update changed in the inventory
        UIManager.Instance.RenderInventory();

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
        if (equippedItem != null)
        {
            //instantiate the game model on the player's hand and put it on the scene
            Instantiate(equippedItem.gameModel, handPoint);

        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
