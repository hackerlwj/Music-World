using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Animator animator;

    public GameObject saveCanvas;
    public GameObject drawCanvas;
    public GameObject mainCanvas;
    public GameObject startCanvas;
    public GameObject gameCanvas;
    public GameObject cameraCanvas;
    // Start is called before the first frame update
    void Start()
    {
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

        yield return new WaitForSeconds(1);

        gameCanvas.SetActive(false);
        mainCanvas.SetActive(true);
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
            // 获取目录中的所有文件
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
