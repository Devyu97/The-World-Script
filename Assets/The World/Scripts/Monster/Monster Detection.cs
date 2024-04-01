using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    public LayerMask playerLayer; //플레이어의 레이어

    public float detectionRadius = 5f; //탐지 반경
    public float attackRadius = 1.0f;

    public bool DetectionRange()
    {
        if (AttackRange() == true)
            return false;

        //플레이어의 콜라이더 감지
        //#.Physics2D.OverlapCircle(Vector2 point, float radius, int layerMask) : point를 중심으로 radius 반경안에 layerMask에 속한 콜라이더 반환
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (playerCollider != null )
            return true;

        return false;
    }
    
    public bool AttackRange()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, attackRadius, playerLayer);
        if (playerCollider != null)
            return true;

        return false;
    }

    private void OnDrawGizmos()
    {
        //탐지범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    
        //공격범위 시각화
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
