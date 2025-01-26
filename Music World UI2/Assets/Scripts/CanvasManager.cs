using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject drawCanvas;
    public GameObject mainCanvas;
    public GameObject startCanvas;
    // Start is called before the first frame update
    void Start()
    {
        drawCanvas.SetActive(false);
        mainCanvas.SetActive(false);
        startCanvas.SetActive(true);
    }
    public void ShowMainCanvas()
    {
        mainCanvas.SetActive(true);
    }
    public void ShowDrawCanvas()
    {
        drawCanvas.SetActive(true);
    }
    public void HideDrawCanvas() 
    {
        drawCanvas.SetActive(false);
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
