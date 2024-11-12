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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
