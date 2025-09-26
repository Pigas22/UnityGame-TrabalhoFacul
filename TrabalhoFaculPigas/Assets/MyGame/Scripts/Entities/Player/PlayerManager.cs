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

    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;
    private CapsuleCollider2D playerCC;
    private Camera mainCamera;
    private GameObject sceneManager;


    private Animator animator;
    private int isRunningHash = Animator.StringToHash("isRunning");
    private int isJumpingHash = Animator.StringToHash("isJumping");
    private int isGroundedHash = Animator.StringToHash("isGrounded");
    private int isTeleportingHash = Animator.StringToHash("isTeleporting");

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
        mainCamera = Camera.main;

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
    }

    public void Walk()
    {
        float walkInput = Input.GetAxis("Horizontal");
        playerRB.linearVelocityX = walkInput * playerSpeed;

        if (walkInput != 0)
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
                            && isGrounded)
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
        Vector3 viewportPoint = GameManagement.positionToViewPortPoint(this.GameObject());
        bool outOfXAxe = viewportPoint.x < 0 || viewportPoint.x > 1;
        bool outOfYAxe = viewportPoint.y < 0 || viewportPoint.y > 1;

        if (outOfYAxe)
        {
            TeleportToSpawn();
            TakeDamage(1);
        }

        if (outOfXAxe)
        {
            TeleportToSpawn();
            TakeDamage(1);

            // Vector3 newViewportPoint = viewportPoint;
            // newViewportPoint.x = (newViewportPoint.x < 0) ? 0.99f : 0.01f;
            // Vector3 targetPosition = GameManagement.viewPortPointToPosition(newViewportPoint);

            // if 
            // {
            //     teleportToSpawn();
            //     TakeDamage(1);
            // }
        }
    }

    void TeleportToSpawn()
    {
        animator.SetBool(isTeleportingHash, true);
        transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
    }

    private void OnTeleportAnimationEnd()
    {
        animator.SetBool(isTeleportingHash, false);
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

    private void UpdatePlayerStats()
    {
        IsGrounded();
        IsJumping();
        IsRunning();
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
