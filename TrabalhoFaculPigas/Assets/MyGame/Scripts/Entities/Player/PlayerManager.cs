using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerManager : CharacterBase, IMovable
{ 
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    
    [SerializeField] private List<CollectedItensInfo> collectedItensInfos = new List<CollectedItensInfo>();
    [SerializeField] private int totalScore = 0;
    [SerializeField] private GameObject playerFootHitBox;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool canDoubleJump = false;
    [SerializeField] private bool isRunning = false;
    [SerializeField] private bool isTakingDamage = false;
    [SerializeField] private bool canMove = true;
    [SerializeField] private float disableTime = 0.7f; // tempo que a colisão fica desativada
    private Rigidbody2D playerRB;
    private SpriteRenderer playerSR;
    private CapsuleCollider2D playerCC;
    private GameObject sceneManager;
    private Vector3 spawnpoint;

    public Action<int, bool> OnHealthChanged;
    private Animator animator;
    private int isRunningHash = Animator.StringToHash("isRunning");
    private int isJumpingHash = Animator.StringToHash("isJumping");
    private int isGroundedHash = Animator.StringToHash("isGrounded");
    private int isTeleportingHash = Animator.StringToHash("isTeleporting");
    private int isTakingDamageHash = Animator.StringToHash("isTakingDamage");
    private int isDoubleJumpingHash = Animator.StringToHash("isDoubleJumping");

    void Awake()
    {
        InitConfigHPChar(6);
        animator = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerFootHitBox = GameObject.Find("PlayerFootHitBox");
        playerFootHitBox.transform.SetParent(this.transform); // Torna a hitbox filha do jogador
        playerFootHitBox.transform.localPosition = new Vector3(0, -0.149f, 0); // Centraliza a hitbox no jogador

        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        playerCC = GetComponent<CapsuleCollider2D>();

        sceneManager = GameObject.Find("SceneManager");
        spawnpoint = sceneManager.GetComponent<SceneManagerModel>().GetPlayerSpawnPoint();
        // transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();

        UpdatePlayerStats();
        TeleportToSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        // playerStats();

        if (IsAlive())
        {
            if (isTakingDamage) return;
            Walk();
            Crouch();
            Jump();
        }
    }

    void LateUpdate()
    {
        UpdatePlayerStats();
        UpdateAnimations();

        if (OutOfCam() && !isTakingDamage) {
            TakeDamage(1);
        }
    }

    public void Walk()
    {
        float walkInput = Input.GetAxis("Horizontal");

        if (walkInput != 0 && canMove)
        {
            playerRB.linearVelocityX = walkInput * playerSpeed;
            // Debug.Log("Player velocity X: " + playerRB.linearVelocityX);

            if (walkInput > 0)
            {
                playerSR.flipX = false;
            }
            else if (walkInput < 0)
            {
                playerSR.flipX = true;
            }

            animator.SetBool(isRunningHash, isRunning);
        }
        else if (walkInput == 0 && !isTakingDamage)
        {
            playerRB.linearVelocityX = 0;
        }

    }

    public void Crouch()
    {
        if (canMove &&
                    (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
        {
            Debug.Log("Agachando");

            GameObject groundObject = GameObject.FindGameObjectWithTag("Chao");
            TilemapCollider2D groundCollider = groundObject?.GetComponent<TilemapCollider2D>();

            if (groundCollider != null)
            {
                StartCoroutine(DisableCollisionTemporarily(groundCollider));
            }
        }
    }

    public void Jump()
    {
        if (Input.GetButtonUp("Jump"))
        {
            if (isGrounded && canMove)
            {
                playerRB.linearVelocityY = jumpForce;
                animator.SetBool(isGroundedHash, isGrounded);
                animator.SetBool(isJumpingHash, isJumping);
                canDoubleJump = true;
            }

            else if (!isGrounded && canDoubleJump && isJumping)
            {
                Debug.Log("Pulo Duplo ON");
                playerRB.linearVelocityY = jumpForce;
                animator.SetBool(isDoubleJumpingHash, canDoubleJump);
            }
        }
    }

    public void OnDoubleJumpAnimationEnd()
    {
        animator.SetBool(isDoubleJumpingHash, false);
        canDoubleJump = false;
    }

    private bool OutOfCam() { return GameManagement.OutOfCam(gameObject); }

    public override void TakeDamage(int amount)
    {
        if (isTakingDamage) return;


        if (base.IsAlive())
        {
            canMove = false;
            isTakingDamage = true;
            isJumping = false;
            isRunning = false;
            UpdateAnimations();

            // Aplica o "empurrão"
            float direction = playerSR.flipX ? 1f : -1f;
            playerRB.linearVelocityX = direction * (playerSpeed * 0.5f);
            playerRB.linearVelocityY = jumpForce * 0.5f;

            base.TakeDamage(amount);
        
            StartCoroutine(TakingDamageRoutine());
        }
    }

    private IEnumerator TakingDamageRoutine()
    {
        // Define um período de invencibilidade de 1 segundos
        yield return new WaitForSeconds(1.5f);

        playerRB.linearVelocityX = 0;
    }

    public void OnDamageAnimationEnd()
    {
        Debug.Log("Passou por aqui");
        isTakingDamage = false;
        animator.SetBool(isTakingDamageHash, isTakingDamage);

        OnHealthChanged?.Invoke(currentHealth, IsAlive());
        Die();

        if (OutOfCam()) TeleportToSpawn();
    }

    void TeleportToSpawn()
    {
        Debug.Log("Teleporantando para o spawn");
        
        playerRB.linearVelocityX = 0;

        canMove = false;
        animator.SetBool(isTeleportingHash, true);

        Debug.Log(sceneManager.GetComponent<SceneManagerModel>().GetPlayerSpawnPoint());
        transform.position = spawnpoint;
    }

    private void OnTeleportAnimationEnd()
    {
        animator.SetBool(isTeleportingHash, false);
        canMove = true;
    }


    private void IsGrounded()
    {
        if (playerFootHitBox.GetComponent<PlayerHitBox>().colidindoComChao
                            || playerFootHitBox.GetComponent<PlayerHitBox>().colidindoComInimigo)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    public bool PlayerIsGrounded()
    {
        return isGrounded;
    }

    private void IsJumping()
    {
        // Debug.Log("Jogador está pulando ? " + isJumping);
        if (!isGrounded && Mathf.Abs(playerRB.linearVelocityY) > 0.001f)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }
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

    public bool PlayerIsTakingDamage()
    {
        return isTakingDamage;
    }

    private void UpdatePlayerStats()
    {
        IsGrounded();
        IsRunning();
        IsJumping();
        if (!isTakingDamage) canMove = true;
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
        base.CharacterStats();
        Debug.Log("Player isGrounded: " + isGrounded);
        Debug.Log("Player isJumping: " + isJumping);
        Debug.Log("Player Velocity X: " + playerRB.linearVelocityX);
        Debug.Log("Player Velocity Y: " + playerRB.linearVelocityY);
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
    
    private IEnumerator DisableCollisionTemporarily(TilemapCollider2D groundCollider)
    {
        // playerCC.enabled = false;
        Physics2D.IgnoreCollision(playerCC, groundCollider, true);

        yield return new WaitForSeconds(disableTime);

        // playerCC.enabled = true;
        Physics2D.IgnoreCollision(playerCC, groundCollider, false);
    }
}
