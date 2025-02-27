using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;

    public GameObject promptImage;

    // ���ڼ�¼�������
    private int clickCount = 0;

    void Start()
    {
        // ��ʼʱ��������2������3����ʾ����1
        Text2.SetActive(false);
        Text3.SetActive(false);
        Text1.SetActive(true);
    }

    void Update()
    {
    
    }

    public void TextClick()
    {
        clickCount++;
        switch (clickCount)
        {
            case 1:
                ShowObject(Text2);
                break;
            case 2:
                ShowObject(Text3);
                break;
            case 3:
                clickCount = 0;
                promptImage.SetActive(false);
                Text1.SetActive(true);
                break;
            default:
                break;
        }
    }
    private void ShowObject(GameObject obj)
    {
        // ������������
        HideAllObjects();
        // ��ʾָ��������
        obj.SetActive(true);
    }

    private void HideAllObjects()
    {
        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
    }
}
