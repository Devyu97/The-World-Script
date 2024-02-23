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
    public string info;     //������ ������
    public Sprite icon;     //������ ������
    public int value;       //������ ��ġ 

    //���������� ��쿡�� ��ġ ����.
    [Header("Equipment")]
    public EquipmentType equipmentType;
    public int hp;
    public int atk;
    public int def;

    //�Һ�������� ��쿡�� ��ġ ����.
    [Header("Consumable")]
    public ConsumableType consumableType;
    public int amount;
}
