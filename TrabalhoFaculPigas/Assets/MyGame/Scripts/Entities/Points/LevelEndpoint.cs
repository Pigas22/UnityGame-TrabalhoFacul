using UnityEngine;
using UnityEngine.UIElements;

public class LevelEndpoint : MonoBehaviour
{
    [SerializeField] private GameObject[] efeitos = new GameObject[4];
    [SerializeField] private bool isPressed;
    private Animator animator;
    private static readonly int isPressedTrigger = Animator.StringToHash("IsPressedTrigger");

    [SerializeField] AudioSource musicaVitoria;

    void Start()
    {
        animator = GetComponent<Animator>();
        foreach (var e in efeitos) { e.SetActive(false); }

        musicaVitoria = gameObject.AddComponent<AudioSource>();
        musicaVitoria.clip = Resources.Load<AudioClip>("Sounds/Victory Sound");
        musicaVitoria.volume = 1;
        musicaVitoria.time = 1f;
        musicaVitoria.loop = false;
    }

    public void OnTriggerEnter2D()
    {
        if (musicaVitoria.isPlaying || isPressed) return;
        musicaVitoria.Play();
    }

    public void OnTriggerStay2D()
    {
        isPressed = true;
        animator.SetTrigger(isPressedTrigger);
        foreach (var e in efeitos)
        {
            e.SetActive(true);
        }

        GetComponentInParent<SceneManagerModel>().FinishGame();
    }
}