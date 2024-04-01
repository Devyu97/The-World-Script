using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    private PlayerManager player;

    public float needSp;
    public UnityEngine.GameObject slash;
    public UnityEngine.GameObject n;
    public UnityEngine.GameObject w;
    public UnityEngine.GameObject e;
    public UnityEngine.GameObject s;

    private Animator animator;
    private int oritentation;

    private void Awake()
    {
        slash = gameObject;
    }

    private void Start()
    {
        player = PlayerManager.instance;
        animator = PlayerCharacter.instance.animator;

        foreach (Transform t in slash.transform)
        {
            t.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (player.playerControl == false) return;

        if (Input.GetKeyDown(KeyCode.S) && player.isAttack == false
            && player.CheckEquip(EquipmentType.MainHand) && player.curSp > needSp)
        {
            StartCoroutine(SlashRoutine());
        }
    }

    private IEnumerator SlashRoutine()
    {
        player.isSlash = true;
        player.playerControl = false;
        player.curSp -= needSp;

        oritentation = PlayerCharacter.instance.animator.GetInteger("orientation");
        switch (oritentation)
        {
            case 0:
                slash.transform.Find("S").gameObject.SetActive(true);
                break;
            case 3:
                slash.transform.Find("W").gameObject.SetActive(true);
                break;
            case 6:
                slash.transform.Find("E").gameObject.SetActive(true);
                break;
            case 9:
                slash.transform.Find("N").gameObject.SetActive(true);
                break;
        }

        int currentAnimatorIndex = animator.GetLayerIndex("Player");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(currentAnimatorIndex);
        float animationRunTime = stateInfo.length;

        yield return new WaitForSeconds(animationRunTime);

        player.isSlash = false;
        player.playerControl = true;

        switch (oritentation)
        {
            case 0:
                slash.transform.Find("S").gameObject.SetActive(false);
                break;
            case 3:
                slash.transform.Find("W").gameObject.SetActive(false);
                break;
            case 6:
                slash.transform.Find("E").gameObject.SetActive(false);
                break;
            case 9:
                slash.transform.Find("N").gameObject.SetActive(false);
                break;
        }
    }
}
