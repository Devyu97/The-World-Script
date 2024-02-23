using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance;

    public List<ItemData> items;//인벤토리의 아이템
    public int gold = 0;

    [Header("Set")]
    public Transform inventoryItemListContent;
    public GameObject itemSlotPrefab;

    private void Awake()
    {
        instance = this;

        items = new List<ItemData>();

        if(itemSlotPrefab == null )
        {
            itemSlotPrefab = Resources.Load<GameObject>("Prefabs/Player/Item Slot");
        }      
    }

    private void Start()
    {
        Test();
    }

    private void Test()
    {
        //ItemAcquisition("Slime Liquid");
        //ItemAcquisition("Slime Liquid");
        //ItemAcquisition("Slime Liquid");
        //ItemAcquisition("HP Potion");
        //ItemAcquisition("HP Potion");
        //ItemAcquisition("HP Potion");
    }

    public void ItemAcquisition(string item_name, bool message = false)
    {
        ItemData item = DataManager.instance.GetItemDataByName(item_name);

        //장비아이템이 아니고 이미 같은 아이템이 인벤토리에 있는경우
        //새로운 아이템 슬롯을 생성하지 않고 개수 증가
        if(items.Contains(item) && item.type != ItemType.Equipment)
        {
            GameObject temp = PlayerInventoryUI.instance.FindItemSlotByItemData(item);
            temp.GetComponent<ItemSlot>().count++;
            return;
        }

        //슬롯을 생성
        GameObject itemSlot = Instantiate(itemSlotPrefab);
        DontDestroyOnLoad(itemSlot);
        items.Add(item);

        //슬롯의 itemData와 세부정보 설정
        ItemSlot newItemSlot = itemSlot.GetComponent<ItemSlot>();
        newItemSlot.itemData = item;
        newItemSlot.SlotSetting();
        newItemSlot.DetailsSetting();
        PlayerInventoryUI.instance.itemSlots.Add(itemSlot);

        //GameMessage 
        if (message == false) return;
        GameMessageUI.instance.SystemMessage(item.name, Color.blue, "획득");        
    }
}
