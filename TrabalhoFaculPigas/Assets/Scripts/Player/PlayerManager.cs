using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int playerLives = 6;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private bool playerIsAlive = true;
    [SerializeField] private GameObject playerHitBox;

    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private SpriteRenderer playerSR;
    [SerializeField] private CapsuleCollider2D playerCC;
    private Camera mainCamera;
    private GameObject sceneManager;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
        playerRB = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        playerCC = GetComponent<CapsuleCollider2D>();

        mainCamera = Camera.main;
        sceneManager = GameObject.Find("SceneManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsAlive)
        {
            walk(Input.GetAxis("Horizontal"));
            jump();
        }
        else
        {
            Debug.Log("Player is dead. Game Over.");
            die();

        }
    }

    void LateUpdate()
    {
        if (playerIsAlive)
        {
            outOfCam();
        }
    }

    public void walk(float walkInput)
    {
        if (walkInput != 0)
        {
            playerRB.linearVelocityX = walkInput * playerSpeed;
            Debug.Log("Player velocity X: " + playerRB.linearVelocityX);

            if (walkInput > 0)
            {
                playerSR.flipX = false;
            }
            else if (walkInput < 0)
            {
                playerSR.flipX = true;
            }
        }
    }

    public void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(playerRB.linearVelocityY) < 0.001f)
        {
            playerRB.linearVelocityY = jumpForce;
        }
    }

    public void takeDamage(int damage)
    {
        playerLives -= damage;
        Debug.Log("Player took " + damage + " damage. Lives left: " + playerLives);

        if (playerLives <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Debug.Log("Game Over. Restarting the game...");
        playerLives = 0;
        playerIsAlive = false;
        this.GameObject().SetActive(false);
    }

    private bool canTeleport(Vector3 targetPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(targetPosition, playerCC.size, 0f);
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider found at target position: " + collider.name);
            if (collider != playerCC && !collider.isTrigger)
            {
                return false;
            }
        }
        return true;
    }


    private void outOfCam()
    {
        Vector3 viewportPoint = GameManagement.positionToViewPortPoint(this.GameObject());
        bool outOfXAxe = viewportPoint.x < 0 || viewportPoint.x > 1;
        bool outOfYAxe = viewportPoint.y < 0 || viewportPoint.y > 1;

        if (outOfYAxe)
        {
            teleportToSpawn();
            takeDamage(1);
        }

        if (outOfXAxe)
        {
            Vector3 newViewportPoint = viewportPoint;
            newViewportPoint.x = (newViewportPoint.x < 0) ? 0.99f : 0.01f;
            Vector3 targetPosition = GameManagement.viewPortPointToPosition(newViewportPoint);
            if (canTeleport(targetPosition))
            {
                transform.position = targetPosition;
            }
            else
            {
                teleportToSpawn();
                takeDamage(1);
            }
        }
    }

    void teleportToSpawn()
    {
        transform.position = sceneManager.GetComponent<SceneTestManager>().GetPlayerSpawnPoint();
    }
}
