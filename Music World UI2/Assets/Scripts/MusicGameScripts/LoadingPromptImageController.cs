using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPromptImageController : MonoBehaviour
{
    public GameObject loadingPromptImage;
    public GameObject StartGameButton;
    public GameObject GenerateMelodyButton;
    // Start is called before the first frame update
    void Start()
    {
        loadingPromptImage.SetActive(false);
        StartGameButton.SetActive(false);
    }
    public void ShowLoadPromptImage()
    {
        StartCoroutine(DisappearAfterTime(5f));
    }
    IEnumerator DisappearAfterTime(float time)
    {
        loadingPromptImage.SetActive(true);
        // 等待指定的时间
        yield return new WaitForSeconds(time);
        // 隐藏物体
        loadingPromptImage.SetActive(false);
    }
    public void ButtonChange()
    {
        StartGameButton.SetActive(true);
        GenerateMelodyButton.SetActive(false);
    }
}
