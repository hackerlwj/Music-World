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
        // ���ð�͸����ʾ������ק
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // ���������λ����RawImage֮���ƫ����
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            offset = rectTransform.position - globalMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ����RawImage��λ��
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �ָ�͸���Ⱥ����߼��
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}