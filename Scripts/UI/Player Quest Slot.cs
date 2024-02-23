using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerQuestSlot : MonoBehaviour
{
    [Header("Set")]
    public UnityEngine.GameObject questName;
    public UnityEngine.GameObject select;

    [HideInInspector]
    public QuestData questData;

    private void Awake()
    {
        select.SetActive(false);      
    }

    private void OnEnable()
    {
        UIManager.OnUI += ToggleSelectImage;
    }

    private void Start()
    {
        if (questData == null) return;
        questName.GetComponent<TextMeshProUGUI>().text = questData.info.name;
    }

    private void OnDisable()
    {
        UIManager.OnUI -= ToggleSelectImage;
    }

    private void ToggleSelectImage()
    {
        select.SetActive(EventSystem.current.currentSelectedGameObject == gameObject);
    }
    public void OnClick()
    {
        PlayerQuestUI.instance.openedQuest = this;
        PlayerQuestUI.instance.main.SetActive(false);
        PlayerQuestUI.instance.detail.SetActive(true);
    }
}
