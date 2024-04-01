using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MonsterData;

public class MonsterAttack : MonoBehaviour
{
    public GameObject damagePrefab;
    public Rigidbody2D rb;
    public float attackSpeed = 10f;
    public float attackStartTime = 1f;
    public float attackCoolTime = 1f;

    private float time = 0f;
    private MonsterManager monster;
    private MonsterDetection detection;
    private bool isAttack = false;
    public bool charging = false;

    private void Awake()
    {
        isAttack = false;
        charging = false;
    }

    private void OnEnable()
    {
        isAttack = false;
        charging = false;
    }

    private void Start()
    {
        monster = gameObject.GetComponent<MonsterManager>();
        detection = gameObject.GetComponent<MonsterDetection>();
    }

    private void Update()
    {
        if (monster.curHp <= 0) return;

        if (detection.AttackRange() && isAttack == false)
        {
            charging = true;
            time += Time.deltaTime;
    
            if (time >= attackStartTime)
            {
                StartCoroutine(AttackRoutine());
                time = 0f;
            }
        }
        else
        {
            charging = false;
            time = 0f;
        }
    }

    private IEnumerator AttackRoutine()
    {
        isAttack = true;
        
        Vector3 position = transform.position; //공격전 위치
        Vector3 direction = PlayerManager.instance.transform.position; //공격할 방향
        direction = direction - position;
        direction.z = 0f;
        direction = direction.normalized;

        //공격
        transform.Translate(direction * attackSpeed * Time.deltaTime);
        yield return new WaitForSeconds(0.1f);

        //원래위치로 이동
        transform.Translate(-direction * attackSpeed * Time.deltaTime);

        //데미지입었을시 효과
        Particle.instance.ActiveParticle(transform.position, Particle.instance.damagedParticles);
        StartCoroutine(PlayerManager.instance.hitEffect());

        //데미지 수치 계산
        float maxDamage = monster.monsterData.info.atk;
        float minDamage = monster.monsterData.info.atk * 0.7f;
        float randomDamage = Random.Range(minDamage, maxDamage);
        float damage = randomDamage - PlayerManager.instance.def;
        if (damage > 1)
        {
            //데미지 수치 적용
            PlayerManager.instance.curHp -= damage;

            //데미지 텍스트 출력
            GameObject damageText = Instantiate(damagePrefab, PlayerManager.instance.damagePoint.transform);
            damageText.GetComponent<Damage>().SetDamageText(damage);
        }
        else
        {
            //데미지 수치 적용
            PlayerManager.instance.curHp -= 1;

            //데미지 텍스트 출력
            GameObject damageText = Instantiate(damagePrefab, PlayerManager.instance.damagePoint.transform);
            damageText.GetComponent<Damage>().SetDamageText();
        }
  
        //공격 쿨타임
        yield return new WaitForSeconds(attackCoolTime);
    
        isAttack = false;
        charging = false;
    }
}
