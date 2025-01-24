using UnityEngine;
using UnityEngine.EventSystems;

public class DrawLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    Vector3 offset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 设置半透明表示正在拖拽
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // 计算鼠标点击位置与RawImage之间的偏移量
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            offset = rectTransform.position - globalMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 更新RawImage的位置
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢复透明度和射线检测
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}