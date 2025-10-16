using System.Collections;
using UnityEngine;

public class EnemyBase : CharacterBase, IMovable
{
    [SerializeField] protected int damageToPlayer = 1;
    [SerializeField] protected GameObject enemyHitBox;
    [SerializeField] protected GameObject enemyFootHitBox;
    [SerializeField] protected float baseScaleX = 0.8f;
    [SerializeField] protected bool alreadySpawned = true;
    [SerializeField] protected bool isGrounded;
    [SerializeField] protected bool isFalling;
    [SerializeField] protected float enemySpeed = 0.15f;
    [SerializeField] protected int direction = 1; // 1 = direita, -1 = esquerda
    protected float maxTime = 5f;
    protected float minTime = 2.5f;
    protected float changeTime;
    protected Rigidbody2D enemyRB;
    protected SpriteRenderer enemySR;
    protected BoxCollider2D enemyBC;
    protected Animator animator = null;
    protected int takingDamageHash = Animator.StringToHash("takingDamage");
    protected int isRunningHash = Animator.StringToHash("isRunning");

    void Awake()
    {
        alreadySpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(AlreadySpawnedCoroutine());

        if (IsAlive())
        {
            if (!alreadySpawned && !IsGrounded() && IsFalling())
            {
                Die();
                Destroy(gameObject);
            }
            else
            {
                Walk();
            }
        }
    }

    void LateUpdate()
    {
        IsRunning();
        //OutOfCam();
        CorrectEnemyAxeXVelocity();
    }

    public void Walk()
    {
        if (alreadySpawned && !IsGrounded()) { return; }
        else
        {
            if (!IsGrounded()
                        || (Time.time >= changeTime && IsGrounded()))
            {
                SetRandomDirection();
            }

            animator.SetBool(isRunningHash, true);

            // --- Transição suave ao virar ---
            Vector3 scale = transform.localScale;

            float targetX = direction < 0 ? baseScaleX : -baseScaleX; // define o lado

            scale.x = Mathf.Lerp(scale.x, targetX, Time.deltaTime * 25f); // suaviza a mudança
            transform.localScale = scale;
            // -------------------------------

            // mover o bot na direção atual
            transform.Translate(Vector2.right * direction * enemySpeed * Time.deltaTime);
        }
    }

    private IEnumerator AlreadySpawnedCoroutine()
    {
        // Define um período de invencibilidade de 3 segundos
        yield return new WaitForSeconds(5f);

        alreadySpawned = false;
    }

    protected void SetRandomDirection()
    {
        // sortear -1 ou 1
        direction = Random.Range(0, 2) == 0 ? -1 : 1;

        // definir o tempo até a próxima troca
        changeTime = Time.time + Random.Range(minTime, maxTime);
    }

    public bool IsGrounded() {
        isGrounded = enemyFootHitBox.GetComponent<EnemyFootHitBox>().colidindoComChao;
        return isGrounded;
    }

    public bool IsFalling()
    {
        isFalling = Mathf.Abs(enemyRB.linearVelocityY) > 5f;
        return isFalling;
    }


    void IsRunning()
    {
        if (enemyRB.linearVelocityX > 0) animator.SetBool(isRunningHash, true);
        else animator.SetBool(isRunningHash, false);
    }

    protected void OutOfCam()
    {
        if (GameManagement.OutOfCam(gameObject))
        {
            TakeDamage(1);
        }
    }

    public override void TakeDamage(int damage)
    {
        animator.SetBool(takingDamageHash, true);
        base.TakeDamage(damage);
    }

    protected void OnDamageAnimationEnd()
    {
        animator.SetBool(takingDamageHash, false);
        Die();
    }

    public int GetDamageValue()
    {
        return damageToPlayer;
    }

    private void CorrectEnemyAxeXVelocity()
    {
        if (enemyRB.linearVelocityX > (enemySpeed * 1.5f))
        {
            enemyRB.linearVelocityX = enemySpeed;
        }
    }
}
