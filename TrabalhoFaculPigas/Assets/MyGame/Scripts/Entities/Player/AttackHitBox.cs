using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackHitBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private bool inimigoAoAlcance = false;
    [SerializeField] private bool caixaAoAlcance = false;
    [SerializeField] private int danoJogador = 1;
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

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // GameManagement.DebugLog("Player can hit a enemy!");
            inimigoAoAlcance = true;

            if (playerIsAttacking)
            {
                GameManagement.DebugLog("Atingiu um inimigo");
                collision.GetComponent<EnemyBase>().TakeDamage(danoJogador);
            }
        }

        if (collision.CompareTag("Box"))
        {
            //GameManagement.DebugLog("Player can hit a box!");
            caixaAoAlcance = true;

            if (playerIsAttacking)
            {
                BoxBase box = collision.GetComponent<BoxBase>();
                GameManagement.DebugLog("Atingiu uma caixa");
                box.TakeDamage(danoJogador);

                // StartCoroutine(GetBoxRewardVerification(box));
            }
        }
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
