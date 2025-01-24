using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    public GameObject drawCanvas;
    public GameObject mainCanvas;
    public GameObject startCanvas;
    // Start is called before the first frame update
    void Start()
    {
        //drawCanvas.SetActive(false);
        //mainCanvas.SetActive(false);
        //startCanvas.SetActive(true);
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
    // Update is called once per frame
    void Update()
    {
        
    }
}
