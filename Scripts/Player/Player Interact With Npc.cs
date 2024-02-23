using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractWithNpc : MonoBehaviour
{
    public LayerMask NpcLayer;

    [SerializeField] 
    private float interactionRadius = 15f;

    [SerializeField]
    private GameObject interactingKey;

    private void Awake()
    {
        interactingKey.SetActive(false);
    }

    private void Update()
    {
        interactingKey.SetActive(CheckInteract());

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (GameManager.curInteractingNpc == null)
            {
                Interacting();        
            }
        }    
    }

    private void Interacting()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        if (colliders != null)
        {
            Collider2D closestCollider = null;
            float closestDistance = float.MaxValue;

            foreach(Collider2D collider in colliders)
            {
                //#.비트 시프트 연산
                // '1 << 0'은 이진수에서 1을 왼쪽으로 0비트 이동시키는 것, 결과는 1.
                // '1 << 1'은 이진수에서 1을 왼쪽으로 1비트 이동시키는 것, 결과는 2.
                // '1 << 2'은 이진수에서 1을 왼쪽으로 2비트 이동시키는 것, 결과는 4.
                //#.비트 &(End)연산
                // 1101 (13)
                // 1010 (10) &연산
                // 1000 (8)  결과
                if((NpcLayer.value & (1 << collider.gameObject.layer)) != 0)
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);

                    if(distance < closestDistance)
                    {
                        closestCollider = collider;
                        closestDistance = distance;
                    }
                }
            }
            if(closestCollider != null)
            {
                print(" Interact With " + closestCollider.gameObject.name);
                GameManager.curInteractingNpc = closestCollider.gameObject;
                UIManager.instance.dialogUI.SetActive(true);
            }
        }
        else
        {
            GameManager.curInteractingNpc = null;
            return;
        }
    }
    private bool CheckInteract()
    {
        Collider2D[] npc = Physics2D.OverlapCircleAll(transform.position, interactionRadius, NpcLayer);

        if(npc.Length > 0)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
