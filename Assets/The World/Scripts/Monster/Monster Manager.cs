using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum MonsterAction
{ idle, move, attck }

public class MonsterManager : MonoBehaviour
{
    public UnityEvent OnDeath;
    public MonsterAction action;
    public MonsterData monsterData;
    public Transform damagePoint;
    public GameObject damagePrefab;
    public Color hitColor;
    public Vector3 spawnPos = Vector3.zero;
    public float curHp;

    private Rigidbody2D monsterRb;
    private SpriteRenderer monsterSpriteRender;
    private Color alpha = Color.white;
    private bool isDead = false;


    private void Awake()
    {
        monsterSpriteRender = GetComponent<SpriteRenderer>();
        monsterRb = gameObject.GetComponent<Rigidbody2D>();   
    }
    
    private void OnEnable()
    {
        action = MonsterAction.idle;
        isDead = false;

        transform.position = spawnPos;
        monsterSpriteRender.color = Color.white;
        
        if(monsterData != null)
        {
            curHp = monsterData.info.hp;
        }
    }

    private void Start()
    {
        action = MonsterAction.idle;
        isDead = false;

        transform.position = spawnPos;
        monsterSpriteRender.color = Color.white;

        if (monsterData != null) { curHp = monsterData.info.hp; }
    }

    private void Update()
    {
        CheckLife();
    }

    public void CheckLife()
    {
        if(isDead) return;

        //óġ�� ������ ���İ��� ���� ��������� ȿ��
        if (curHp <= 0 && monsterSpriteRender.color.a >= 0.3f)
        {
            alpha.a = Mathf.Lerp(monsterSpriteRender.color.a, 0, Time.deltaTime * 3f);
            monsterSpriteRender.color = alpha;

            if (monsterSpriteRender.color.a <= 0.3f) isDead = true;
        }

        if (isDead)
        {
            OnDeath.Invoke();

            //���� óġ�� ����ġ ȹ�� / ����ǰ ȹ��
            PlayerManager.instance.curExp += monsterData.info.exp;
            GameMessageUI.instance.SystemMessage(monsterData.info.exp.ToString(), Color.blue, "����ġ");

            //������� �ִ� ���
            if (monsterData.info.dropItems.Length != 0)
            {
                int index = Random.Range(0, monsterData.info.dropItems.Length);
                string itemName = monsterData.info.dropItems[index].name;
                PlayerInventory.instance.ItemAcquisition(itemName, true);
                GameMessageUI.instance.SystemMessage(itemName, Color.blue, "ȹ��");
            }

            QuestManager.instance.IncrementMonsterKillCount(monsterData);
            GameManager.instance.deadMonsters.Enqueue(gameObject);
            gameObject.SetActive(false);

            return;
        }

    }

    public IEnumerator hitEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().color = hitColor;

        yield return new WaitForSeconds(0.2f);

        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
