using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerManager : CharacterBase, IMovable
{
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private List<CollectedItensInfo> collectedItensInfos = new List<CollectedItensInfo>();
    [SerializeField] private int totalScore = 0;
    [SerializeField] private GameObject playerFootHitBox;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isTakingDamage = false;
    [SerializeField] private bool canMove = true;

    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;
    private CapsuleCollider2D playerCC;
    private GameObject sceneManager;


    private Animator animator;
    private int isRunningHash = Animator.StringToHash("isRunning");
    private int isJumpingHash = Animator.StringToHash("isJumping");
    private int isGroundedHash = Animator.StringToHash("isGrounded");
    private int isTeleportingHash = Animator.StringToHash("isTeleporting");
    private int isTakingDamageHash = Animator.StringToHash("isTakingDamage");

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerFootHitBox = GameObject.Find("PlayerFootHitBox");
        playerFootHitBox.transform.SetParent(this.transform); // Torna a hitbox filha do jogador
        playerFootHitBox.transform.localPosition = new Vector3(0, -0.149f, 0); // Centraliza a hitbox no jogador

        lives = 6;
        UpdatePlayerStats();

        sceneManager = GameObject.Find("SceneManager");
        // transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
        TeleportToSpawn();

        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        playerCC = GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePlayerStats();
        // playerStats();

        if (isAlive)
        {
            Walk();
            Jump();
        }
        else
        {
            Die();
            Debug.Log("Player is dead. Game Over.");

        }
    }

    void LateUpdate()
    {
        if (isAlive)
        {
            OutOfCam();
        }

        if (!canMove && !isTakingDamage)
        {
            playerRB.linearVelocityX = 0;
            playerRB.linearVelocityY = 0;

            isJumping = false;
            isRunning = false;
            isGrounded = false;

            UpdatePlayerStats();
            UpdateAnimations();
        }
    }

    public void Walk()
    {
        float walkInput = Input.GetAxis("Horizontal");
        playerRB.linearVelocityX = walkInput * playerSpeed;

        if (walkInput != 0 && canMove)
        {
            // Debug.Log("Player velocity X: " + playerRB.linearVelocityX);

            if (walkInput > 0)
            {
                playerSR.flipX = false;
            }
            else if (walkInput < 0)
            {
                playerSR.flipX = true;
            }
        }

        animator.SetBool(isRunningHash, isRunning);
    }

    public void Jump()
    {
        if (Input.GetButtonDown("Jump")
                            // && Mathf.Abs(playerRB.linearVelocityY) < 0.001f
                            && isGrounded && canMove)
        {
            playerRB.linearVelocityY = jumpForce;
            UpdatePlayerStats();
        }

        animator.SetBool(isJumpingHash, isJumping);
        animator.SetBool(isGroundedHash, isGrounded);
    }

    // private bool canTeleport(Vector3 targetPosition)
    // {
    //     Collider2D[] colliders = Physics2D.OverlapBoxAll(targetPosition, playerCC.size, 0f);
    //     foreach (Collider2D collider in colliders)
    //     {
    //         Debug.Log("Collider found at target position: " + collider.name);
    //         if (collider != playerCC && !collider.isTrigger)
    //         {
    //             return false;
    //         }
    //     }
    //     return true;
    // }

    private void OutOfCam()
    {
        if (GameManagement.OutOfCam(gameObject) && !isTakingDamage)
        {
            TakeDamage(1);
        }

        // if (outOfXAxe)
        // {
        //     TakeDamage(1);

        //     // Vector3 newViewportPoint = viewportPoint;
        //     // newViewportPoint.x = (newViewportPoint.x < 0) ? 0.99f : 0.01f;
        //     // Vector3 targetPosition = GameManagement.viewPortPointToPosition(newViewportPoint);

        //     // if 
        //     // {
        //     //     teleportToSpawn();
        //     //     TakeDamage(1);
        //     // }
        // }
    }

    public override void TakeDamage(int amount)
    {
        canMove = false;
        isTakingDamage = true;

        StartCoroutine(TakingDamageRoutine());

        animator.SetBool(isTakingDamageHash, isTakingDamage);
        playerRB.linearVelocityY = jumpForce * 2;
        playerRB.linearVelocityX = 1;
        base.TakeDamage(amount);
    }

    void OnDamageAnimationEnd()
    {
        isTakingDamage = false;
        animator.SetBool(isTakingDamageHash, isTakingDamage);
        TeleportToSpawn();
    }

    void TeleportToSpawn()
    {
        canMove = false;
        animator.SetBool(isTeleportingHash, true);
        transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
    }

    private void OnTeleportAnimationEnd()
    {
        animator.SetBool(isTeleportingHash, false);
        canMove = true;
    }


    private void IsGrounded()
    {
        // Detectar a layer Ground com a Tag Chao
        // isGrounded = Physics2D.Raycast(playerCC.bounds.center, Vector2.down, playerCC.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
        isGrounded = playerFootHitBox.GetComponent<PlayerHitBox>().colidindoComChao || playerFootHitBox.GetComponent<PlayerHitBox>().colidindoComInimigo;
    }
    public bool PlayerIsGrounded()
    {
        return isGrounded;
    }

    private void IsJumping()
    {
        // Debug.Log("Jogador está pulando ? " + isJumping);
        isJumping = !isGrounded;
    }
    public bool PlayerIsJumping()
    {
        return isJumping;
    }

    private void IsRunning()
    {
        isRunning = Input.GetAxis("Horizontal") != 0;
    }
    public bool PlayerIsRunning()
    {
        return isRunning;
    }

    private IEnumerator TakingDamageRoutine()
    {
        isTakingDamage = true;
        // Define um período de invencibilidade de 0.5 segundos
        yield return new WaitForSeconds(0.5f); 
        isTakingDamage = false;
    }

    public bool PlayerIsTakingDamage()
    {
        return isTakingDamage;
    }

    private void UpdatePlayerStats()
    {
        IsGrounded();
        IsJumping();
        IsRunning();
        canMove = true;
    }

    private void UpdateAnimations()
    {
        animator.SetBool(isJumpingHash, isJumping);
        animator.SetBool(isGroundedHash, isGrounded);
        animator.SetBool(isRunningHash, isRunning);
        animator.SetBool(isTeleportingHash, false);
        animator.SetBool(isTakingDamageHash, isTakingDamage);
    }

    public void PlayerStats()
    {
        Debug.Log("Player Lives: " + lives);
        Debug.Log("Player isGrounded: " + isGrounded);
        Debug.Log("Player isJumping: " + isJumping);
        Debug.Log("Player Velocity X: " + playerRB.linearVelocityX);
        Debug.Log("Player Velocity Y: " + playerRB.linearVelocityY);
        Debug.Log("Player Position: " + transform.position);
    }

    public void AddScore((string name, int value) collectibleInfo)
    {
        totalScore += collectibleInfo.value;
        Debug.Log("Total Score: " + totalScore);

        // Procura se já existe o item na lista
        var found = collectedItensInfos.Find(item => item.NameItem == collectibleInfo.name);
        if (found != null)
        {
            collectedItensInfos.Remove(found);
            collectedItensInfos.Add(new CollectedItensInfo(collectibleInfo.name, collectibleInfo.value, found.Qtd + 1));
        }
        else
        {
            collectedItensInfos.Add(new CollectedItensInfo(collectibleInfo.name, collectibleInfo.value, 1));
        }
    }

    public int GetTotalScore()
    {
        return this.totalScore;
    }

    public List<CollectedItensInfo> GetCollectedItensInfos()
    {
        return this.collectedItensInfos;
    }
}
