using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    //전체퀘스트목록, 수락한퀘스트목록, 완료한퀘스트목록
    public List<QuestData> Quests;
    public List<QuestData> acceptedQuests;
    public List<QuestData> completedQuests;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
        #endregion

        acceptedQuests = new List<QuestData>();
        acceptedQuests.Clear();
        completedQuests = new List<QuestData>();
        completedQuests.Clear();
    }

    public void IncrementMonsterKillCount(MonsterData monster)
    {
        foreach(QuestData data in acceptedQuests)
        {
            if (data.isClear == true) continue;

            if (data.info.objective.monsterData == monster)
            {
                data.info.objective.curKillCount++;

                if (data.info.objective.curKillCount >= data.info.objective.objectiveCount)
                {
                    data.isClear = true;
                }
            }
        }
    }

    public bool QuestClearCheck(QuestData quest)
    {
        return quest.isClear;
    }

    public QuestData GetQuestDataByName(string questName)
    {
        QuestData quest = Quests.Find(x => x.info.name == questName);

        if (quest == null)
        {
            Debug.Log("cant found : " + questName);
            return null;
        }

        return quest;
    }
    
}
