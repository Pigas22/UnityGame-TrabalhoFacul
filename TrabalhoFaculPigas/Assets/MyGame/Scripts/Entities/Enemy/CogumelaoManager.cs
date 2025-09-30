using UnityEngine;

public class CogumeloManager : EnemyBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        gameObject.name = "Cogumelao";
        animator = GetComponent<Animator>();
        enemyRB = GetComponent<Rigidbody2D>();
        enemySR = GetComponent<SpriteRenderer>();
        enemyBC = GetComponent<BoxCollider2D>();

        enemySpeed = 1;
    }
}
