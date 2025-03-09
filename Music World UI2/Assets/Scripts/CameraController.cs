using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;

public class CameraController : MonoBehaviour
{
    public LineManager lineManager;
    public RawImage display; // 用于显示摄像头画面的RawImage组件
    private WebCamTexture webCamTexture; // 摄像头纹理
    private string savePath; // 拍照保存路径

    void Start()
    {
        savePath = Application.streamingAssetsPath + "/SavedDrawings/"; // 设置保存路径
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        OpenCamera(); // 打开摄像头
    }

    // 打开摄像头
    public void OpenCamera()
    {
        if (WebCamTexture.devices.Length > 0)
        {
            webCamTexture = new WebCamTexture();
            display.texture = webCamTexture;
            webCamTexture.Play();
        }
        else
        {
            Debug.LogError("No webcam found.");
        }
    }

    // 拍照并保存图片
    public void TakePhoto()
    {
        if (webCamTexture != null)
        {
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            byte[] bytes = photo.EncodeToPNG(); // 保存为PNG格式
            string fileName = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png"; // 文件名
            File.WriteAllBytes(savePath + fileName, bytes);
            Debug.Log("Photo saved to: " + savePath + fileName);
            lineManager.CreateLine(savePath + fileName,Color.white,new Vector3 (0,0,0));
        }
    }

    private void OnDestroy()
    {
        if (webCamTexture != null)
        {
            webCamTexture.Stop();
        }
    }
}