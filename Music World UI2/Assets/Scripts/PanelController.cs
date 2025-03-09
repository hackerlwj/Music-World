using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class PanelController : MonoBehaviour
{
    //public Image panel2PromptImage;
    public Image panel4PromptImage;
    //public Image panel5PromptImage;
    public float hideTime = 1f;

    public GameObject drawPenList;
    private bool isDrawPenListVisible = false;
    public CanvasGroup panel1CanvasGroup; // ��һ������ CanvasGroup ���
    public CanvasGroup panel2CanvasGroup; // �ڶ������� CanvasGroup ���
    public CanvasGroup panel3CanvasGroup; // ���������� CanvasGroup ���
    public CanvasGroup panel4CanvasGroup; // ���ĸ����� CanvasGroup ���
    public CanvasGroup panel5CanvasGroup; // ��������� CanvasGroup ���
    public float fadeDuration = 0.5f; // ���뵭�������ĳ���ʱ��
    public Ease fadeEaseType = Ease.Linear; // ������������

    public void ShowDrawPenList()
    {
        if(isDrawPenListVisible == false)
        {
            drawPenList.SetActive(true);
            isDrawPenListVisible = true;
        }
        else
        {
            drawPenList.SetActive(false);
            isDrawPenListVisible = false;
        }    
            
    }
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
            //StartCoroutine(FadeOutOverTime(hideTime, panel2PromptImage));
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

    // ��ʾ���ĸ����ķ���
    public void ShowPanel4()
    {
        if (panel4CanvasGroup != null)
        {
            StartCoroutine(FadeOutOverTime(hideTime, panel4PromptImage));
            panel4CanvasGroup.blocksRaycasts = true; // �������߼�⣬ʹ���ɽ���
            panel4CanvasGroup.interactable = true; // ���ý�����
            panel4CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // ���붯��
        }
    }

    // ���ص��ĸ����ķ���
    public void HidePanel4()
    {
        if (panel4CanvasGroup != null)
        {
            panel4CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel4CanvasGroup.blocksRaycasts = false; // �������߼�⣬ʹ��岻�ɽ���
                panel4CanvasGroup.interactable = false; // ���ý�����
            });
        }
    }

    // ��ʾ��������ķ���
    public void ShowPanel5()
    {
        if (panel5CanvasGroup != null)
        {
            //StartCoroutine(FadeOutOverTime(hideTime, panel5PromptImage));
            panel5CanvasGroup.blocksRaycasts = true; // �������߼�⣬ʹ���ɽ���
            panel5CanvasGroup.interactable = true; // ���ý�����
            panel5CanvasGroup.DOFade(1f, fadeDuration).SetEase(fadeEaseType); // ���붯��
        }
    }

    // ���ص�������ķ���
    public void HidePanel5()
    {
        if (panel5CanvasGroup != null)
        {
            panel5CanvasGroup.DOFade(0f, fadeDuration).SetEase(fadeEaseType).OnComplete(() =>
            {
                panel5CanvasGroup.blocksRaycasts = false; // �������߼�⣬ʹ��岻�ɽ���
                panel5CanvasGroup.interactable = false; // ���ý�����
            });
        }
    }

    IEnumerator FadeOutOverTime(float fadeDuration, Image promptImage)
    {
        // ��ʾ��ʾά�ֵ�ʱ��
        yield return new WaitForSeconds(4f);

        // ��¼��ʼʱ��
        float startTime = Time.time;
        // ��¼��ʼʱ��͸����
        float startAlpha = promptImage.color.a;

        // ��ȡ����������� Image ���
        Image[] childImages = promptImage.GetComponentsInChildren<Image>();
        float[] startAlphas = new float[childImages.Length];

        // ��¼ÿ�������� Image �ĳ�ʼ͸����
        for (int i = 0; i < childImages.Length; i++)
        {
            startAlphas[i] = childImages[i].color.a;
        }

        while (Time.time - startTime < fadeDuration)
        {
            // ���㵱ǰ��͸����
            float t = (Time.time - startTime) / fadeDuration;
            float currentAlpha = Mathf.Lerp(startAlpha, 0f, t);

            // ��ȡ��ǰ����ɫ
            Color currentColor = promptImage.color;
            // �����µ�͸����
            currentColor.a = currentAlpha;
            // �����µ���ɫ
            promptImage.color = currentColor;

            // ����������� Image ͸����
            for (int i = 0; i < childImages.Length; i++)
            {
                float childCurrentAlpha = Mathf.Lerp(startAlphas[i], 0f, t);
                Color childColor = childImages[i].color;
                childColor.a = childCurrentAlpha;
                childImages[i].color = childColor;
            }

            // �ȴ���һ֡
            yield return null;
        }

        // ȷ��������͸��������Ϊ 0
        promptImage.color = new Color(promptImage.color.r, promptImage.color.g, promptImage.color.b, 0f);
        promptImage.gameObject.SetActive(false);

        // ȷ��������͸��������Ϊ 0 ������������
        for (int i = 0; i < childImages.Length; i++)
        {
            Color childColor = childImages[i].color;
            childColor.a = 0f;
            childImages[i].color = childColor;
            childImages[i].gameObject.SetActive(false);
        }
    }
}