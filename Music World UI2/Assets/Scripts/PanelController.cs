using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class PanelController : MonoBehaviour
{
    public Image panel2PromptImage;
    public Image panel4PromptImage;
    public Image panel5PromptImage;
    public float hideTime = 1f;

    public CanvasGroup panel1CanvasGroup; // 第一个面板的 CanvasGroup 组件
    public CanvasGroup panel2CanvasGroup; // 第二个面板的 CanvasGroup 组件
    public CanvasGroup panel3CanvasGroup; // 第三个面板的 CanvasGroup 组件
    public CanvasGroup panel4CanvasGroup; // 第四个面板的 CanvasGroup 组件
    public CanvasGroup panel5CanvasGroup; // 第五个面板的 CanvasGroup 组件
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
            StartCoroutine(FadeOutOverTime(hideTime, panel2PromptImage));
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

    // 显示第四个面板的方法
    public void ShowPanel4()
    {
        if (panel4CanvasGroup != null)
        {
            StartCoroutine(FadeOutOverTime(hideTime, panel4PromptImage));
            panel4CanvasGroup.blocksRaycasts = true; // 启用射线检测，使面板可交互
            panel4CanvasGroup.interactable = true; // 启用交互性
            panel4CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // 淡入动画
        }
    }

    // 隐藏第四个面板的方法
    public void HidePanel4()
    {
        if (panel4CanvasGroup != null)
        {
            panel4CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel4CanvasGroup.blocksRaycasts = false; // 禁用射线检测，使面板不可交互
                panel4CanvasGroup.interactable = false; // 禁用交互性
            });
        }
    }

    // 显示第五个面板的方法
    public void ShowPanel5()
    {
        if (panel5CanvasGroup != null)
        {
            StartCoroutine(FadeOutOverTime(hideTime, panel5PromptImage));
            panel5CanvasGroup.blocksRaycasts = true; // 启用射线检测，使面板可交互
            panel5CanvasGroup.interactable = true; // 启用交互性
            panel5CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // 淡入动画
        }
    }

    // 隐藏第五个面板的方法
    public void HidePanel5()
    {
        if (panel5CanvasGroup != null)
        {
            panel5CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel5CanvasGroup.blocksRaycasts = false; // 禁用射线检测，使面板不可交互
                panel5CanvasGroup.interactable = false; // 禁用交互性
            });
        }
    }

    IEnumerator FadeOutOverTime(float fadeDuration, Image promptImage)
    {
        // 提示显示维持的时间
        yield return new WaitForSeconds(4f);
        // 记录开始时间
        float startTime = Time.time;
        // 记录开始时的透明度
        float startAlpha = promptImage.color.a;

        while (Time.time - startTime < fadeDuration)
        {
            // 计算当前的透明度
            float t = (Time.time - startTime) / fadeDuration;
            float currentAlpha = Mathf.Lerp(startAlpha, 0f, t);

            // 获取当前的颜色
            Color currentColor = promptImage.color;
            // 设置新的透明度
            currentColor.a = currentAlpha;
            // 设置新的颜色
            promptImage.color = currentColor;

            // 等待下一帧
            yield return null;
        }

        // 确保透明度最终为0
        promptImage.color = new Color(promptImage.color.r, promptImage.color.g, promptImage.color.b, 0f);
        promptImage.gameObject.SetActive(false);
    }
}