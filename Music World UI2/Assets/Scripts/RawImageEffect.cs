using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RawImageEffect : MonoBehaviour, IPointerClickHandler
{
    public float jumpStrength = 100f; // 跳动的强度
    public float duration = 0.5f; // 跳动的持续时间
    public int jumpCount = 1; // 跳动的次数

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 触发跳动效果
        rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
    }
}