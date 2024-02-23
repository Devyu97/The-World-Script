using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public delegate void LoadEvent();
    public static event LoadEvent OnloadEvent;

    //#.Scene
    [Header("Scene")]
    public string currentSceneName; //Gate 스크립트에 있는 transferSceneName 변수 값 저장
    public string beforeSceneName;

    //#.Camera
    [Header("Camera")]
    private UnityEngine.GameObject cameraLimitObject;
    public Dictionary<string, Transform> cameraLimits;
    public string curerntCameraLimitName; //현재 카메라 제한

    //#.Monster Pooling
    [Header("Monster Pooling")]
    public UnityEngine.GameObject MonsterPrefab = null;
    public List<MonsterData> monsterDatas;
    public List<Transform> spawnPoints = new List<Transform>();
    public List<UnityEngine.GameObject> monsters = new List<UnityEngine.GameObject>();
    public Queue<UnityEngine.GameObject> deadMonsters = new Queue<UnityEngine.GameObject>();

    //#.Npc
    public static UnityEngine.GameObject curInteractingNpc = null;

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

        OnloadEvent += SceneLoadSetting;
        cameraLimits = new Dictionary<string, Transform>(); 
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        //AudioManager.instance.PlayBgm(AudioManager.Bgm.Title);
    }


    #region Function

    public void LoadGameStart()
    {
        DataManager.instance.LoadPlayData();
        SceneManager.LoadScene(currentSceneName);
        CameraLimitSetting();
    }
    public void NewGameStart()
    {
        PlayerManager.instance.transform.position = new Vector3(0, 240,0);
        SceneManager.LoadScene("Forgotten Temple Grounds");
        CameraLimitSetting();

        foreach(QuestData quest in QuestManager.instance.Quests)
        {
            quest.isClear = false;
            quest.info.objective.curKillCount = 0;
        }

    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            OnloadEvent();
        }
    }
    #endregion

    #region Setting

    private void SceneLoadSetting()
    {
        CameraLimitSetting();
        PlayerPositionSetting();
        
        SpawnMonsters("Slime Grove", 0);
        SpawnMonsters("Cave", 1);

        //if (SceneManager.GetActiveScene().name == "Village Edenbrook")
        //    AudioManager.instance.PlayBgm(AudioManager.Bgm.VillageEdenbrook);
    }

    private void PlayerPositionSetting()
    {
        if (currentSceneName != beforeSceneName)
        {
            string temp = currentSceneName + " to " + beforeSceneName;
            GameObject gate = GameObject.Find(temp);
            if (gate != null)
            {
                PlayerManager.instance.transform.position = gate.transform.localPosition;
                Camera.main.GetComponent<CameraMovement>().SetPos(gate.transform.position);
            }
        }
    }
    #endregion

    #region Camera
    public UnityEngine.GameObject FindCameraLimitObject()
    {
        UnityEngine.GameObject cameraLimit = UnityEngine.GameObject.Find("CameraLimit"); // CameraLimit이름의 게임오브젝트 
        if (cameraLimit != null)
        {
            return cameraLimit;
        }

        return null; // CameraLimit 오브젝트를 찾지 못한 경우
    }
    public void LoadCameraLimit()
    {
        int temp = cameraLimitObject.transform.childCount;

        for (int i = 0; i < temp; i++)
        {
            Transform child = cameraLimitObject.transform.GetChild(i);

            cameraLimits[child.name] = child;
        }
    }
    private void CameraLimitSetting()
    {
        if (SceneManager.GetActiveScene().name == "Title Screen")
            return;

        cameraLimitObject = FindCameraLimitObject();

        LoadCameraLimit();
    }
    #endregion

    #region Monster
    private void SpawnMonsters(string SceneName, int monsterNum)
    {
        // 슬라임 그루브 맵 몬스터 풀링
        if (SceneManager.GetActiveScene().name == SceneName)
        {
            monsters = new List<UnityEngine.GameObject>();
            monsters.Clear();
            deadMonsters = new Queue<UnityEngine.GameObject>();
            deadMonsters.Clear();

            UnityEngine.GameObject spawnObject = UnityEngine.GameObject.Find("Spawn");
            if (spawnObject == null) return;

            spawnPoints = new List<Transform>();
            spawnPoints.Clear();

            foreach (Transform t in spawnObject.transform)
            {
                spawnPoints.Add(t);
            }

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                UnityEngine.GameObject monster = Instantiate(MonsterPrefab);
                MonsterManager manager = monster.GetComponent<MonsterManager>();
                manager.monsterData = monsterDatas[monsterNum];
                manager.OnDeath.AddListener(HandleMonsterDeath);
                manager.spawnPos = spawnPoints[i].position;
                monster.transform.position = spawnPoints[i].position;
                monsters.Add(monster);         
            }
        }
    }

    private void HandleMonsterDeath()
    {
        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(3f);

        if (deadMonsters.Count > 0)
        {
            UnityEngine.GameObject monster = deadMonsters.Dequeue();
            //monster.GetComponent<MonsterManager>().monsterData.info.
            monster.SetActive(true);
            yield break;
        }
    }
    #endregion
}
