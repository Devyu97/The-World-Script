using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private PlayerManager player;

    public float needSp;
    public UnityEngine.GameObject attack;
    public UnityEngine.GameObject n;
    public UnityEngine.GameObject w;
    public UnityEngine.GameObject e;
    public UnityEngine.GameObject s;

    private Animator animator;
    private int oritentation;

    private void Awake()
    {
        attack = gameObject;
    }

    private void Start()
    {
        player = PlayerManager.instance;
        animator = PlayerCharacter.instance.animator;

        foreach (Transform t in attack.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (player.playerControl == false) return;

        if(Input.GetKeyDown(KeyCode.A) && player.isAttack == false 
            && player.CheckEquip(EquipmentType.MainHand) && player.curSp > needSp)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        player.isAttack = true;
        player.playerControl = false;
        player.curSp -= needSp;

        oritentation = PlayerCharacter.instance.animator.GetInteger("orientation");
        switch(oritentation)
        {
            case 0:
                attack.transform.Find("S").gameObject.SetActive(true);
                break;
            case 3:
                attack.transform.Find("W").gameObject.SetActive(true);
                break;
            case 6:
                attack.transform.Find("E").gameObject.SetActive(true);
                break;
            case 9:
                attack.transform.Find("N").gameObject.SetActive(true);
                break;
        }

        //���� ������� �ִϸ��̼��� ���� ������
        int currentAnimatorIndex = animator.GetLayerIndex("Player");//�ִϸ����� Layer ���� "Player"�� �ε��� 
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(currentAnimatorIndex);
        
        //������� �ִϸ��̼��� ����ð� 
        float animationRunTime = stateInfo.length;

        yield return new WaitForSeconds(animationRunTime);

        player.isAttack = false;
        player.playerControl = true;

        switch (oritentation)
        {
            case 0:
                attack.transform.Find("S").gameObject.SetActive(false);
                break;
            case 3:
                attack.transform.Find("W").gameObject.SetActive(false);
                break;
            case 6:
                attack.transform.Find("E").gameObject.SetActive(false);
                break;
            case 9:
                attack.transform.Find("N").gameObject.SetActive(false);
                break;
        }
    }
}
