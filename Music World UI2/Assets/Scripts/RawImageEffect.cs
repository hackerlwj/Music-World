using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RawImageEffect : MonoBehaviour, IPointerClickHandler
{
    public float jumpStrength = 100f; // ������ǿ��
    public float duration = 0.5f; // �����ĳ���ʱ��
    public int jumpCount = 1; // �����Ĵ���

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // ��������Ч��
        rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
    }
}