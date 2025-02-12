using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CanvasPopupEffect : MonoBehaviour
{
    public CanvasGroup drawCanvasGroup; // �������� CanvasGroup ���
    public CanvasGroup cameraCanvasGroup; // �������� CanvasGroup ���
    public float duration = 0.5f; // ��������ʱ��
    public Ease easeType = Ease.OutBack; // ������������

    private void Start()
    {
        // ��ʼʱ���������
        drawCanvasGroup.alpha = 0;
        drawCanvasGroup.transform.localScale = Vector3.zero;
        drawCanvasGroup.interactable = false;
        drawCanvasGroup.blocksRaycasts = false;

        cameraCanvasGroup.alpha = 0;
        cameraCanvasGroup.transform.localScale = Vector3.zero;
        cameraCanvasGroup.interactable = false;
        cameraCanvasGroup.blocksRaycasts = false;
    }

    public void ShowDrawCanvas()
    {
        // ���ý�����
        drawCanvasGroup.interactable = true;
        drawCanvasGroup.blocksRaycasts = true;

        // ���ŵ�������
        drawCanvasGroup.DOFade(1, duration).SetEase(easeType);
        drawCanvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    public void HideDrawCanvas()
    {
        // �������ض���
        Sequence sequence = DOTween.Sequence();
        sequence.Append(drawCanvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(drawCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // ������ɺ���ý�����
            drawCanvasGroup.interactable = false;
            drawCanvasGroup.blocksRaycasts = false;
        });
    }
    public void ShowCameraCanvas()
    {
        // ���ý�����
        cameraCanvasGroup.interactable = true;
        cameraCanvasGroup.blocksRaycasts = true;
        // ���ŵ�������
        cameraCanvasGroup.DOFade(1, duration).SetEase(easeType);
        cameraCanvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    public void HideCameraCanvas()
    {
        // �������ض���
        Sequence sequence = DOTween.Sequence();
        sequence.Append(cameraCanvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(cameraCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // ������ɺ���ý�����
            cameraCanvasGroup.interactable = false;
            cameraCanvasGroup.blocksRaycasts = false;
        });
    }
}