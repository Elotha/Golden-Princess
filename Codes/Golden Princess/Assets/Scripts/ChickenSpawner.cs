using UnityEngine;

public class ChickenSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Chicken;
    [SerializeField] private Transform ChickenParent;
    [SerializeField] private Transform ChickenRegion;
    [SerializeField] private int ChickenAmount = 0;
    private bool ChickensCreated = false;
    public void CreateChickens()
    {
        float rndX;
        float rndY;
        for (int i = 0; i < ChickenAmount; i++) {
            rndX = Random.Range(0f,ChickenRegion.localScale.x * 3f);
            rndY = Random.Range(0f,ChickenRegion.localScale.y * 3f);
            Instantiate(Chicken,ChickenRegion.transform.position + new Vector3(rndX,rndY,-1f),Quaternion.identity,ChickenParent);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ChickensCreated) {
            if (other.CompareTag("Umbrella")) {
                ChickensCreated = true;
                CreateChickens();
            }
        }
    }
}
