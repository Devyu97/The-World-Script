using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemListClassification : MonoBehaviour
{
    enum classifyItem
    {all, equipment, consumable, etc }

    public Transform itemListContent;
    private classifyItem classify;

    private void Start()
    {
        classify = classifyItem.all;
    }

    private void Update()
    {
        switch (classify)
        {
            case classifyItem.all:
                break;
            case classifyItem.equipment:
                foreach (Transform item in itemListContent)
                {
                    if (item.GetComponent<ItemSlot>().itemData.type != ItemType.Equipment)
                    {
                        item.gameObject.SetActive(false);
                    }
                    else
                        item.gameObject.SetActive(true);
                }
                break;
            case classifyItem.consumable:
                foreach (Transform item in itemListContent)
                {
                    if (item.GetComponent<ItemSlot>().itemData.type != ItemType.Consumable)
                    {
                        item.gameObject.SetActive(false);
                    }
                    else
                        item.gameObject.SetActive(true);
                }
                break;
            case classifyItem.etc:
                foreach (Transform item in itemListContent)
                {
                    if (item.GetComponent<ItemSlot>().itemData.type != ItemType.Etc)
                    {
                        item.gameObject.SetActive(false);
                    }
                    else
                        item.gameObject.SetActive(true);
                }
                break;
        }
    }
    public void OnClickAllButton()
    {
        foreach (Transform item in itemListContent)
        {
            item.gameObject.SetActive(true);
        }

        classify = classifyItem.all;
    }

    public void OnClickEquipmentButton()
    {
        foreach (Transform item in itemListContent)
        {
            item.gameObject.SetActive(true);
        }

        classify = classifyItem.equipment;




    }

    public void OnClickConsumableButton()
    {
        foreach (Transform item in itemListContent)
        {
            item.gameObject.SetActive(true);
        }

        classify = classifyItem.consumable;
    }

    public void OnClickEtcButton()
    {
        foreach (Transform item in itemListContent)
        {
            item.gameObject.SetActive(true);
        }

        classify = classifyItem.etc;
    }
}
