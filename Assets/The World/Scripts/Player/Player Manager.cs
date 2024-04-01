using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Dictionary<EquipmentType, ItemData> equipments;
    public GameObject damagePoint;
    public Color hitColor = Color.white;
    [Serializable]
    public struct PlayerEquipStat
    {
        public float atk;
        public float def;
        public float hp;
        public float sp;
    }  

    
    
    private float tempTime = 0f;

    [Header ("Player Manager")]
    public Vector2 position;
    public bool playerControl = true;
    public bool isAttack = false;
    public bool isSlash = false;
    public int levelUpPoint = 5;

    [Header ("Player Stats")]
    public int level = 1;
    public float reqExp;
    public float curExp = 0;
    public float maxHp = 100;
    public float curHp;
    public float maxSp = 100;
    public float curSp;
    public float atk = 10;
    public float def = 10;
    public int str = 1;
    public int dex = 1;
    public int statPoint = 0;
    public PlayerEquipStat equipmentStat;


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

        equipments = new Dictionary<EquipmentType, ItemData>();
        equipmentStat = new PlayerEquipStat();
    }

    private void Start()
    {
        reqExp = level * 100;
    }

    private void Update()
    {
        position = transform.position;

        //상한 설정 
        if(curHp > maxHp)
            curHp = maxHp;
        if(curHp < 0)
            curHp = 0;
        if(curSp > maxSp) 
            curSp = maxSp;
        if(curSp < 0)
            curSp = 0;
        if(curExp >= reqExp)
        {
            LevelUp();
        }

        //색상 원래대로 
        Temp();

        //스텟 업테이트
        atk = (str * 2) + equipmentStat.atk;
        def = (dex * 2) + equipmentStat.def;
        maxHp = (str * 10) + equipmentStat.hp;
        maxSp = (dex * 10) + equipmentStat.sp;
        curHp += (str * 0.3f) * Time.deltaTime;
        curSp += (dex * 2f) * Time.deltaTime;
    }

    #region Function

    public void LevelUp()
    {
        GameMessageUI.instance.SystemMessage("레벨업!", Color.black);
        
        float over = curExp - reqExp;
        level++;
        reqExp = level * 100;
        curExp = over;

        statPoint += levelUpPoint;
    }

    public bool CheckEquip(EquipmentType type)
    {
        equipments.TryGetValue(type, out var equip);
        if (equip != null)
            return true;

        return false;
    }

    public void Equip(EquipmentType type, ItemData itemData)
    {
        //이미 장비를 장착한상태
        if (equipments.ContainsKey(type))
        {
            equipments[type] = itemData;
        }
        else
            equipments.Add(type, itemData);

        if(itemData != null)
        {
            equipmentStat.atk += itemData.atk;
            equipmentStat.def += itemData.def;
            equipmentStat.hp += itemData.hp;
        }
    }

    public void UnEquip(EquipmentType type, ItemData itemData)
    {
        if (equipments.ContainsKey(type))
        {
            equipments[type] = null;

            if (itemData != null)
            {
                equipmentStat.atk -= itemData.atk;
                equipmentStat.def -= itemData.def;
                equipmentStat.hp -= itemData.hp;
            }
        }
        else
            return;
    }

    private void Temp()
    {    
        if(gameObject.GetComponent<SpriteRenderer>().color == hitColor)
        {
            tempTime += Time.deltaTime;

            if(tempTime > 0.5f)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                tempTime = 0f;
            }
        }
    }

    public IEnumerator hitEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().color = hitColor;

        yield return new WaitForSeconds(0.2f);

        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
    #endregion
}
