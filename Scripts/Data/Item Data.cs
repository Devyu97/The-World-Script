using UnityEngine;

public enum ItemType
{
    None,
    Equipment, Consumable, Etc
}
public enum EquipmentType
{
    None, 
    Head, Chest, Gloves, Feet,
    MainHand, OffHand
}
public enum ConsumableType
{
    None,
    Hp, Food,
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Game/Item")]
public class ItemData : ScriptableObject
{
    public ItemType type;
    public string info;     //아이템 상세정보
    public Sprite icon;     //아이템 아이콘
    public int value;       //아이템 가치 

    //장비아이템인 경우에만 수치 조정.
    [Header("Equipment")]
    public EquipmentType equipmentType;
    public int hp;
    public int atk;
    public int def;

    //소비아이템인 경우에만 수치 조정.
    [Header("Consumable")]
    public ConsumableType consumableType;
    public int amount;
}
