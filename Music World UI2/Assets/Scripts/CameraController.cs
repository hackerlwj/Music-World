using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;

public class CameraController : MonoBehaviour
{
    public LineManager lineManager;
    public RawImage display; // ������ʾ����ͷ�����RawImage���
    private WebCamTexture webCamTexture; // ����ͷ����
    private string savePath; // ���ձ���·��

    void Start()
    {
        savePath = Application.streamingAssetsPath + "/SavedDrawings/"; // ���ñ���·��
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        OpenCamera(); // ������ͷ
    }

    // ������ͷ
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

    // ���ղ�����ͼƬ
    public void TakePhoto()
    {
        if (webCamTexture != null)
        {
            Texture2D photo = new Texture2D(webCamTexture.width, webCamTexture.height);
            photo.SetPixels(webCamTexture.GetPixels());
            photo.Apply();

            byte[] bytes = photo.EncodeToPNG(); // ����ΪPNG��ʽ
            string fileName = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png"; // �ļ���
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