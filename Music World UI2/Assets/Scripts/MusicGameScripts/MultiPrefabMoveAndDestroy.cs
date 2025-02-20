using UnityEngine;

public class MultiPrefabMoveAndDestroy : MonoBehaviour
{
    public float speed = 5f;
    public float destroyXScreen;
    public bool isCollidingWithLine = false;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rectTransform != null)
        {
            Vector2 currentScreenPosition = rectTransform.anchoredPosition;
            currentScreenPosition.x -= speed * Time.deltaTime;
            rectTransform.anchoredPosition = currentScreenPosition;

            if (currentScreenPosition.x < destroyXScreen)
            {
                Destroy(gameObject);
            }
            if (currentScreenPosition.x < -228)
            {
                isCollidingWithLine = true;
            }
        }
    }
}