using Palmmedia.ReportGenerator.Core;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InputPanelScript : MonoBehaviour
{
    public InputField inputField;// ���������ֶ�
    public Dropdown dropdown;// ���������б�
    public CanvasGroup melodyGenerationPanelCanvasGroup;
    public float duration = 0.5f; // ��������ʱ��
    public Ease easeType = Ease.OutBack; // ������������

    void Start()
    {
        if (melodyGenerationPanelCanvasGroup != null)
        {
            melodyGenerationPanelCanvasGroup.alpha = 0;
            melodyGenerationPanelCanvasGroup.transform.localScale = Vector3.zero;
            melodyGenerationPanelCanvasGroup.interactable = false;
            melodyGenerationPanelCanvasGroup.blocksRaycasts = false;
        }
        // Ϊ�����б����ѡ��
        dropdown.options.Clear();
        dropdown.options.Add(new Dropdown.OptionData("ѡ�� 1"));
        dropdown.options.Add(new Dropdown.OptionData("ѡ�� 2"));
        dropdown.options.Add(new Dropdown.OptionData("ѡ�� 3"));
        // Ϊ�����б����ѡ���¼�������
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    public void OnSubmitButtonClicked()
    {
        // ��ȡ�����ֶ��е��ı�
        string inputText = inputField.text;

        // ��������Դ���������ı��������ӡ������̨
        Debug.Log("������ı���: " + inputText);
    }

    public void OnDropdownValueChanged(int index)
    {
        // ��ȡ�����б���ѡ�е�ѡ���ı�
        string selectedOption = dropdown.options[index].text;
        // ��ѡ�е�ѡ���ı����������ֶ�
        inputField.text = selectedOption;
    }
    public void ShowMelodyGenerationPanel()
    {
        if (melodyGenerationPanelCanvasGroup != null)
        {
            // ���ý�����
            melodyGenerationPanelCanvasGroup.interactable = true;
            melodyGenerationPanelCanvasGroup.blocksRaycasts = true;
            // ���ŵ�������
            melodyGenerationPanelCanvasGroup.DOFade(1, duration).SetEase(easeType);
            melodyGenerationPanelCanvasGroup.transform.DOScale(new Vector3(0.5f,0.7f,0.5f), duration).SetEase(easeType);
        }
    }
    public void HideMelodyGenerationPanel()
    {
        if (melodyGenerationPanelCanvasGroup != null)
        {
            // �������ض���
            melodyGenerationPanelCanvasGroup.DOFade(0, duration).SetEase(easeType);
            melodyGenerationPanelCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType)
               .OnComplete(() =>
               {
                   // ������ɺ���ý����Ժ�����Ͷ��
                   melodyGenerationPanelCanvasGroup.interactable = false;
                   melodyGenerationPanelCanvasGroup.blocksRaycasts = false;
               });
        }
    }
}