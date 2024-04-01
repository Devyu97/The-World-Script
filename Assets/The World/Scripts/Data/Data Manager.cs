using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;


public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    [Serializable]
    public struct PlayerData
    {
        public string scene;
        public Vector2 position;
        public List<ItemData> inventory;
        public List<ItemData> equipment;
        public List<QuestData> acceptedQuests;
        public List<QuestData> completedQuests;

        public int level;
        public float reqExp;
        public float curExp;
        public float maxHp;
        public float curHp;
        public float maxSp;
        public float curSp;
        public float atk;
        public float def;
        public int str;
        public int dex;
        public int statPoint;
    }

    [Serializable]
    public struct NpcData
    {
        public string scene;
        public string name;
        public Vector2 position;
    }


    //Npc Dialog
    public struct NpcDialog
    {
        public string name; //npc이름
        public Dictionary<string, List<string>> dialog; //<대화명칭, 대화>
    }  
    public List<TextAsset> textFiles; //npc별 대화 텍스트 파일 
    public Dictionary<string, NpcDialog> npcDialog = new Dictionary<string, NpcDialog>(); //대화를 담을 맵 선언

    [Serializable]
    public struct GameSaveData
    {
        public PlayerData player;
        public List<NpcData> npcs;
    }
    public GameSaveData gameSaveData;

    private string saveFileName = "SaveData";
    private string saveFilePath;

    //#.Item
    [Header("Item")]
    public List<ItemData> itemDatas; //아이템 데이터를 담을 List선언

    //#.Quest Dialog Text
    public Dictionary<string, List<string>> questDialog;

    //#.Monster Sprite Sheet
    public Dictionary<string, Sprite> monsterSpriteSheet = new Dictionary<string, Sprite>();

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

        //#.SavePlayData 저장 경로.
        saveFilePath = Application.persistentDataPath + "/";

        //#.Item
        LoadItemData();

        //#.Quest Dialog Txt
        LoadQuestDialogTxt();

        //#.Npc Dialog Txt
        LoadNpcDialogTxt();

        //#.MonsterSprites
        SaveMonsterSprites();

    }

    private void Start()
    {
        //#.Quest
        QuestSetting();
    }

    private void Update()
    {
        //#.Save
        if(Input.GetKeyDown(KeyCode.F5))
        {
            SavePlayData();
            GameMessageUI.instance.SystemMessage("게임 저장 완료", Color.black);
        }
    }

    #region Item

    private void LoadItemData()
    {
        string itemDataPath = "Datas/Items"; //itemData가 저장되어있는 경로
        ItemData[] loadedItems = Resources.LoadAll<ItemData>(itemDataPath);

        itemDatas = new List<ItemData>(loadedItems);
    }

    public ItemData GetItemDataByName(string itemName)
    {
        //#.람다식을 사용해서 특정조건을 만족하는 요소(ItemData)를 찾음
        //'x' 는 리스트의 각 ItemData를 가리키는 변수.
        ItemData item = itemDatas.Find(x => x.name == itemName);

        if (item == null)
        {
            Debug.Log("cant found : " + itemName);
            return null;
        }

        return item; //찾은 아이템 데이터 반환
    }

    #endregion

    #region Quest

    private void QuestSetting()
    {
        //퀘스트 지문 설정
        QuestDialogSetting(QuestManager.instance.Quests);
    }

    private void LoadQuestDialogTxt()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Datas/Text/Quest Dialog");

        if (textAsset != null)
        {
            questDialog = new Dictionary<string, List<string>>();

            string[] quests = textAsset.text.Split(';');

            foreach (string line in quests)
            {
                if (line == null) continue;

                string[] split = line.Split(':');
                if (split.Length == 2)
                {
                    string questName = split[0].Trim();

                    string dialog = split[1].Trim();

                    //SplitOptions.RemoveEmptyEntries를 사용하면 빈 문자열인 ""이 배열에 포함되지 않는다. 
                    string[] dialogLines = dialog.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < dialogLines.Length; i++)
                    {
                        dialogLines[i] = dialogLines[i].Trim();
                    }

                    questDialog[questName] = dialogLines.ToList();
                }
            }       
        }
        else
        {
            print("퀘스트 지문 txt 파일 로드 실패");
        }
    }

    private void QuestDialogSetting(List<QuestData>questDatas)
    {
        foreach (QuestData questData in questDatas)
        {
            if (questDialog.TryGetValue(questData.info.name, out List<string> text))
            {
                questData.info.dialog = text;
            }
            else
            {
                questData.info.dialog = new List<string>();
            }
        }
    }

    #endregion

    #region Npc
    private void LoadNpcDialogTxt()
    {
        foreach (TextAsset text in textFiles)
        {
            NpcDialog npcDialogTemp = new NpcDialog();
            npcDialogTemp.name = text.name;
            npcDialogTemp.dialog = new Dictionary<string, List<string>>();

            string[] dialogSections = text.text.Split(';'); //대화목록 분류

            foreach (string sections in dialogSections)
            {
                string[] lines = sections.Split('/'); //대사 분류
                string key = lines[0].Trim().Trim('<', '>'); //대화명칭, 괄호 제거

                List<string> dialogs = new List<string>();
                for (int i = 1; i < lines.Length; i++)
                {
                    string[] temp = lines[i].Split(':'); //말하는이, 대사 분리
                    if (temp.Length == 2)
                    {
                        string speaker = temp[0].Trim();
                        string dialog = temp[1].Trim();
                        dialogs.Add(speaker + dialog);
                    }
                }
                npcDialogTemp.dialog[key] = dialogs;
            }

            if (npcDialogTemp.name != null)
            {
                npcDialog[npcDialogTemp.name] = npcDialogTemp;
                Debug.Log("success to load npc Dialog " + text.name);
            }
            else
            {
                Debug.Log("faild to load npc Dialog " + text.name);
            }
        }
    }

    #endregion

    #region Monster 
    private void LoadMonsterSprites(string spriteName)
    {
        string spriteFilePath;
        spriteFilePath = "Characters/Monsters" + "/" + spriteName;

        if (File.Exists(spriteFilePath))
        {
            print("Sprite file does not exist: " + spriteName);
            return;
        }

        Sprite[] additionalSprites = Resources.LoadAll<Sprite>(spriteFilePath);
        foreach (Sprite sprite in additionalSprites)
        {
            if (!monsterSpriteSheet.ContainsKey(sprite.name))
            {
                monsterSpriteSheet.Add(sprite.name, sprite);
            }
        }
    }

    private void SaveMonsterSprites()
    {
        LoadMonsterSprites("Slime");
        LoadMonsterSprites("Skeleton");
        print("Success Load Monster Sprites");
    }

    #endregion

    #region PlayData
    public void SavePlayData()
    {
        gameSaveData = new GameSaveData();
        gameSaveData.npcs = new List<NpcData>();

        gameSaveData.player.scene = SceneManager.GetActiveScene().name;

        gameSaveData.player.position = PlayerManager.instance.position;
        gameSaveData.player.level = PlayerManager.instance.level;
        gameSaveData.player.reqExp = PlayerManager.instance.reqExp;
        gameSaveData.player.curExp = PlayerManager.instance.curExp;
        gameSaveData.player.maxHp = PlayerManager.instance.maxHp;
        gameSaveData.player.curHp = PlayerManager.instance.curHp;
        gameSaveData.player.maxSp = PlayerManager.instance.maxSp;
        gameSaveData.player.curSp = PlayerManager.instance.curSp;
        gameSaveData.player.atk = PlayerManager.instance.atk;
        gameSaveData.player.def = PlayerManager.instance.def;
        gameSaveData.player.str = PlayerManager.instance.str;
        gameSaveData.player.dex = PlayerManager.instance.dex;
        gameSaveData.player.statPoint = PlayerManager.instance.statPoint;

        //#.Inventory
        gameSaveData.player.inventory = new List<ItemData>();
        gameSaveData.player.inventory.Clear();
        foreach (ItemData item in PlayerInventory.instance.items)
        {
            gameSaveData.player.inventory.Add(item);
        }

        //#.Equipment
        gameSaveData.player.equipment = new List<ItemData>();
        gameSaveData.player.equipment.Clear();
        foreach(var kvp in PlayerManager.instance.equipments)
        {
            gameSaveData.player.equipment.Add(kvp.Value);
        }

        //#.Quest 진행상황
        gameSaveData.player.acceptedQuests = new List<QuestData>();
        gameSaveData.player.completedQuests = new List<QuestData>();
        gameSaveData.player.acceptedQuests.Clear();
        gameSaveData.player.completedQuests.Clear();
        foreach (QuestData quest in QuestManager.instance.acceptedQuests)
        {
            gameSaveData.player.acceptedQuests.Add(quest);
        }
        foreach (QuestData quest in QuestManager.instance.completedQuests)
        {
            gameSaveData.player.completedQuests.Add(quest);
        }

        //#.Npc
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("Npc");
        foreach (GameObject npcObject in npcObjects)
        {
            NpcData temp = new NpcData();
            temp.name = npcObject.GetComponent<NpcManager>().name;
            temp.position = npcObject.transform.position;
            temp.scene = SceneManager.GetActiveScene().name;
            gameSaveData.npcs.Add(temp);
        }

        string data = JsonUtility.ToJson(gameSaveData, true);
        File.WriteAllText(saveFilePath + saveFileName, data);

        Debug.Log("Save Success");
        print(saveFilePath);
    }

    public void LoadPlayData()
    {
        try
        {
            string data = File.ReadAllText(saveFilePath + saveFileName);
            gameSaveData = JsonUtility.FromJson<GameSaveData>(data); 

            if (gameSaveData.player.scene == null || gameSaveData.npcs == null)
            {
                throw new Exception("e");
            }

            GameManager.instance.currentSceneName = gameSaveData.player.scene;
            PlayerManager.instance.position = gameSaveData.player.position;
            PlayerManager.instance.level = gameSaveData.player.level;
            PlayerManager.instance.reqExp = gameSaveData.player.reqExp;
            PlayerManager.instance.curExp = gameSaveData.player.curExp;
            PlayerManager.instance.maxHp = gameSaveData.player.maxHp;
            PlayerManager.instance.curHp = gameSaveData.player.curHp;
            PlayerManager.instance.maxSp = gameSaveData.player.maxSp;
            PlayerManager.instance.curSp = gameSaveData.player.curSp;
            PlayerManager.instance.atk = gameSaveData.player.atk;
            PlayerManager.instance.def = gameSaveData.player.def;
            PlayerManager.instance.str = gameSaveData.player.str;
            PlayerManager.instance.dex = gameSaveData.player.dex;
            PlayerManager.instance.statPoint = gameSaveData.player.statPoint;

            GameObject p = GameObject.FindWithTag("Player");
            p.transform.position = gameSaveData.player.position;
            
            //#.UIControl
            foreach(ItemData item in gameSaveData.player.inventory)
            {
                PlayerInventory.instance.ItemAcquisition(item.name);
            }

            //#.Quest
            foreach(QuestData quest in gameSaveData.player.acceptedQuests)
            {
                QuestManager.instance.acceptedQuests.Add(quest);
            }
            foreach (QuestData quest in gameSaveData.player.completedQuests)
            {
                QuestManager.instance.completedQuests.Add(quest);
            }

            //#.Equipment
            foreach(ItemData equipment in gameSaveData.player.equipment)
            {
                ItemData itemData = equipment;
                if(itemData != null)
                {
                    //아이템데이터의 장착타입에 따라 분류
                    EquipmentType type = itemData.equipmentType;
                    switch (type)
                    {
                        case EquipmentType.Head:
                            PlayerInventoryUI.instance.head.GetComponent<PlayerEquipmentSlot>().itemData = itemData;
                            PlayerManager.instance.Equip(EquipmentType.Head, itemData);
                            break;
                        case EquipmentType.Chest:
                            PlayerInventoryUI.instance.chest.GetComponent<PlayerEquipmentSlot>().itemData = itemData;
                            PlayerManager.instance.Equip(EquipmentType.Chest, itemData);
                            break;
                        case EquipmentType.Gloves:
                            PlayerInventoryUI.instance.gloves.GetComponent<PlayerEquipmentSlot>().itemData = itemData;
                            PlayerManager.instance.Equip(EquipmentType.Gloves, itemData);
                            break;
                        case EquipmentType.Feet:
                            PlayerInventoryUI.instance.feet.GetComponent<PlayerEquipmentSlot>().itemData = itemData;
                            PlayerManager.instance.Equip(EquipmentType.Feet, itemData);
                            break;
                        case EquipmentType.MainHand:
                            PlayerInventoryUI.instance.mainHand.GetComponent<PlayerEquipmentSlot>().itemData = itemData;
                            PlayerManager.instance.Equip(EquipmentType.MainHand, itemData);
                            break;
                        case EquipmentType.OffHand:
                            PlayerInventoryUI.instance.offHand.GetComponent<PlayerEquipmentSlot>().itemData = itemData;
                            PlayerManager.instance.Equip(EquipmentType.OffHand, itemData);
                            break;
                    }
                }          
            }

            print("Load Success");
            print(saveFilePath);
        }

        //File.ReadAllText 메서드는 파일을 찾을수 없거나 읽을 수 없을때 "FileNotFoundException" 을 throw 한다.
        catch (FileNotFoundException)
        {
            Debug.LogWarning("SaveFile Missing");
            print(saveFilePath);
            return;
        }
        catch(Exception e)
        {
            print(e);
            print(e.Data);
            Debug.LogWarning("예외처리");
            GameManager.instance.currentSceneName = "Village Edenbrook";
        }
    }
    #endregion

    public void NewGameSetting()
    {

    }
}
