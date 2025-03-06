using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InputPanelScript : MonoBehaviour
{
    public InputField inputField;// 引用输入字段
    public Dropdown dropdown;// 引用下拉列表
    public CanvasGroup melodyGenerationPanelCanvasGroup;
    public float duration = 0.5f; // 动画持续时间
    public Ease easeType = Ease.OutBack; // 动画缓动类型

    void Start()
    {
        if (melodyGenerationPanelCanvasGroup != null)
        {
            melodyGenerationPanelCanvasGroup.alpha = 0;
            melodyGenerationPanelCanvasGroup.transform.localScale = Vector3.zero;
            melodyGenerationPanelCanvasGroup.interactable = false;
            melodyGenerationPanelCanvasGroup.blocksRaycasts = false;
        }
        // 为下拉列表添加选项
        dropdown.options.Clear();
        dropdown.options.Add(new Dropdown.OptionData("选项 1"));
        dropdown.options.Add(new Dropdown.OptionData("选项 2"));
        dropdown.options.Add(new Dropdown.OptionData("选项 3"));
        // 为下拉列表添加选择事件监听器
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnSubmitButtonClicked()
    {
        // 获取输入字段中的文本
        string inputText = inputField.text;

        // 在这里可以处理输入的文本，例如打印到控制台
        Debug.Log("输入的文本是: " + inputText);
    }

    public void OnDropdownValueChanged(int index)
    {
        // 获取下拉列表中选中的选项文本
        string selectedOption = dropdown.options[index].text;
        // 将选中的选项文本填入输入字段
        inputField.text = selectedOption;
    }
    public void ShowMelodyGenerationPanel()
    {
        if (melodyGenerationPanelCanvasGroup != null)
        {
            // 启用交互性
            melodyGenerationPanelCanvasGroup.interactable = true;
            melodyGenerationPanelCanvasGroup.blocksRaycasts = true;
            // 播放弹出动画
            melodyGenerationPanelCanvasGroup.DOFade(1, duration).SetEase(easeType);
            melodyGenerationPanelCanvasGroup.transform.DOScale(new Vector3(0.5f,0.7f,0.5f), duration).SetEase(easeType);
        }
    }
    public void HideMelodyGenerationPanel()
    {
        if (melodyGenerationPanelCanvasGroup != null)
        {
            // 播放隐藏动画
            melodyGenerationPanelCanvasGroup.DOFade(0, duration).SetEase(easeType);
            melodyGenerationPanelCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType)
               .OnComplete(() =>
               {
                   // 动画完成后禁用交互性和射线投射
                   melodyGenerationPanelCanvasGroup.interactable = false;
                   melodyGenerationPanelCanvasGroup.blocksRaycasts = false;
               });
        }
    }
}