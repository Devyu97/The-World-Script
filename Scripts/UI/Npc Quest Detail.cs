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
        acceptButtonText.text = "����";
        acceptButton.SetActive(true);

        //#. string Join(string separator, IEnumerable<string> values);
        // separator: �� ��� ���̿� ������ ���ڿ�
        // IEnumerable<string> values: ������ ���ڿ��� ����ִ� �迭

        //����Ʈ ����
        if (questData.info.dialog != null) dialog.text = string.Join("\n", questData.info.dialog); 
        if (questData.info.objective.objectiveText != null) dialog.text += "\n\n����Ʈ��� : " + questData.info.objective.objectiveText;

        //����Ʈ ����
        if (questData.info.reward.exp != 0) reward.text = "����ġ : " + questData.info.reward.exp.ToString();
        if (questData.info.reward.glod != 0) reward.text += "\n��� : " + questData.info.reward.glod.ToString();
        if (questData.info.reward.item != null) reward.text += "\n������ : " + questData.info.reward.item.name;

        if (NpcInteractionUI.instance.openedQuest.GetComponent<NpcQuestSlot>().isAccepted == true)
        {
            acceptButtonText.text = "�Ϸ�";
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
