using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ItemSlot : MonoBehaviour
{
    public delegate void ItemDetails(ItemSlot itemSlot);
    public static event ItemDetails OnItemDetails;

    public ItemData itemData = null; 
    
    //슬롯의 고유 번호
    public static int next = 1;
    [SerializeField] private int ID;

    //상태
    public bool isDisplayed = false;
    public int count = 1;
    public TextMeshProUGUI countText;

    private Button itemSlotButton;

    [Header("Set")]
    public GameObject selectImage;
    public Image slotIcon;
    public TextMeshProUGUI slotName;
    public TextMeshProUGUI slotType;

    public GameObject detail;
    public TextMeshProUGUI detailName;
    public TextMeshProUGUI detailInfo;
    public TextMeshProUGUI value;
    public Button[] buttons;

    private void Awake()
    {
        ID = next;
        next++;

        selectImage.gameObject.SetActive(false);          
    }

    private void Start()
    {
        OnItemDetails += CloseAnotherItemSlotDetail;
        UIManager.OnUI += CloseItemDetails;
    }

    private void OnEnable()
    {
        UIManager.OnUI += ToggleSelectImage;
    }

    private void OnDisable()
    {
        UIManager.OnUI -= ToggleSelectImage;
    }

    private void OnDestroy()
    {
        OnItemDetails -= CloseAnotherItemSlotDetail;
        PlayerInventoryUI.instance.itemSlots.Remove(gameObject);
        UIManager.OnUI -= CloseItemDetails;
        UIManager.OnUI -= ToggleSelectImage;
    }

    private void Update()
    {
        countText.text = count.ToString();

        if (detail.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            detail.SetActive(false);
            itemSlotButton.Select();
        }

        if(detail.activeSelf)
        {
            GameObject selectObject = EventSystem.current.currentSelectedGameObject;

            if (selectObject == null || !IsDetailsButton(selectObject))
            {
                if (buttons[0].gameObject.activeSelf)
                    buttons[0].Select();
                else
                    buttons[1].Select();
            }
        }
    }

    #region Setting

    public void SlotSetting()
    {
        if (itemData == null) return;

        if(itemData.type == ItemType.Equipment)
        {
            countText.gameObject.SetActive(false);
        }

        if(itemData.type == ItemType.None ||  itemData.type == ItemType.Etc)
        {
            buttons[0].gameObject.SetActive(false);
        }

        slotIcon.sprite = itemData.icon;
        slotName.text = itemData.name;
        slotType.text = itemData.type.ToString();
    }

    public void DetailsSetting()
    {
        detail.SetActive(true);

        detailName.text = itemData.name;
        detailInfo.text = "";
        if (itemData.info != null) detailInfo.text += itemData.info;
        if (itemData.hp != 0) detailInfo.text += "\n추가 체력: " + itemData.hp;
        if(itemData.atk != 0) detailInfo.text += "\n추가 공격력: " + itemData.atk;
        if(itemData.def != 0) detailInfo.text += "\n추가 방어력: " + itemData.def;
        if(itemData.value != 0) value.text = "value: " + itemData.value;
        TextMeshProUGUI actionText = buttons[0].GetComponentInChildren<TextMeshProUGUI>();
        switch (itemData.type)
        {
            case ItemType.Equipment:
                actionText.text = "Equip";
                break;
            case ItemType.Consumable:
                actionText.text = "Use";
                break;
            case ItemType.None:
                actionText.text = "";
                break;
        }

        SetDetailsButtonNavigation();

        detail.SetActive(false);
    }

    private void SetDetailsButtonNavigation()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation navigation = new Navigation
            {
                mode = Navigation.Mode.Explicit,
                selectOnRight = buttons[(i + 1) % buttons.Length], //.GetComponent<Selectable>(),
                selectOnLeft = buttons[((i - 1) + buttons.Length) % buttons.Length]
            };
            buttons[i].navigation = navigation;
        }
    }

    #endregion


    #region Function
    private void ToggleSelectImage()
    {
        selectImage.SetActive(EventSystem.current.currentSelectedGameObject == gameObject);
    }

    private bool IsDetailsButton(GameObject gameObject)
    {
        foreach (Button button in buttons)
        {
            if (gameObject == button.gameObject)
                return true;
        }
        return false;
    }

    public void OpenItemDetails()
    {
        itemSlotButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        detail.SetActive(true);
        OnItemDetails(this);
    }

    private void CloseItemDetails()
    {
        if (PlayerInventoryUI.instance.gameObject.activeSelf == false)
        {
            detail.SetActive(false);
        }
    }

    private void CloseAnotherItemSlotDetail(ItemSlot itemSlot)
    {
        detail.SetActive(itemSlot == this);
    }

    private void EquipItem(ItemData itemData, EquipmentType equipmentType, Button equipmentSlot)
    {
        PlayerEquipmentSlot equipmentSlotComponent = equipmentSlot.GetComponent<PlayerEquipmentSlot>();

        //이미 장착한 장비가 있는경우
        if(equipmentSlotComponent.itemData != null)
        {
            //장비하고 있던 아이템을 인벤토리에 넣음
            PlayerInventory.instance.ItemAcquisition(equipmentSlotComponent.itemData.name);

            //장비상태와 스텟 업데이트
            PlayerManager.instance.UnEquip(equipmentType, equipmentSlotComponent.itemData);

            //장비슬롯의 아이템 데이터 비움
            equipmentSlotComponent.itemData = null;
        }

        //장비슬롯의 아이템 데이터를 장착할 아이템 데이터로 교체
        equipmentSlotComponent.itemData = itemData;
        //장비상태와 스텟 업데이트
        PlayerManager.instance.Equip(equipmentType, itemData);
    }
 
    public void ActionButton(ItemSlot itemSlot)
    {
        if (itemSlot == null) return;
        if (itemSlot.ID != ID) return;
     
        //소비 아이템일 경우
        if (itemData.type == ItemType.Consumable)
        {
            switch (itemData.consumableType)
            {
                //체력회복 아이템인 경우
                case ConsumableType.Hp:
                    PlayerManager.instance.curHp += itemData.amount;
                    break;
                //음식 아이템인 경우
                case ConsumableType.Food:
                    PlayerManager.instance.curSp += itemData.amount;
                    break;
            }                   
        }

        //장비 아이템일 경우
        if(itemData.type == ItemType.Equipment)
        {
            switch (itemData.equipmentType)
            {
                case EquipmentType.Head:
                    EquipItem(itemData, EquipmentType.Head, PlayerInventoryUI.instance.head);
                    break;
                case EquipmentType.Chest:
                    EquipItem(itemData, EquipmentType.Chest, PlayerInventoryUI.instance.chest);
                    break;
                case EquipmentType.Gloves:
                    EquipItem(itemData, EquipmentType.Gloves, PlayerInventoryUI.instance.gloves);
                    break;
                case EquipmentType.Feet:
                    EquipItem(itemData, EquipmentType.Feet, PlayerInventoryUI.instance.feet);
                    break;
                case EquipmentType.MainHand:
                    EquipItem(itemData, EquipmentType.MainHand, PlayerInventoryUI.instance.mainHand);
                    break;
                case EquipmentType.OffHand:
                    EquipItem(itemData, EquipmentType.OffHand, PlayerInventoryUI.instance.offHand);
                    break;
            }
        }

        //소비아이템의 개수가 1개 이상일 경우엔 카운트 감소
        if(itemSlot.count > 0 && itemSlot.itemData.type != ItemType.Equipment)
        {
            itemSlot.count--;
        }

        //카운트가 0이되거나 아이템인경우 슬롯 삭제, 
        if (itemSlot.count == 0 || itemSlot.itemData.type == ItemType.Equipment)
        {
            UpdateItemSlot(itemSlot);
            Destroy(gameObject);
        }
    }

    public void DeleteButton(ItemSlot itemSlot)
    {
        if (itemSlot == null)
            return;
        if (itemSlot.ID != ID)
            return;

        if (itemSlot.count > 0 && itemSlot.itemData.type != ItemType.Equipment)
        {
            itemSlot.count--;
        }

        if (itemSlot.count == 0 || itemSlot.itemData.type == ItemType.Equipment)
        {
            UpdateItemSlot(itemSlot);
            Destroy(gameObject);
        }
    }

    public void SellButton(ItemSlot itemSlot)
    {
        if (!NpcShop.instance.gameObject.activeSelf) return;
        if (itemSlot == null) return;
        if (itemSlot.ID != ID) return;
        
        PlayerInventory.instance.gold += itemSlot.itemData.value;

        if(PlayerInventory.instance.items.Count == 1)
        {
            ShopItemSlot temp = NpcShop.instance.itemListContent.GetComponentInChildren<ShopItemSlot>();
            temp.GetComponent<Button>().Select();                
        }

        if (itemSlot.count > 0 && itemSlot.itemData.type != ItemType.Equipment)
        {
            itemSlot.count--;
        }

        if (itemSlot.count == 0 || itemSlot.itemData.type == ItemType.Equipment)
        {
            UpdateItemSlot(itemSlot);
            Destroy(gameObject);
        }
    }

    private void UpdateItemSlot(ItemSlot itemSlot)
    {    
        Transform itemListContent = PlayerInventoryUI.instance.itemListContent;
        int lastIndex = 0; //활성화 되어있는 슬롯의 총 개수
        int curIndex = 0; //활성화 되어있는 슬롯중 자신의 인덱스 번호

        //활성화 되어있는 슬롯을 가져옴
        List<GameObject> items = new List<GameObject>(); 
        foreach(Transform item in itemListContent)
        {
            if(item.gameObject.activeSelf)
            {
                items.Add(item.gameObject);

                if(item.gameObject == itemSlot.gameObject)
                {
                    curIndex = items.Count - 1;
                }
            }
        }
        lastIndex = items.Count - 1;

        //동일 아이템 겹칠경우 수정추가
        //인벤토리에서 데이터 삭제(UI의 슬롯개수에는 영향x)
        PlayerInventory.instance.items.Remove(itemData); 

        // 마지막 슬롯이고 활성화된 슬롯이 한개가 아닐때,
        if (curIndex == lastIndex && lastIndex != 0)
        {
            items[curIndex - 1].GetComponent<Button>().Select();
        }
        // 마지막 슬롯이고 비활성화된 슬롯이 있는경우
        else if(curIndex == lastIndex && lastIndex == 0)
        {
            PlayerInventoryUI.instance.categoriAllButton.Select();
        }        
        // 아이템이 없을때(비활성화된 슬롯이 있을 수도 있음)
        else if (PlayerInventory.instance.items.Count == 0)
        {
            PlayerInventoryUI.instance.categoriAllButton.Select();
        }
        //그 외
        else
            items[curIndex + 1].GetComponent<Button>().Select();
    }
    #endregion
}
