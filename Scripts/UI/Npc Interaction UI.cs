using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NpcInteractionUI : MonoBehaviour
{
    public static NpcInteractionUI instance;

    [HideInInspector] public bool openQuestDetail = false;
    [HideInInspector] public bool openShopWindow = false;
    [HideInInspector] public NpcQuestSlot openedQuest = null;
    [HideInInspector] public UnityEngine.GameObject npc = null;

    [Header("Set")]
    public UnityEngine.GameObject npcNameText;
    public UnityEngine.GameObject Main;
    public UnityEngine.GameObject questListContent;
    public UnityEngine.GameObject questDetail;

    public UnityEngine.GameObject shop;
    public UnityEngine.GameObject shopSlot;
    public UnityEngine.GameObject characterEquipmentPanel;
    public UnityEngine.GameObject characterInfoPanel;

    public Selectable closeButton;

    public Sprite QuestIcon;
    public Sprite AcceptedQuestIcon;
    public Sprite CompletedQuestIcon;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        OpenNpcInteractionUISetting();
    }

    private void OnDisable()
    {
        if (PlayerManager.instance == null) return;

        PlayerManager.instance.playerControl = true;
        foreach (Transform child in questListContent.transform)
        {
            if (child == null) return;

            Destroy(child.gameObject);
        }

        GameManager.curInteractingNpc = null;
        openQuestDetail = false;
        openShopWindow = false;
    }

    private void Update() 
    {
        if (openQuestDetail || openShopWindow)
        {
            Main.SetActive(false); 
        }
        else
        {
            Main.SetActive(true);
        }
    }

    #region Setting
    private void OpenNpcInteractionUISetting()
    {
        if (UIManager.instance == null) return;
        if (GameManager.curInteractingNpc == null) return;

        Main.SetActive(true);

        //#.npc 이름 설정
        npc = GameManager.curInteractingNpc;
        npcNameText.GetComponent<TextMeshProUGUI>().text = npc.GetComponent<NpcManager>().name;

        QuestSetting();
        ShopSetting();

        questDetail.SetActive(false);

        UIManager.instance.ButtonInit(gameObject);
    }

    private void QuestSetting()
    {
        foreach (QuestData quest in npc.GetComponent<NpcManager>().acceptedQuests)
        {
            if (quest == null) return;

            UnityEngine.GameObject temp = Resources.Load<UnityEngine.GameObject>("Prefabs/Npc/Quest Slot");
            UnityEngine.GameObject questSlot = Instantiate(temp, questListContent.transform);
            questSlot.GetComponent<NpcQuestSlot>().questData = quest;
            questSlot.GetComponent<NpcQuestSlot>().isAccepted = true;

            //퀘스트 클리어 유무에 따라 퀘스트슬롯의 아이콘 변경
            if (QuestManager.instance.QuestClearCheck(quest) == false)
                questSlot.GetComponent<NpcQuestSlot>().icon.GetComponent<Image>().sprite = AcceptedQuestIcon;
            if (QuestManager.instance.QuestClearCheck(quest) == true)
                questSlot.GetComponent<NpcQuestSlot>().icon.GetComponent<Image>().sprite = CompletedQuestIcon;
        }

        foreach (QuestData quest in npc.GetComponent<NpcManager>().quests)
        {
            if(quest == null) return;
            if(quest.info.requireLevel > PlayerManager.instance.level) return;

            UnityEngine.GameObject temp = Resources.Load<UnityEngine.GameObject>("Prefabs/Npc/Quest Slot");
            UnityEngine.GameObject questSlot = Instantiate(temp, questListContent.transform);
            questSlot.GetComponent<NpcQuestSlot>().questData = quest; 
        }


    }

    private void ShopSetting()
    {
        if (npc.GetComponent<NpcManager>().type == NpcType.Merchant)
        {
            shopSlot.SetActive(true);
        }
        else
            shopSlot.SetActive(false);
    }
    #endregion

    #region Function
    public void OnClickCloseButton()
    {
        GameManager.curInteractingNpc = null;

        questDetail.SetActive(false);
        openQuestDetail = false;

        shop.SetActive(false);
        openShopWindow = false;

        gameObject.SetActive(false);
    }

    public void QuestDetailBackButton()
    {
        openQuestDetail = false;
        questDetail.SetActive(false);
    }

    public void QuestDetailAcceptButton()
    {
        //퀘스트 수락 
        if(openedQuest.isAccepted == false)
        {
            npc.GetComponent<NpcManager>().acceptedQuests.Add(openedQuest.questData);
            QuestManager.instance.acceptedQuests.Add(openedQuest.questData);
            npc.GetComponent<NpcManager>().quests.Remove(openedQuest.questData);
            Destroy(openedQuest.gameObject);
            openQuestDetail = false;
            gameObject.SetActive(false);
        }

        //퀘스트 완료       
        else if(openedQuest.isAccepted == true)
        {
            //GameMessage
            GameMessageUI.instance.SystemMessage(openedQuest.questData.info.name, Color.yellow,  "퀘스트 완료");

            //QuestManager
            QuestManager.instance.acceptedQuests.Remove(openedQuest.questData);
            QuestManager.instance.completedQuests.Add(openedQuest.questData);

            //퀘스트 완료 보상
            PlayerManager.instance.curExp += openedQuest.questData.info.reward.exp;
            PlayerInventory.instance.gold += openedQuest.questData.info.reward.glod;
            if(openedQuest.questData.info.reward.item != null)
            {
                PlayerInventory.instance.ItemAcquisition(openedQuest.questData.info.reward.item.name);
            }

            //퀘스트 UI처리 
            npc.GetComponent<NpcManager>().acceptedQuests.Remove(openedQuest.questData);
            Destroy(openedQuest.gameObject);
            openQuestDetail = false;
            questDetail.SetActive(false);
        }

        closeButton.Select();
    }

    public void OnClickShopSlot()
    {
        openShopWindow = true;
        shop.SetActive(true);
        UIManager.instance.playerInventoryUI.SetActive(true);
        characterEquipmentPanel.SetActive(false);
        characterInfoPanel.SetActive(false);
    }

    #endregion
}
