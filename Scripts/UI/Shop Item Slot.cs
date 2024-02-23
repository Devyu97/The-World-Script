using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : MonoBehaviour
{
    public ItemData itemData;

    [Header("Set")]
    new public TextMeshProUGUI name;
    public Image icon;
    public TextMeshProUGUI price;
    
    public void Set()
    {
        if(itemData != null)
        {
            name.text = itemData.name;
            icon.sprite = itemData.icon;
            price.text = itemData.value.ToString();
        }
    }
    public void OnClick()
    {
        NpcShop.instance.selectedSlot = this;
        NpcShop.instance.DetailSetting();
        NpcShop.instance.details.SetActive(true);
    }
}
