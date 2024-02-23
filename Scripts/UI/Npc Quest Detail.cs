using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NpcQuestDetail : MonoBehaviour
{
    [Header("Set")]
    new public UnityEngine.GameObject name;
    public TextMeshProUGUI dialog;
    public TextMeshProUGUI reward;
    public UnityEngine.GameObject acceptButton;
    public TextMeshProUGUI acceptButtonText;

    private QuestData questData;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        questData = NpcInteractionUI.instance.openedQuest.GetComponent<NpcQuestSlot>().questData;
        name.GetComponent<TextMeshProUGUI>().text = questData.info.name;
        acceptButtonText.text = "수락";
        acceptButton.SetActive(true);

        //#. string Join(string separator, IEnumerable<string> values);
        // separator: 각 요소 사이에 삽입할 문자열
        // IEnumerable<string> values: 결합할 문자열이 들어있는 배열

        //퀘스트 지문
        if (questData.info.dialog != null) dialog.text = string.Join("\n", questData.info.dialog); 
        if (questData.info.objective.objectiveText != null) dialog.text += "\n\n퀘스트요약 : " + questData.info.objective.objectiveText;

        //퀘스트 보상
        if (questData.info.reward.exp != 0) reward.text = "경험치 : " + questData.info.reward.exp.ToString();
        if (questData.info.reward.glod != 0) reward.text += "\n골드 : " + questData.info.reward.glod.ToString();
        if (questData.info.reward.item != null) reward.text += "\n아이템 : " + questData.info.reward.item.name;

        if (NpcInteractionUI.instance.openedQuest.GetComponent<NpcQuestSlot>().isAccepted == true)
        {
            acceptButtonText.text = "완료";
            if(QuestManager.instance.QuestClearCheck(questData))
            {
                acceptButton.SetActive(true);
            }
            else
                acceptButton.SetActive(false);
        }

        UIManager.instance.ButtonInit(gameObject);
    }

    private void OnDisable()
    {
        if (NpcInteractionUI.instance == null) return;
        
        NpcInteractionUI.instance.openedQuest = null;
        questData = null;

        NpcInteractionUI.instance.closeButton.Select();
    }
}
