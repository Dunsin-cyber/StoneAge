using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [Header("Status Bar")]
    //tool equip slot on the status bar
    public Image toolEquipSlot;

    [Header("Inventory System")]
    // the inventory panel 
    public GameObject inventoryPanel;

    //the tool equip Ui slot on the inventory panel
    public HandInventorySlot toolHandSlot;
    //The tool slot UIs
    public InventorySlot[] toolSlots;

    //The item slot UIs
    public InventorySlot[] itemSlots;

    //the tool equip Ui slot on the inventory panel
    public HandInventorySlot itemHandSlot;

    //item info box
    public TMP_Text itemNameText;
    public TMP_Text itemDescriptionText;

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }

    private void Start()
    {
        RenderInventory();
        AssignSlotIndexes();
    }

    public void AssignSlotIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
        }
    }

    //Render the inventory screen to reflect the Player's Inventory. 
    public void RenderInventory()
    {
        //Get the inventory tool slots from Inventory Manager
        ItemData[] inventoryToolSlots = InventoryManager.Instance.tools;

        //Get the inventory item slots from Inventory Manager
        ItemData[] inventoryItemSlots = InventoryManager.Instance.items;

        //Render the Tool section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render the Item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Render the equipped slots
        toolHandSlot.Display(InventoryManager.Instance.equippedTool);
        itemHandSlot.Display(InventoryManager.Instance.equippedItem);


        //get tool equip from inventoryManager
        ItemData equippedTool = InventoryManager.Instance.equippedTool;
        // check if there is an item to display
        if (equippedTool != null)
        {

            // switch the thumbnail over
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);


            return;
        }

        toolEquipSlot.gameObject.SetActive(false);

    }

    //Iterate through a slot in a section and display them in the UI
    void RenderInventoryPanel(ItemData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            //Display them accordingly
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToggleInventoryPanel()
    {
        //if the panel is hidden, show it and vice versa
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        RenderInventory();
    }


    public void DisplayItemInfo(ItemData data)
    {
        //if data is null, reset
        if (data == null)
        {
            itemNameText.SetText("");
            itemDescriptionText.SetText("");

            return;
        }
        itemNameText.SetText(data.name);
        itemDescriptionText.SetText(data.description);
    }

}