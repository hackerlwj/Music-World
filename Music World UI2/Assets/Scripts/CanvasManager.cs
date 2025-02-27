using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public float hideTime = 1f;
    public Image gamePromptImage;
    public Animator animator;
    private Canvas mainCanva;

    public GameObject saveCanvas;
    public GameObject drawCanvas;
    public GameObject mainCanvas;
    public GameObject startCanvas;
    public GameObject gameCanvas;
    public GameObject cameraCanvas;
    // Start is called before the first frame update
    void Start()
    {
        mainCanva=mainCanvas.GetComponent<Canvas>();
        saveCanvas.SetActive(true);
        cameraCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        drawCanvas.SetActive(true);
        mainCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }
    public void EnterGameCanvas()
    {
        StartCoroutine(LoadGameCanvas());
    }
    IEnumerator LoadGameCanvas()
    {
        animator.SetBool("FadeIn", true);
        animator.SetBool("FadeOut", false);

        yield return new WaitForSeconds(1);

        mainCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        //StartCoroutine(FadeOutOverTime(hideTime,gamePromptImage));
        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);
    }
    public void ShowMainCanvas()
    {
        StartCoroutine(LoadMainCanvas());
    }
    IEnumerator LoadMainCanvas()
    {
        animator.SetBool("FadeIn", true);
        animator.SetBool("FadeOut", false);
        mainCanvas.SetActive(true) ;
        mainCanva.enabled = false;

        yield return new WaitForSeconds(1);

        gameCanvas.SetActive(false);
        mainCanva.enabled=true;
        startCanvas.SetActive(false);
        animator.SetBool("FadeIn", false);
        animator.SetBool("FadeOut", true);
    }
    //public void ShowDrawCanvas()
    //{
    //    drawCanvas.SetActive(true);
    //}
    //public void ShowCameraCanvas()
    //{
    //    cameraCanvas.SetActive(true);
    //}
    //public void HideDrawCanvas() 
    //{
    //    drawCanvas.SetActive(false);
    //}
    //public void HideCameraCanvas()
    //{
    //    cameraCanvas.SetActive(false) ;
    //}
    public void ExitApplication()
    {
        DeleteSavedTextures();
        Debug.Log("Application has been quit.");
    }
    public void DeleteSavedTextures()
    {
        string directoryPath = Application.streamingAssetsPath + "/SavedDrawings";
        if (Directory.Exists(directoryPath))
        {
            // ��ȡĿ¼�е������ļ�
            string[] files = Directory.GetFiles(directoryPath);
            foreach (string file in files)
            {
                File.Delete(file);
                Debug.Log($"Deleted file: {file}");
            }
        }
        else
        {
            Debug.Log("No saved textures to delete.");
        }
    }
    IEnumerator FadeOutOverTime(float fadeDuration, Image promptImage)
    {
        //��ʾ��ʾά�ֵ�ʱ��
        yield return new WaitForSeconds(3f);
        // ��¼��ʼʱ��
        float startTime = Time.time;
        // ��¼��ʼʱ��͸����
        float startAlpha = promptImage.color.a;

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

            // �ȴ���һ֡
            yield return null;
        }

        // ȷ��͸��������Ϊ0
        promptImage.color = new Color(promptImage.color.r, promptImage.color.g, promptImage.color.b, 0f);
        promptImage.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
