using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private bool inimigoAoAlcance = false;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private static readonly int attackTrigger = Animator.StringToHash("AttackTrigger");
    private bool playerIsAttacking;
    private bool previousAttackState = false;
    private SpriteRenderer playerRenderer;
    private Vector3 originalLocalPosition;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;

        playerRenderer = transform.parent.GetComponent<SpriteRenderer>();
        originalLocalPosition = transform.localPosition;

    }
    void Update()
    {
        var player = GameManagement.CurrentPlayer.GetComponent<PlayerManager>();
        playerIsAttacking = player.PlayerIsAttacking();

        if (playerIsAttacking && !previousAttackState)
        {
            animator.SetTrigger(attackTrigger);
            spriteRenderer.enabled = true;
        }

        previousAttackState = playerIsAttacking;
    }

    void LateUpdate()
    {
        transform.localPosition = new Vector3(
            playerRenderer.flipX ? -originalLocalPosition.x : originalLocalPosition.x,
            originalLocalPosition.y,
            originalLocalPosition.z
        );

        if (playerRenderer.flipX) spriteRenderer.flipX = false;
        else spriteRenderer.flipX = true;
    }

    void AttackRoutine(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Player can hit a enemy!");
            inimigoAoAlcance = true;

            if (playerIsAttacking)
            {
                Debug.Log("Atingiu um inimigo");
                collision.GetComponent<EnemyBase>().TakeDamage(1);
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     AttackRoutine(collision);
    // }

    void OnTriggerStay2D(Collider2D collision)
    {
        AttackRoutine(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        inimigoAoAlcance = false;
    }

    public void OnAttackAnimationEnd()
    {
        spriteRenderer.enabled = false;
    }
}
