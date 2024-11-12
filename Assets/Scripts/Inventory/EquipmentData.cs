using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment")]

public class Equipments : ItemData
{
    public enum ToolType
    {
        Hoe, WateringCan, Axe, Pickaxe
    }

    public ToolType toolType;
}
