using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using static QuestData;

public class PlayerQuestUI : MonoBehaviour
{
    public static PlayerQuestUI instance;

    [Header("Set")]
    public UnityEngine.GameObject main;
    public UnityEngine.GameObject detail;

    public UnityEngine.GameObject acceptedQuestListContent;
    public UnityEngine.GameObject completedQuestListContent;

    public UnityEngine.GameObject playerQuestSlotPrefab;

    [HideInInspector] public PlayerQuestSlot openedQuest;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);      
    }

    private void OnEnable()
    {
        PlayerManager.instance.playerControl = false;
        main.SetActive(true);
        detail.SetActive(false);
        QuestSlotSetting();

        UIManager.instance.ButtonInit(main);
    }

    private void OnDisable()
    {
        if (PlayerManager.instance == null) return;

        PlayerManager.instance.playerControl = true;

        foreach (Transform child in acceptedQuestListContent.transform)
        {
            if (child == null) return;

            Destroy(child.gameObject);
        }

        foreach (Transform child in completedQuestListContent.transform)
        {
            if (child == null) return;

            Destroy(child.gameObject);
        }

        openedQuest = null;
    }

    #region Setting

    private void QuestSlotSetting()
    {
        foreach(QuestData quest in QuestManager.instance.acceptedQuests)
        {
            UnityEngine.GameObject questSlot = Instantiate(playerQuestSlotPrefab, acceptedQuestListContent.transform);
            questSlot.GetComponent<PlayerQuestSlot>().questData = quest;
        }

        foreach (QuestData quest in QuestManager.instance.completedQuests)
        {
            UnityEngine.GameObject questSlot = Instantiate(playerQuestSlotPrefab, completedQuestListContent.transform);
            questSlot.GetComponent<PlayerQuestSlot>().questData = quest;
        }
    }

    #endregion

}
