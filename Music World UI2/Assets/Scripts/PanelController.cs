using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public CanvasGroup panel1CanvasGroup; // 第一个面板的 CanvasGroup 组件
    public CanvasGroup panel2CanvasGroup; // 第二个面板的 CanvasGroup 组件
    public CanvasGroup panel3CanvasGroup; // 第三个面板的 CanvasGroup 组件
    public float fadeDuration = 0.5f; // 淡入淡出动画的持续时间
    public Ease fadeEaseType = Ease.Linear; // 动画缓动类型

    // 显示第一个面板的方法
    public void ShowPanel1()
    {
        if (panel1CanvasGroup != null)
        {
            panel1CanvasGroup.blocksRaycasts = true; // 启用射线检测，使面板可交互
            panel1CanvasGroup.interactable = true; // 启用交互性
            panel1CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // 淡入动画
        }
    }

    // 隐藏第一个面板的方法
    public void HidePanel1()
    {
        if (panel1CanvasGroup != null)
        {
            panel1CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel1CanvasGroup.blocksRaycasts = false; // 禁用射线检测，使面板不可交互
                panel1CanvasGroup.interactable = false; // 禁用交互性
            });
        }
    }

    // 显示第二个面板的方法
    public void ShowPanel2()
    {
        if (panel2CanvasGroup != null)
        {
            panel2CanvasGroup.blocksRaycasts = true; // 启用射线检测，使面板可交互
            panel2CanvasGroup.interactable = true; // 启用交互性
            panel2CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // 淡入动画
        }
    }

    // 隐藏第二个面板的方法
    public void HidePanel2()
    {
        if (panel2CanvasGroup != null)
        {
            panel2CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel2CanvasGroup.blocksRaycasts = false; // 禁用射线检测，使面板不可交互
                panel2CanvasGroup.interactable = false; // 禁用交互性
            });
        }
    }

    // 显示第三个面板的方法
    public void ShowPanel3()
    {
        if (panel3CanvasGroup != null)
        {
            panel3CanvasGroup.blocksRaycasts = true; // 启用射线检测，使面板可交互
            panel3CanvasGroup.interactable = true; // 启用交互性
            panel3CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // 淡入动画
        }
    }

    // 隐藏第三个面板的方法
    public void HidePanel3()
    {
        if (panel3CanvasGroup != null)
        {
            panel3CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel3CanvasGroup.blocksRaycasts = false; // 禁用射线检测，使面板不可交互
                panel3CanvasGroup.interactable = false; // 禁用交互性
            });
        }
    }
}