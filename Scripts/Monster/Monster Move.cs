using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class MonsterMove : MonoBehaviour
{
    public Transform target;

    private MonsterManager monster;
    private MonsterDetection detection;
    private NavMeshAgent agent;   

    private void Start()
    {
        monster = GetComponent<MonsterManager>();
        detection = gameObject.GetComponent<MonsterDetection>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        if (monster.curHp <= 0) return;

        if(detection.AttackRange())
        {
            monster.action = MonsterAction.attck;
            agent.isStopped = true;
        }
        else if (detection.DetectionRange())
        {
            monster.action = MonsterAction.move;
            agent.isStopped = false;
            target = PlayerManager.instance.transform;
            agent.SetDestination(target.position);
        }
        else
        {
            monster.action = MonsterAction.idle;
            target = null;
            agent.isStopped = true;
        }
    }
}
