using Unity.VisualScripting;
using UnityEngine;

public class SawTrap : TrapBase
{
    [SerializeField] private GameObject pontoAObject;
    [SerializeField] private GameObject pontoBObject;
    private Vector3 pontoDestino; // Variável privada para rastrear o ponto de destino atual
    private Vector3 pontoA;
    private Vector3 pontoB;

    [SerializeField] private float sawSpeed = 3f;
    [SerializeField] private static bool trapOn = true;
    private Animator animator;
    private int turnedOnHash = Animator.StringToHash("turnedOn");

    void Awake()
    {
        trapName = "Saw";
        damageValue = 2;
        animator = GetComponent<Animator>();

        pontoA = pontoAObject.transform.position;
        pontoB = pontoBObject.transform.position;
        // Destroy(pontoAObject);
        // Destroy(pontoBObject);
    }

    void Start()
    {
        UpdateAnimation();
        pontoDestino = pontoB;
    }

    void Update() { if (trapOn) Move(); }

    void LateUpdate()
    {
        pontoAObject.transform.position = pontoA;
        pontoBObject.transform.position = pontoB;
    }

    void Move()
    {
        // 1. Move o objeto em direção ao 'pontoDestino'
        transform.position = Vector3.MoveTowards(
            transform.position, 
            pontoDestino, 
            sawSpeed * Time.deltaTime // 'Time.deltaTime' garante que o movimento seja suave e independente do FPS
        );

        // 2. Verifica se o objeto chegou ao 'pontoDestino'
        if (transform.position == pontoDestino)
        {
            // Se chegou ao ponto B, o novo destino é o ponto A.
            if (pontoDestino == pontoB) pontoDestino = pontoA;
            // Se chegou ao ponto A, o novo destino é o ponto B.
            else if (pontoDestino == pontoA) pontoDestino = pontoB;
        }
    }

    public void TurnOnTrap()
    {
        trapOn = true;
        UpdateAnimation();
    }
    public void TurnOffTrap()
    {
        trapOn = false;
        UpdateAnimation();
    }
    
    void UpdateAnimation()
    {
        animator.SetBool(turnedOnHash, trapOn);
    }
}