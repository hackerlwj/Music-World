using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CanvasPopupEffect : MonoBehaviour
{
    public CanvasGroup drawCanvasGroup; // 弹出面板的 CanvasGroup 组件
    public CanvasGroup cameraCanvasGroup; // 弹出面板的 CanvasGroup 组件
    public float duration = 0.5f; // 动画持续时间
    public Ease easeType = Ease.OutBack; // 动画缓动类型

    private void Start()
    {
        // 初始时将面板隐藏
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
        // 启用交互性
        drawCanvasGroup.interactable = true;
        drawCanvasGroup.blocksRaycasts = true;

        // 播放弹出动画
        drawCanvasGroup.DOFade(1, duration).SetEase(easeType);
        drawCanvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    public void HideDrawCanvas()
    {
        // 播放隐藏动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(drawCanvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(drawCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // 隐藏完成后禁用交互性
            drawCanvasGroup.interactable = false;
            drawCanvasGroup.blocksRaycasts = false;
        });
    }
    public void ShowCameraCanvas()
    {
        // 启用交互性
        cameraCanvasGroup.interactable = true;
        cameraCanvasGroup.blocksRaycasts = true;
        // 播放弹出动画
        cameraCanvasGroup.DOFade(1, duration).SetEase(easeType);
        cameraCanvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    public void HideCameraCanvas()
    {
        // 播放隐藏动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(cameraCanvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(cameraCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // 隐藏完成后禁用交互性
            cameraCanvasGroup.interactable = false;
            cameraCanvasGroup.blocksRaycasts = false;
        });
    }
}