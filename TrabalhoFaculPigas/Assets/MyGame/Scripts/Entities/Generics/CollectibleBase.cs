using UnityEngine;


public abstract class CollectibleBase : MonoBehaviour
{
    [SerializeField] protected int scoreValue = 1;
    [SerializeField] protected string collectibleName = "Collectible";
    [SerializeField] protected Animator animator = null;
    private int collectingHash = Animator.StringToHash("collecting");


    public void OnCollect(GameObject obj)
    {
        var entity = obj.GetComponent<PlayerManager>();
        if (entity != null)
        {
            entity.AddScore((collectibleName, scoreValue));
            GameManagement.DebugLog($"{collectibleName} collected! Score +{scoreValue}");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            OnCollect(collision.gameObject);
            animator.SetBool(collectingHash, true);

            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    void OnCollectAnimationEnd() {
        animator.SetBool(collectingHash, false);
        Destroy(gameObject);   
    }
}