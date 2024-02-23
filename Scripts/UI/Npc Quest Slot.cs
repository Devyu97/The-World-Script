using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NpcQuestSlot : MonoBehaviour
{
    [Header("Set")]
    public UnityEngine.GameObject select;
    new public UnityEngine.GameObject name;
    public UnityEngine.GameObject icon;
    public bool isAccepted = false;

    [HideInInspector]
    public QuestData questData;

    private void OnEnable()
    {
        select.SetActive(false);
        UIManager.OnUI += ToggleSelectImage;
    }

    private void OnDisable()
    {
        UIManager.OnUI -= ToggleSelectImage;
    }

    private void Start()
    {
        name.GetComponent<TextMeshProUGUI>().text = questData.name;
    }

    private void ToggleSelectImage()
    {
        select.SetActive(EventSystem.current.currentSelectedGameObject == gameObject);
    }

    public void OnClick()
    {
        NpcInteractionUI.instance.openQuestDetail = true;
        NpcInteractionUI.instance.openedQuest = this;
        NpcInteractionUI.instance.questDetail.SetActive(true);
    }
}
