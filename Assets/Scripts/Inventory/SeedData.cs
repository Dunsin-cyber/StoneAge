using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed")]

public class SeedData : ItemData
{
    public int daysToGrow;


    public ItemData cropToYield;

    public GameObject seedling;

    [Header("Regrowable")]
    //is the plant able to regrow the crop after being harvested
    public bool regrowable;
    public int daysToRegrow;

}
