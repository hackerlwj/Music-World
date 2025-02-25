using UnityEngine;

public class MultiPrefabMoveAndDestroy : MonoBehaviour
{
    public float speed = 5f;
    public float destroyXScreen;
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
            currentScreenPosition.x -= speed * Time.deltaTime;
            rectTransform.anchoredPosition = currentScreenPosition;

            if (currentScreenPosition.x < destroyXScreen)
            {
                Destroy(gameObject);
            }
            if (currentScreenPosition.x < 84)
            {
                isCollidingWithLine = true;
            }
        }
    }
    public void SuccessEffect()
    {
        if (SuccessEffectPrefab != null)
        {
            // ��ȡ��ǰUIԪ�����ڵ�Canvas
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                // �ڵ�ǰUIԪ�ص�λ��ʵ����������Ч
                GameObject particleEffect = Instantiate(SuccessEffectPrefab, transform.position, Quaternion.identity);
                // ��������Ч����ΪCanvas���Ӷ���
                particleEffect.transform.SetParent(parentCanvas.transform, true);
            }
        }
    }
}