using System.Collections.Generic;
using UnityEngine;

public enum NpcType
{
    None,
    Merchant
}
public class NpcManager : MonoBehaviour
{
    public NpcType type;
    public Vector2 position;
    public List<ItemData> items;
    public List<QuestData> quests; //�⺻ ���õ� ����Ʈ
    public List<QuestData> acceptedQuests;

    private void OnEnable()
    {
        GameManager.OnloadEvent += LoadData;
    }

    private void LoadData()
    {
        //����� npc�� ��ġ �ҷ���
        foreach (DataManager.NpcData npc in DataManager.instance.gameSaveData.npcs)
        {
            if (npc.name == this.name)
            {
                transform.position = npc.position;
            }
        }
        
        List<QuestData> remove = new List<QuestData>();

        foreach(QuestData quest in quests)
        {
            if(QuestManager.instance.acceptedQuests.Contains(quest))
            {
                acceptedQuests.Add(quest);
                remove.Add(quest);
            }
            if(QuestManager.instance.completedQuests.Contains(quest))
            {
                remove.Add(quest);
            }          
        }

        foreach(QuestData quest in remove)
        {
            quests.Remove(quest);
        }  
    }

    private void OnDisable()
    {
        GameManager.OnloadEvent -= LoadData;
    }

    public List<QuestData> GetQuestData()
    {
        if (quests != null)
            return quests;
        else
            return new List<QuestData>();
    }
}
