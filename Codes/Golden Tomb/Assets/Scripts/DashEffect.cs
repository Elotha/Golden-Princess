using UnityEngine;

public class DashEffect : MonoBehaviour
{
    private SpriteRenderer sprRenderer;
    private Color sprColor;
    [SerializeField] private float Scale = 1.1f;
    [SerializeField] private float ScaleSpeed;
    [SerializeField] private float Alpha = 0.5f;
    [SerializeField] private float AlphaSpeed;
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        sprColor = new Color(1,1,1,Alpha);
        sprRenderer.color = sprColor;
        transform.localScale = new Vector3(Scale,Scale,Scale);
    }

    // Update is called once per frame
    void Update()
    {
        sprRenderer.sortingOrder = -Mathf.RoundToInt(transform.position.y * 100) + 1;

        if (Alpha > 0f) {
            Alpha -= AlphaSpeed * Time.deltaTime;
            sprColor = new Color(1,1,1,Alpha);
            sprRenderer.color = sprColor;
            Scale += ScaleSpeed * Time.deltaTime;
            transform.localScale = new Vector3(Scale,Scale,Scale);
        }
        else Destroy(gameObject);
    }
}
