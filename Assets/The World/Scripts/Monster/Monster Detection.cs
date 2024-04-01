using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    public LayerMask playerLayer; //�÷��̾��� ���̾�

    public float detectionRadius = 5f; //Ž�� �ݰ�
    public float attackRadius = 1.0f;

    public bool DetectionRange()
    {
        if (AttackRange() == true)
            return false;

        //�÷��̾��� �ݶ��̴� ����
        //#.Physics2D.OverlapCircle(Vector2 point, float radius, int layerMask) : point�� �߽����� radius �ݰ�ȿ� layerMask�� ���� �ݶ��̴� ��ȯ
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
        //Ž������ �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    
        //���ݹ��� �ð�ȭ
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
