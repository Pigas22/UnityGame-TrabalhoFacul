using System.Data.Common;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EnemyBase : CharacterBase, IMovable
{
    [SerializeField] protected float enemySpeed = 0.1f;
    [SerializeField] protected int damageToPlayer = 1;
    [SerializeField] protected GameObject enemyHitBox;
    [SerializeField] protected GameObject enemyFootHitBox;
    protected Rigidbody2D enemyRB;
    protected SpriteRenderer enemySR;
    protected BoxCollider2D enemyBC;
    protected Animator animator;

    protected int direction = 1; // 1 = direita, -1 = esquerda
    protected float changeTime;
    protected float minTime = 0.5f;
    protected float maxTime = 1.5f;
    protected int takingDamageHash = Animator.StringToHash("takingDamage");
    protected int isRunningHash = Animator.StringToHash("isRunning");

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Walk();
        }
        else
        {
            Die();
        }
    }

    void LateUpdate()
    {
        IsRunning();
        OutOfCam();
    }

    public void Walk()
    {
        if (enemyFootHitBox.GetComponent<EnemyFootHitBox>().colidindoComChao)
        {
            animator.SetBool(isRunningHash, true);

            // mover o bot na direção atual
            transform.Translate(Vector2.right * direction * enemySpeed * Time.deltaTime);

            if (direction < 0)
                transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            else if (direction > 0)
                transform.localScale = new Vector3(-0.8f, 0.8f, 0.8f);

            // verificar se já é hora de mudar a direção
            if (Time.time >= changeTime)
            {
                SetRandomDirection();
            }
        }
        else
        {
            SetRandomDirection();
        }

    }

    void IsRunning()
    {
        if (enemyRB.linearVelocityX > 0) animator.SetBool(isRunningHash, true);
        else animator.SetBool(isRunningHash, false);
    }

    protected void SetRandomDirection()
    {
        // sortear -1 ou 1
        direction = Random.Range(0, 2) == 0 ? -1 : 1;

        // definir o tempo até a próxima troca
        changeTime = Time.time + Random.Range(minTime, maxTime);
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
        if (lives > 0)
        {
            animator.SetBool(takingDamageHash, false);
        }
        else
        {
            base.Die();
        }
    }

    public int GetDamageValue()
    {
        return damageToPlayer;
    }
}
