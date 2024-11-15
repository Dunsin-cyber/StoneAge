using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public LandStatus landStatus;

    public Material soilMat, farmlandMat, wateredMat;
    new Renderer renderer;

    //The selection gameobject to enable when the player is selecting the land
    public GameObject select;

    //Cache the time the land was watered 
    GameTimestamp timeWatered;



    [Header("Crops")]
    public GameObject cropPrefab;


    //The crop currently planted on the land
    CropBehaviour cropPlanted = null;

    // Start is called before the first frame update
    void Start()
    {
        //Get the renderer component
        renderer = GetComponent<Renderer>();

        //Set the land to soil by default
        SwitchLandStatus(LandStatus.Soil);

        //Deselect the land by default
        Select(false);
        //Add this to TimeManager's Listener list
        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        //Set land status accordingly
        landStatus = statusToSwitch;

        Material materialToSwitch = soilMat;

        //Decide what material to switch to
        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                //Switch to the soil material
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                //Switch to farmland material 
                materialToSwitch = farmlandMat;
                break;

            case LandStatus.Watered:
                //Switch to watered material
                materialToSwitch = wateredMat;
                //Cache the time it was watered
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;

        }

        //Get the renderer to apply the changes
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    {
        select.SetActive(toggle);
    }

    //When the player presses the interact button while selecting this land
    public void Interact()
    {
        //check the player's tool slot
        ItemData toolSlot = InventoryManager.Instance.equippedTool;


        //if there's nothing equipped, return
        if (toolSlot == null)
        {
            return;
        }

        // try casting the item data in the toolslot as equipment   
        EquipmentData equipmentTool = toolSlot as EquipmentData;


        //check if  it is of type EquipmentData
        if (equipmentTool != null)
        {
            //get the tool type
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(LandStatus.Watered);
                    break;
                case EquipmentData.ToolType.Shovel:
                    //remove the crop from the land
                    if (cropPlanted != null)
                    {

                        Destroy(cropPlanted.gameObject);
                    }
                    break;
            }
            // we don't need to check for seed if we have already comfirmed the tool to be an equipment
            return;
        }

        //try casting the itemdata in the toolslot as SeedData
        SeedData seedTool = toolSlot as SeedData;



        //conditions for the player to be able to plant a seed
        ///1. He is holding a tool of type SeedData
        ///2. Our land state must be either watered or farmland
        ///3. there isn't already a crop that has been planted
        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null)
        {
            //Instantiate  the crop object parented to the land
            GameObject cropObject = Instantiate(cropPrefab, transform);
            //move the crop object to the top of the land gameObject
            cropObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);


            //Access the cropBehaviour of the crop we're going to plant
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            //plant it 
            cropPlanted.Plant(seedTool);


        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Checked if 24 hours has passed since last watered
        if (landStatus == LandStatus.Watered)
        {
            //Hours since the land was watered
            int hoursElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hoursElapsed + " hours since this was watered");


            //Grow the planted crop, if any
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if (hoursElapsed > 24)
            {
                //Dry up (Switch back to farmland)
                SwitchLandStatus(LandStatus.Farmland);
            }
        }

        //Handle the wiltering of the plant
        if (landStatus != LandStatus.Watered && cropPlanted != null)
        {
            //If the crop has already germinated, start the wiltering
            if (cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wiither();
            }
        }
    }

}