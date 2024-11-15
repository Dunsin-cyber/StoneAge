using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }
    [Header("Status Bar")]
    //tool equip slot on the status bar
    public Image toolEquipSlot;
    //Time UI
    public TMP_Text timeText;
    public TMP_Text dateText;

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


        //Add UIManager to the list of objects TimeManager will notify when the time updates
        TimeManager.Instance.RegisterTracker(this);
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
        //get the respective slots to process
        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);



        //Render the Tool section
        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        //Render the Item section
        RenderInventoryPanel(inventoryItemSlots, itemSlots);

        //Render the equipped slots
        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));


        //get tool equip from inventoryManager
        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);
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
    void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
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

    //Callback to handle the UI for time
    public void ClockUpdate(GameTimestamp timestamp)
    {

        //Handle the time
        //Get the hours and minutes
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        //AM or PM
        string prefix = "AM ";

        //Convert hours to 12 hour clock
        if (hours > 12)
        {
            //Time becomes PM 
            prefix = "PM ";
            hours = hours - 12;
            Debug.Log(hours);
        }


        //Format it for the time text display
        timeText.SetText(prefix + hours + ":" + minutes.ToString("00"));

        //Handle the Date
        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        //Format it for the date text display
        dateText.SetText(season + " " + day + " (" + dayOfTheWeek + ")");
    }

}