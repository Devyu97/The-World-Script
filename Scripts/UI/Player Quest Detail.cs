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

        //��ǥ
        string temp = string.Join("\n", questData.info.dialog);
        objective.text = temp;

        objective.text += "\n\n����Ʈ��� : " + questData.info.objective.objectiveText;

        if(!QuestManager.instance.completedQuests.Contains(questData))
        {
            objective.text += "\n" + "�����Ȳ : " + "(" + questData.info.objective.curKillCount.ToString() + " / "
            + questData.info.objective.objectiveCount.ToString() + ")";
        }

        //����
        if (questData.info.reward.exp != 0) reward.text = "����ġ : " + questData.info.reward.exp.ToString();
        if (questData.info.reward.glod != 0) reward.text += "\n��� : " + questData.info.reward.glod.ToString();
        if (questData.info.reward.item != null) reward.text += "\n������ : " + questData.info.reward.item.name;

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
