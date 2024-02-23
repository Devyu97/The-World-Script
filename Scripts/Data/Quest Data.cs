using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewQuest", menuName = "Game/Quest")]
public class QuestData : ScriptableObject
{
    public Quest info;
    public bool isClear = false;

    [Serializable]
    public struct Quest
    {
        public string name;
        public int requireLevel;
        public QuestObjective objective;
        public Reward reward;
        public List<string> dialog;
    }

    [Serializable]
    public struct QuestObjective
    {
        public string objectiveText;

        public MonsterData monsterData;
        public int objectiveCount;
        public int curKillCount;
    }

    [Serializable]
    public struct Reward
    {
        public int exp;
        public int glod;
        public ItemData item;
    }
}
