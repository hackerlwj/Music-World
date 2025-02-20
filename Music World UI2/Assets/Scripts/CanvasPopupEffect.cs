using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CanvasPopupEffect : MonoBehaviour
{
    public CanvasGroup drawCanvasGroup;       // �������� CanvasGroup ���
    public CanvasGroup cameraCanvasGroup;     // �������� CanvasGroup ���
    public CanvasGroup saveLoadCanvas;        // �浵��ȡ���� CanvasGroup ���
    public float duration = 0.5f;             // ��������ʱ��
    public Ease easeType = Ease.OutBack;      // ������������

    private void Start()
    {
        // ��ʼ������״̬
        InitializeCanvas(drawCanvasGroup);
        InitializeCanvas(cameraCanvasGroup);
        InitializeCanvas(saveLoadCanvas);
    }

    // ��ʼ������ CanvasGroup ������״̬
    private void InitializeCanvas(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.transform.localScale = Vector3.zero;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // ��ʾָ���� CanvasGroup
    public void ShowCanvas(CanvasGroup canvasGroup)
    {
        // ���ý�����
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // ���ŵ�������
        canvasGroup.DOFade(1, duration).SetEase(easeType);
        canvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    // ����ָ���� CanvasGroup
    public void HideCanvas(CanvasGroup canvasGroup)
    {
        // �������ض���
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(canvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // ������ɺ���ý�����
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    // ����������������ʾ�������ض��� CanvasGroup
    public void ShowDrawCanvas() => ShowCanvas(drawCanvasGroup);
    public void HideDrawCanvas() => HideCanvas(drawCanvasGroup);
    public void ShowCameraCanvas() => ShowCanvas(cameraCanvasGroup);
    public void HideCameraCanvas() => HideCanvas(cameraCanvasGroup);
    public void ShowSaveLoadCanvas() => ShowCanvas(saveLoadCanvas);
    public void HideSaveLoadCanvas() => HideCanvas(saveLoadCanvas);
}