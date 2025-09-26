using UnityEngine;


public abstract class CollectibleBase : MonoBehaviour
{
    [SerializeField] protected int scoreValue = 1;
    [SerializeField] protected string collectibleName = "Collectible";

    public void OnCollect(GameObject obj)
    {
        var entity = obj.GetComponent<PlayerManager>();
        if (entity != null)
        {
            entity.AddScore((collectibleName, scoreValue));
            Debug.Log($"{collectibleName} collected! Score +{scoreValue}");
        }
    }
}