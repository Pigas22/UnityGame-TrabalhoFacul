using UnityEngine;

public class EnemyManager : CharacterBase, IMovable
{
    // [SerializeField] private float enemySpeed = 2f;
    // [SerializeField] private int damageToPlayer = 1;
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySR;
    private BoxCollider2D enemyBC;
    private Animator animator;
    private int takingDamageHash = Animator.StringToHash("takingDamage");

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyBC = GetComponent<BoxCollider2D>();

        animator = GetComponent<Animator>();
        // updateEnemyStats();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Walk()
    {
        throw new System.NotImplementedException();
        // bool walking = Physics2D.Raycast(playerCC.bounds.center, Vector2.down, playerCC.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
    }

    public void takeDamage(int damage)
    {
        animator.SetBool(takingDamageHash, true);
        lives -= damage;
    }

    private void OnDamageAnimationEnd()
    {
        if (lives > 0)
        {
            animator.SetBool(takingDamageHash, false);
        }
        else
        {
            Die();
        }
    }
}
