using UnityEngine;
using UnityEngine.UIElements;

public class LevelEndpoint : MonoBehaviour
{
    [SerializeField] private GameObject[] efeitos = new GameObject[4];
    [SerializeField] private bool isPressed;
    private Animator animator;
    private static readonly int isPressedTrigger = Animator.StringToHash("IsPressedTrigger");


    void Start()
    {
        animator = GetComponent<Animator>();
        foreach (var e in efeitos) { e.SetActive(false); }
    }

    public void OnTriggerStay2D()
    {
        animator.SetTrigger(isPressedTrigger);
        foreach (var e in efeitos)
        {
            e.SetActive(true);
        }
        GetComponentInParent<SceneManagerModel>().FinishGame();
    }
}