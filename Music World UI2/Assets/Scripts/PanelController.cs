using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public CanvasGroup panel1CanvasGroup; // ��һ������ CanvasGroup ���
    public CanvasGroup panel2CanvasGroup; // �ڶ������� CanvasGroup ���
    public CanvasGroup panel3CanvasGroup; // ���������� CanvasGroup ���
    public float fadeDuration = 0.5f; // ���뵭�������ĳ���ʱ��
    public Ease fadeEaseType = Ease.Linear; // ������������

    // ��ʾ��һ�����ķ���
    public void ShowPanel1()
    {
        if (panel1CanvasGroup != null)
        {
            panel1CanvasGroup.blocksRaycasts = true; // �������߼�⣬ʹ���ɽ���
            panel1CanvasGroup.interactable = true; // ���ý�����
            panel1CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // ���붯��
        }
    }

    // ���ص�һ�����ķ���
    public void HidePanel1()
    {
        if (panel1CanvasGroup != null)
        {
            panel1CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel1CanvasGroup.blocksRaycasts = false; // �������߼�⣬ʹ��岻�ɽ���
                panel1CanvasGroup.interactable = false; // ���ý�����
            });
        }
    }

    // ��ʾ�ڶ������ķ���
    public void ShowPanel2()
    {
        if (panel2CanvasGroup != null)
        {
            panel2CanvasGroup.blocksRaycasts = true; // �������߼�⣬ʹ���ɽ���
            panel2CanvasGroup.interactable = true; // ���ý�����
            panel2CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // ���붯��
        }
    }

    // ���صڶ������ķ���
    public void HidePanel2()
    {
        if (panel2CanvasGroup != null)
        {
            panel2CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel2CanvasGroup.blocksRaycasts = false; // �������߼�⣬ʹ��岻�ɽ���
                panel2CanvasGroup.interactable = false; // ���ý�����
            });
        }
    }

    // ��ʾ���������ķ���
    public void ShowPanel3()
    {
        if (panel3CanvasGroup != null)
        {
            panel3CanvasGroup.blocksRaycasts = true; // �������߼�⣬ʹ���ɽ���
            panel3CanvasGroup.interactable = true; // ���ý�����
            panel3CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // ���붯��
        }
    }

    // ���ص��������ķ���
    public void HidePanel3()
    {
        if (panel3CanvasGroup != null)
        {
            panel3CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel3CanvasGroup.blocksRaycasts = false; // �������߼�⣬ʹ��岻�ɽ���
                panel3CanvasGroup.interactable = false; // ���ý�����
            });
        }
    }
}