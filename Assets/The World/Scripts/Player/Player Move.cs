using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    public float moveSpeed;
    public Vector2 movement;

    private Rigidbody2D rb;

    void Awake()
    {
        instance = this;

        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (PlayerManager.instance.playerControl == false) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //monsterRb.MovePosition(monsterRb.position + movement.normalized * moveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if(PlayerManager.instance.playerControl == false) return;

        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
