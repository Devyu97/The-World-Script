using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NpcShop : MonoBehaviour
{
    public static NpcShop instance;

    public ShopItemSlot selectedSlot;

    [Header("Set")]
    public UnityEngine.GameObject shopItemSlotPrefab;
    public UnityEngine.GameObject itemListContent;

    public UnityEngine.GameObject details;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemType;
    public TextMeshProUGUI itemInfo;
    public TextMeshProUGUI itemPrice;
    public Selectable purchaseButton;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        details.SetActive(false);

        NpcManager npc = NpcInteractionUI.instance.npc.GetComponent<NpcManager>();

        if(npc.items != null)
        {
            foreach(ItemData item  in npc.items)
            {
                UnityEngine.GameObject slot = Instantiate(shopItemSlotPrefab, itemListContent.transform);
                slot.GetComponent<ShopItemSlot>().itemData = item;
                slot.GetComponent<ShopItemSlot>().Set();
            }

            ShopItemSlot temp = itemListContent.GetComponentInChildren<ShopItemSlot>();
            temp.GetComponent<Button>().Select();
        }       
    }


    private void OnDisable()
    {
        if (NpcInteractionUI.instance == null) return;
        NpcInteractionUI.instance.characterInfoPanel.SetActive(true);
        NpcInteractionUI.instance.characterEquipmentPanel.SetActive(true);

        if (UIManager.instance == null) return;
        UIManager.instance.playerInventoryUI.SetActive(false);

        foreach (Transform child in itemListContent.transform)
        {
            if (child == null) return;

            Destroy(child.gameObject);
        }
    }

    public void DetailSetting()
    {
        if(selectedSlot != null)
        {
            itemName.text = selectedSlot.itemData.name;
            itemType.text = selectedSlot.itemData.type.ToString();
            itemInfo.text = selectedSlot.itemData.info;
            if (selectedSlot.itemData.hp != 0) itemInfo.text += "\n추가 체력: " + selectedSlot.itemData.hp;
            if (selectedSlot.itemData.atk != 0) itemInfo.text += "\n추가 공격력: " + selectedSlot.itemData.atk;
            if (selectedSlot.itemData.def != 0) itemInfo.text += "\n추가 방어력: " + selectedSlot.itemData.def;
            itemPrice.text = selectedSlot.itemData.value.ToString();
            purchaseButton.Select();
        }
    }

    public void OnClickPurchaseButton()
    {
        ItemData item = selectedSlot.itemData;

        // 돈부족시 경고창 띄우기(일단리턴)
        if (PlayerInventory.instance.gold < item.value) return;

        PlayerInventory.instance.gold -= item.value;
        PlayerInventory.instance.ItemAcquisition(item.name,true);

        selectedSlot.GetComponent<Button>().Select();
        selectedSlot = null;
        details.SetActive(false);
    }
}
