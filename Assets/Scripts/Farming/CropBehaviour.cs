using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    //Information on what the crop will grow into
    SeedData seedToGrow;

    [Header("Stages of Life")]
    public GameObject seed;
    private GameObject seedling;
    private GameObject harvestable;

    //the grow points of the crop
    int growth;
    //how many growth points it takes before it becomes harvestable
    int maxGrowth;







    public enum CropState
    {
        Seed, Seedling, Harvestable
    }

    //the current stage in  the crop's growth
    public CropState cropState;



    //Initialization for the crop GameObject
    //Called when the player plants a seed
    public void Plant(SeedData seedToGrow)
    {
        this.seedToGrow = seedToGrow;


        //Instantiate the seedling and harvestable GameObjects
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item data
        ItemData cropToYield = seedToGrow.cropToYield;


        //Instantiate the harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);


        //convert Days to grow into hours
        int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);

        //convert into minutes
        maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);

        //Check if it's regrowable
        if (seedToGrow.regrowable)
        {
            //get the RegrowableHarvestBehaviour from the GameObject
            RegrowbaleHarvestBehaviour regrowbaleHarvest = harvestable.GetComponent<RegrowbaleHarvestBehaviour>();


            //initialize the harvestable
            regrowbaleHarvest.SetParent(this);
        }

        //set the inital state to seed
        SwitchState(CropState.Seed);
    }

    public void Grow()
    {
        //increase growth poiny by 1
        growth++;


        //the seed will sprout into a seedling when the growth is at 50%
        if (growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //fully grown
        if (growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }
    }


    //function to handle the state changes
    void SwitchState(CropState stateToSwitch)
    {

        //Reset everything and set all GameObjects to inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);



        switch (stateToSwitch)
        {
            case CropState.Seed:
                //Enable the Seed GameObject
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                //Enable the Seedling GameObject
                seedling.SetActive(true);
                break;
            case CropState.Harvestable:
                //Enable the Harvestable GameObject
                harvestable.SetActive(true);

                //if the seed is not regrowable, detach the harvestable from this crop gameobject and destroy it
                if (!seedToGrow.regrowable)
                {
                    //unparent it to the crop
                    harvestable.transform.parent = null;
                    Destroy(gameObject);
                }
                break;
        }


        //set the current crop state to the state we're switching to
        cropState = stateToSwitch;

    }


    //called when a player harvests a regrowable crop
    public void Regrow()
    {

    }
}
