using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TheSimpleDraw;

public class CanvasPopupEffect : MonoBehaviour
{
    public SimpleDraw simpleDraw;
    public CameraController cameraController;

    public CanvasGroup drawCanvasGroup;       // 弹出面板的 CanvasGroup 组件
    public CanvasGroup cameraCanvasGroup;     // 弹出面板的 CanvasGroup 组件
    public CanvasGroup saveLoadCanvas;        // 存档读取面板的 CanvasGroup 组件
    public float duration = 0.5f;             // 动画持续时间
    public Ease easeType = Ease.OutBack;      // 动画缓动类型

    private void Start()
    {
        // 初始化隐藏状态
        InitializeCanvas(drawCanvasGroup);
        InitializeCanvas(cameraCanvasGroup);
        InitializeCanvas(saveLoadCanvas);
    }

    // 初始化单个 CanvasGroup 的隐藏状态
    private void InitializeCanvas(CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.transform.localScale = Vector3.zero;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    // 显示指定的 CanvasGroup
    public void ShowCanvas(CanvasGroup canvasGroup)
    {
        // 启用交互性
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // 播放弹出动画
        canvasGroup.DOFade(1, duration).SetEase(easeType);
        canvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
    }

    // 隐藏指定的 CanvasGroup
    public void HideCanvas(CanvasGroup canvasGroup)
    {
        // 播放隐藏动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(canvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // 隐藏完成后禁用交互性
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }
    public void TakeActionAndHideCanvas(CanvasGroup canvasGroup)
    {
        // 播放隐藏动画
        Sequence sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(0, duration).SetEase(easeType));
        sequence.Join(canvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));
        sequence.OnComplete(() =>
        {
            // 隐藏完成后禁用交互性
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            if (canvasGroup == drawCanvasGroup)
            {
                simpleDraw.SaveTextureAsPNG();
            }
            else if (canvasGroup == cameraCanvasGroup)
            {
                cameraController.TakePhoto();
            }
        });
    }

    // 公开方法，用于显示和隐藏特定的 CanvasGroup
    public void ShowDrawCanvas() => ShowCanvas(drawCanvasGroup);
    public void HideDrawCanvas() => HideCanvas(drawCanvasGroup);
    public void ShowCameraCanvas() => ShowCanvas(cameraCanvasGroup);
    public void HideCameraCanvas() => HideCanvas(cameraCanvasGroup);
    public void TakePhotoAndHideCameraCanvas() => TakeActionAndHideCanvas(cameraCanvasGroup);
    public void SaveAndHideDrawCanvas() => TakeActionAndHideCanvas(drawCanvasGroup);
    public void ShowSaveLoadCanvas() => ShowCanvas(saveLoadCanvas);
    public void HideSaveLoadCanvas() => HideCanvas(saveLoadCanvas);
}