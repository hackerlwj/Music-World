using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextController : MonoBehaviour
{
    public GameObject Text1;
    public GameObject Text2;
    public GameObject Text3;

    public GameObject promptImage;

    // 用于记录点击次数
    private int clickCount = 0;

    void Start()
    {
        // 初始时隐藏物体2和物体3，显示物体1
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
        // 隐藏所有物体
        HideAllObjects();
        // 显示指定的物体
        obj.SetActive(true);
    }

    private void HideAllObjects()
    {
        Text1.SetActive(false);
        Text2.SetActive(false);
        Text3.SetActive(false);
    }
}
