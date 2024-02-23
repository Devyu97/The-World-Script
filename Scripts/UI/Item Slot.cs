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
    
    //������ ���� ��ȣ
    public static int next = 1;
    [SerializeField] private int ID;

    //����
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
        if (itemData.hp != 0) detailInfo.text += "\n�߰� ü��: " + itemData.hp;
        if(itemData.atk != 0) detailInfo.text += "\n�߰� ���ݷ�: " + itemData.atk;
        if(itemData.def != 0) detailInfo.text += "\n�߰� ����: " + itemData.def;
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

        //�̹� ������ ��� �ִ°��
        if(equipmentSlotComponent.itemData != null)
        {
            //����ϰ� �ִ� �������� �κ��丮�� ����
            PlayerInventory.instance.ItemAcquisition(equipmentSlotComponent.itemData.name);

            //�����¿� ���� ������Ʈ
            PlayerManager.instance.UnEquip(equipmentType, equipmentSlotComponent.itemData);

            //��񽽷��� ������ ������ ���
            equipmentSlotComponent.itemData = null;
        }

        //��񽽷��� ������ �����͸� ������ ������ �����ͷ� ��ü
        equipmentSlotComponent.itemData = itemData;
        //�����¿� ���� ������Ʈ
        PlayerManager.instance.Equip(equipmentType, itemData);
    }
 
    public void ActionButton(ItemSlot itemSlot)
    {
        if (itemSlot == null) return;
        if (itemSlot.ID != ID) return;
     
        //�Һ� �������� ���
        if (itemData.type == ItemType.Consumable)
        {
            switch (itemData.consumableType)
            {
                //ü��ȸ�� �������� ���
                case ConsumableType.Hp:
                    PlayerManager.instance.curHp += itemData.amount;
                    break;
                //���� �������� ���
                case ConsumableType.Food:
                    PlayerManager.instance.curSp += itemData.amount;
                    break;
            }                   
        }

        //��� �������� ���
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

        //�Һ�������� ������ 1�� �̻��� ��쿣 ī��Ʈ ����
        if(itemSlot.count > 0 && itemSlot.itemData.type != ItemType.Equipment)
        {
            itemSlot.count--;
        }

        //ī��Ʈ�� 0�̵ǰų� �������ΰ�� ���� ����, 
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
        int lastIndex = 0; //Ȱ��ȭ �Ǿ��ִ� ������ �� ����
        int curIndex = 0; //Ȱ��ȭ �Ǿ��ִ� ������ �ڽ��� �ε��� ��ȣ

        //Ȱ��ȭ �Ǿ��ִ� ������ ������
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

        //���� ������ ��ĥ��� �����߰�
        //�κ��丮���� ������ ����(UI�� ���԰������� ����x)
        PlayerInventory.instance.items.Remove(itemData); 

        // ������ �����̰� Ȱ��ȭ�� ������ �Ѱ��� �ƴҶ�,
        if (curIndex == lastIndex && lastIndex != 0)
        {
            items[curIndex - 1].GetComponent<Button>().Select();
        }
        // ������ �����̰� ��Ȱ��ȭ�� ������ �ִ°��
        else if(curIndex == lastIndex && lastIndex == 0)
        {
            PlayerInventoryUI.instance.categoriAllButton.Select();
        }        
        // �������� ������(��Ȱ��ȭ�� ������ ���� ���� ����)
        else if (PlayerInventory.instance.items.Count == 0)
        {
            PlayerInventoryUI.instance.categoriAllButton.Select();
        }
        //�� ��
        else
            items[curIndex + 1].GetComponent<Button>().Select();
    }
    #endregion
}
