using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public GameObject drawCanvas;
    public GameObject mainCanvas;
    public GameObject startCanvas;
    public GameObject gameCanvas;
    public GameObject cameraCanvas;
    // Start is called before the first frame update
    void Start()
    {
        cameraCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        drawCanvas.SetActive(false);
        mainCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }
    public void EnterGameCanvas()
    {
        mainCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }
    public void ShowMainCanvas()
    {
        gameCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        startCanvas.SetActive(false) ;
    }
    public void ShowDrawCanvas()
    {
        drawCanvas.SetActive(true);
    }
    public void ShowCameraCanvas()
    {
        cameraCanvas.SetActive(true) ;
    }
    public void HideDrawCanvas() 
    {
        drawCanvas.SetActive(false);
    }
    public void HideCameraCanvas()
    {
        cameraCanvas.SetActive(false) ;
    }
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
