using TMPro;
using UnityEngine;

public class PlayerQuestDetail : MonoBehaviour
{
    private QuestData questData;

    [Header("Set")]
    public TextMeshProUGUI questName;
    public TextMeshProUGUI objective;
    public TextMeshProUGUI reward;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        questData = PlayerQuestUI.instance.openedQuest.questData;
        questName.text = questData.info.name;

        //목표
        string temp = string.Join("\n", questData.info.dialog);
        objective.text = temp;

        objective.text += "\n\n퀘스트요약 : " + questData.info.objective.objectiveText;

        if(!QuestManager.instance.completedQuests.Contains(questData))
        {
            objective.text += "\n" + "진행상황 : " + "(" + questData.info.objective.curKillCount.ToString() + " / "
            + questData.info.objective.objectiveCount.ToString() + ")";
        }

        //보상
        if (questData.info.reward.exp != 0) reward.text = "경험치 : " + questData.info.reward.exp.ToString();
        if (questData.info.reward.glod != 0) reward.text += "\n골드 : " + questData.info.reward.glod.ToString();
        if (questData.info.reward.item != null) reward.text += "\n아이템 : " + questData.info.reward.item.name;

        UIManager.instance.ButtonInit(gameObject);
    }

    private void OnDisable()
    {
        if (PlayerQuestUI.instance == null) return;

        PlayerQuestUI.instance.openedQuest = null;
        questData = null;

        UIManager.instance.ButtonInit(PlayerQuestUI.instance.main);
    }

    public void OnClickBackButton()
    {
        PlayerQuestUI.instance.detail.SetActive(false);
        PlayerQuestUI.instance.main.SetActive(true);
    }

}
