using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Game/Monster")]
public class MonsterData : ScriptableObject
{
    public Monster info;

    [Serializable]
    public struct Monster
    {
        public string name;
        public Sprite sprite;
        public int hp;
        public int atk;
        public int def;
        public int exp;
        public int gold;
        public ItemData[] dropItems;
    }
}
