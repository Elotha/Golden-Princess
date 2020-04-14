using UnityEngine;
 
public class Depth : MonoBehaviour {
 
    private SpriteRenderer cachedSpriteRenderer;
 
    void Start () {
        cachedSpriteRenderer = GetComponent<SpriteRenderer> ();
    }
 
    void Update () {
        cachedSpriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100);
    }
}