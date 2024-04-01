using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSlashCollider : MonoBehaviour
{
    public GameObject damagePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            MonsterManager monster = collision.gameObject.GetComponent<MonsterManager>();
            if (monster.curHp <= 0) return;

            //공격 이펙트 효과
            Particle.instance.ActiveParticle(collision.transform.position, Particle.instance.hitParticles);
            StartCoroutine(monster.hitEffect());

            //데미지 계산
            float maxDamage = PlayerManager.instance.atk;
            float minDamage = PlayerManager.instance.atk * 0.7f;
            float randomDamage = Random.Range(minDamage, maxDamage);
            float damage = randomDamage - monster.monsterData.info.def;
            if (damage > 1)
            {
                GameObject damageText = Instantiate(damagePrefab, monster.damagePoint);
                damageText.GetComponent<Damage>().SetDamageText(damage);
                monster.curHp -= damage;
            }
            else
            {
                GameObject damageInstance = Instantiate(damagePrefab, monster.damagePoint);
                damageInstance.GetComponent<Damage>().SetDamageText();
                monster.curHp -= 1;
            }
        }
    }
}
