using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractKey : MonoBehaviour
{
    [SerializeField] 
    private float interactionRadius = 10f;
    [SerializeField]
    private LayerMask playerLayer;
    [SerializeField]
    private GameObject interactKey;


    private void Awake()
    {
        interactKey.SetActive(false);
    }

    private void Update()
    {
        playerLayer = LayerMask.GetMask("Player");

        Collider2D player = Physics2D.OverlapCircle(transform.position, interactionRadius, playerLayer);
        if(player != null)
        {
            interactKey.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
