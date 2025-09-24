using System.Linq.Expressions;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerManager : CharacterBase, IMovable
{
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
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
        updatePlayerStats();

        sceneManager = GameObject.Find("SceneManager");
        transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
        mainCamera = Camera.main;

        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        playerCC = GetComponent<CapsuleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        updatePlayerStats();
        // playerStats();

        if (isAlive)
        {
            Walk();
            jump();
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
            outOfCam();
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

    public void jump()
    {
        if (Input.GetButtonDown("Jump")
                   && Mathf.Abs(playerRB.linearVelocityY) < 0.001f
                            && isGrounded)
        {
            playerRB.linearVelocityY = jumpForce;
            updatePlayerStats();
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

    private void outOfCam()
    {
        Vector3 viewportPoint = GameManagement.positionToViewPortPoint(this.GameObject());
        bool outOfXAxe = viewportPoint.x < 0 || viewportPoint.x > 1;
        bool outOfYAxe = viewportPoint.y < 0 || viewportPoint.y > 1;

        if (outOfYAxe)
        {
            teleportToSpawn();
            TakeDamage(1);
        }

        if (outOfXAxe)
        {
            teleportToSpawn();
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

    void teleportToSpawn()
    {
        transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
    }


    private void IsGrounded()
    {
        // Detectar a layer Ground com a Tag Chao
        // isGrounded = Physics2D.Raycast(playerCC.bounds.center, Vector2.down, playerCC.bounds.extents.y + 0.1f, LayerMask.GetMask("Ground"));
        isGrounded = playerFootHitBox.GetComponent<PlayerHitBox>().colidindoComChao || playerFootHitBox.GetComponent<PlayerHitBox>().colidindoComInimigo;
    }
    public bool playerIsGrounded()
    {
        return isGrounded;
    }

    private void IsJumping()
    {
        // Debug.Log("Jogador está pulando ? " + isJumping);
        isJumping = !isGrounded;
    }
    public bool playerIsJumping()
    {
        return isJumping;
    }

    private void IsRunning()
    {
        isRunning = Input.GetAxis("Horizontal") != 0;
    }
    public bool playerIsRunning()
    {
        return isRunning;
    }

    private void updatePlayerStats()
    {
        IsGrounded();
        IsJumping();
        IsRunning();
    }


    public void playerStats()
    {
        Debug.Log("Player Lives: " + lives);
        Debug.Log("Player isGrounded: " + isGrounded);
        Debug.Log("Player isJumping: " + isJumping);
        Debug.Log("Player Velocity X: " + playerRB.linearVelocityX);
        Debug.Log("Player Velocity Y: " + playerRB.linearVelocityY);
        Debug.Log("Player Position: " + transform.position);
    }
}
