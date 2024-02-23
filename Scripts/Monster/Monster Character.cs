using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCharacter : MonoBehaviour
{
    private MonsterManager monster;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        monster = GetComponent<MonsterManager>();
    }

    private void Update()
    {
        if(monster.curHp <= 0) return;
        AnimationUpdate();
    }

    private void LateUpdate()
    {
        ChangeSprite();        
    }

    private void AnimationUpdate()
    { 
        switch (monster.action)
        {
            case MonsterAction.idle:
                animator.SetBool("charging_bool", false);
                animator.SetInteger("orientation", 0);
                break;
            case MonsterAction.move:
                animator.SetBool("charging_bool", false);
                if (PlayerManager.instance.transform.position.x < transform.position.x)
                    animator.SetInteger("orientation", 2);
                else 
                    animator.SetInteger("orientation", 4);
                break;
            case MonsterAction.attck:
                animator.SetBool("charging_bool", true);
                if (PlayerManager.instance.transform.position.x < transform.position.x)
                    animator.SetInteger("orientation", 2);
                else
                    animator.SetInteger("orientation", 4);
                break;
        }

    }

    private void ChangeSprite()
    {
        if (monster.monsterData.info.name == "Slime") return;

        string[] parts = spriteRenderer.sprite.name.Split('_'); // 스프라이트 번호. parts[1]
        string spriteKey = monster.monsterData.info.name + "_" + parts[1];
        if (!DataManager.instance.monsterSpriteSheet.ContainsKey(spriteKey)) return;

        spriteRenderer.sprite = DataManager.instance.monsterSpriteSheet[spriteKey];
    }


}
