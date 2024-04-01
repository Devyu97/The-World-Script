using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerEquipmentSlot : MonoBehaviour
{
    public ItemData itemData;
    public EquipmentType equipmentType;

    [Header("Set")]
    public GameObject select;
    public GameObject icon;
    [Header("Detail")]
    public GameObject detail;
    new public TextMeshProUGUI name;
    public TextMeshProUGUI info;
    public Button unequipButton;

    private void OnEnable()
    {
        UIManager.OnUI += ToggleSelectImage;
    }
    private void OnDisable()
    {
        UIManager.OnUI -= ToggleSelectImage;
    }
    private void Awake()
    {
        Init();
    }
    private void Update()
    { 
        if(UIManager.instance.playerInventoryUI.activeSelf)
        {
            DetailSetting();

            if (detail.activeSelf)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    detail.SetActive(false);
                    gameObject.GetComponent<Button>().Select();
                }
            }
        }      
    }
    private void ToggleSelectImage()
    {
        GameObject selectedSlot = EventSystem.current.currentSelectedGameObject;
        if(selectedSlot == gameObject || selectedSlot == unequipButton.gameObject)
        {
            select.SetActive(true);
        }
        else
        {
            select.SetActive(false);
            detail.SetActive(false);
        }
    }

    #region Setting
    private void Init()
    {
        select.SetActive(false);
        icon.SetActive(false);
        detail.SetActive(false);
    }

    private void DetailSetting()
    {
        if (itemData == null)
            return;
        
        icon.GetComponent<Image>().sprite = itemData.icon;
        icon.SetActive(true);

        name.text = itemData.name;
        info.text = "";
        if (itemData.info != null) info.text += itemData.info;
        if (itemData.hp != 0) info.text += "\n추가 체력: " + itemData.hp;
        if (itemData.atk != 0) info.text += "\n추가 공격력: " + itemData.atk;
        if (itemData.def != 0) info.text += "\n추가 방어력: " + itemData.def;
    }

    #endregion

    #region Function
    
    public void OnClickSlot()
    {
        if (itemData == null) return;

        detail.SetActive(true);
        unequipButton.Select();
    }

    public void OnClickUnequipButton()
    {
        PlayerInventory.instance.ItemAcquisition(itemData.name);
        PlayerManager.instance.UnEquip(equipmentType, itemData);
        itemData = null;

        detail.SetActive(false);
        icon.SetActive(false);
        gameObject.GetComponent<Button>().Select();
    }

    #endregion
}
