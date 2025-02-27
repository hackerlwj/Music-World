using UnityEngine;

public class MultiPrefabMoveAndDestroy : MonoBehaviour
{
    public float speed = 5f;
    public float destroyYScreen;
    public bool isCollidingWithLine = false;

    public GameObject SuccessEffectPrefab;

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
            currentScreenPosition.y -= speed * Time.deltaTime;
            rectTransform.anchoredPosition = currentScreenPosition;

            if (currentScreenPosition.y < destroyYScreen)
            {
                Destroy(gameObject);
            }
            if (currentScreenPosition.y < -216)
            {
                isCollidingWithLine = true;
            }
        }
    }
    public void SuccessEffect()
    {
        if (SuccessEffectPrefab != null)
        {
            // 获取当前UI元素所在的Canvas
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                // 在当前UI元素的位置实例化粒子特效
                GameObject particleEffect = Instantiate(SuccessEffectPrefab, transform.position, Quaternion.identity);
                // 将粒子特效设置为Canvas的子对象
                particleEffect.transform.SetParent(parentCanvas.transform, true);
            }
        }
    }
}