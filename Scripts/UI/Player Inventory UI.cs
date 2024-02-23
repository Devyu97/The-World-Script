using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInventoryUI : MonoBehaviour
{
    public static PlayerInventoryUI instance;

    public Selectable selectedSlot; //선택된 슬롯
    public Transform itemListContent;
    public List<GameObject> itemSlots = new List<GameObject>();

    public ScrollRect itemListScrollRect; //UIControl Item List Panel 의 ScrollRect 
    public Button categoriAllButton;
    
    [Header("Character Info / Stat")]
    public TextMeshProUGUI level;
    public TextMeshProUGUI exp;
    public TextMeshProUGUI Hp;
    public TextMeshProUGUI Sp;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI def;
    public TextMeshProUGUI str;
    public Button strBtn;
    public TextMeshProUGUI dex;
    public Button dexBtn;
    public TextMeshProUGUI statPoint;

    [Header("Inventory Gold")]
    public TextMeshProUGUI gold;

    [Header("Equipment Slot")]
    public Button head;
    public Button chest;
    public Button mainHand;
    public Button gloves;
    public Button feet;
    public Button offHand;

    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {      
        OpenInventorySettings();
    }

    private void Start()
    {
        gameObject.SetActive(false); //게임 시작시 인벤 닫음.
    }

    private void Update()
    {
        //Scroll();

        if(gameObject.activeSelf)
        {
            CharacterInfoSetting();
            ItemSlotSetting();
        }
    }

    #region Setting

    private void CharacterInfoSetting()
    {
        if (PlayerManager.instance == null) return;
        
        level.text = PlayerManager.instance.level.ToString();
        exp.text = PlayerManager.instance.curExp.ToString() + " / " + PlayerManager.instance.reqExp;
        int hp = (int)PlayerManager.instance.curHp;
        Hp.text = hp.ToString() + " / " + PlayerManager.instance.maxHp;
        int sp = (int)PlayerManager.instance.curSp;
        Sp.text = sp.ToString() + " / " + PlayerManager.instance.maxSp;
        atk.text = PlayerManager.instance.atk.ToString();
        def.text = PlayerManager.instance.def.ToString();
        str.text = PlayerManager.instance.str.ToString();
        dex.text = PlayerManager.instance.dex.ToString();
        statPoint.text = PlayerManager.instance.statPoint.ToString();

        gold.text = PlayerInventory.instance.gold.ToString();

        if(PlayerManager.instance.statPoint > 0)
        {
            strBtn.gameObject.SetActive(true);
            dexBtn.gameObject.SetActive(true);
        }
        else
        {
            strBtn.gameObject.SetActive(false);
            dexBtn.gameObject.SetActive(false);
        }
    }

    private void ItemSlotSetting()
    {
        foreach(GameObject itemSlot in itemSlots)
        {
            //if (!itemSlot.activeSelf) return;
            itemSlot.transform.SetParent(itemListContent, false);

            //ItemSlot temp = itemSlot.GetComponent<ItemSlot>();
            //if (temp.isDisplayed == false)
            //{
            //    temp.isDisplayed = true;                      
            //}       
        }
    }

    private void OpenInventorySettings()
    {      
        CharacterInfoSetting();
        ItemSlotSetting();

        if (itemListContent.childCount > 0)
        {
            itemListContent.GetChild(0).GetComponent<Button>().Select();
        }
        else
        {
            head.Select();
        }
    }

    #endregion


    #region Function

    public GameObject FindItemSlotByItemData(ItemData itemData)
    {
        foreach(GameObject itemSlot in itemSlots)
        {
            ItemSlot temp = itemSlot.GetComponent<ItemSlot>();
            if (temp.itemData == itemData)
            {
                return itemSlot;
            }              
        }
        return null;
    }

    private void Scroll()
    {
        //현재 선택된 슬롯 
        GameObject selectObject = EventSystem.current.currentSelectedGameObject;
        if (selectObject == null) return;
        selectedSlot = selectObject.GetComponent<Selectable>();

        if (selectedSlot.tag != "Item Slot") return;

        //선택된 Slot의 RectTransform을 받음
        RectTransform selectedSlotRect = selectedSlot.GetComponent<RectTransform>();
        RectTransform contentRect = itemListScrollRect.content.GetComponent<RectTransform>();

        //selectedSlot 의 위경계 아래경계가 viewport에 포함되면 retun;
        float slotRectHeightHalf = selectedSlotRect.rect.height * 0.5f;
        Vector3 up = selectedSlot.transform.position + new Vector3(0, slotRectHeightHalf,0);
        Vector3 down = selectedSlot.transform.position - new Vector3(0, slotRectHeightHalf, 0);

        if (RectTransformUtility.RectangleContainsScreenPoint(itemListScrollRect.viewport, up)
            && RectTransformUtility.RectangleContainsScreenPoint(itemListScrollRect.viewport, down))
        {
            return;
        }
      
        if (selectedSlotRect.offsetMin.y < itemListScrollRect.viewport.rect.yMin)
        {
            float temp = itemListScrollRect.viewport.rect.yMin - selectedSlotRect.offsetMin.y;
            Vector3 contentRectPos = contentRect.localPosition;
            contentRectPos.y += temp - contentRectPos.y;
            contentRect.localPosition = contentRectPos;
        }
        else
        {
            Vector3 contentRectPos = contentRect.localPosition;
            contentRectPos.y = 0;
            contentRect.localPosition = contentRectPos;
        }
    }

    public void OnClickIncreaseStrBtn()
    {
        PlayerManager.instance.statPoint--;
        PlayerManager.instance.str++;
    }
    public void OnClickIncreaseDexBtn()
    {
        PlayerManager.instance.statPoint--;
        PlayerManager.instance.dex++;
    }

    #endregion
}
