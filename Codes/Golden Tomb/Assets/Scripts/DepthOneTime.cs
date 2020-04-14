using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthOneTime : MonoBehaviour
{
    private SpriteRenderer cachedSpriteRenderer;
 
    void Start () {
        cachedSpriteRenderer = GetComponent<SpriteRenderer> ();
        cachedSpriteRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100);
    }
}
