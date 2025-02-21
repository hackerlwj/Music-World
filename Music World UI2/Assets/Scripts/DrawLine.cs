using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Newtonsoft.Json; // 需要安装 Newtonsoft.Json 包

public class DrawLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private RawImage rawImage;
    private CanvasGroup canvasGroup;
    private Vector3 offset;
    private AudioSource audioSource;
    public string apiUrl = "http://cn-hk-bgp-5.ofalias.net:37688/graffiti/get_voice";

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        audioSource = GetComponent<AudioSource>();
        rawImage = GetComponent<RawImage>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 设置半透明表示正在拖拽
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // 计算鼠标点击位置与RawImage之间的偏移量
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            offset = rectTransform.position - globalMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 更新RawImage的位置
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢复透明度和射线检测
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(CallApi());
    }

    IEnumerator CallApi()
    {
        // 1. 获取 RectTransform 的 Y 坐标
        float yCoordinate = rectTransform.anchoredPosition.y;
        int yParameter = 1;

        if (yCoordinate < -23)
        {
            yParameter = 1;
        }
        else if (yCoordinate >= -23 && yCoordinate < -17)
        {
            yParameter = 2;
        }
        else if (yCoordinate >= -17 && yCoordinate < -10)
        {
            yParameter = 3;
        }
        else if (yCoordinate >= -10 && yCoordinate < -6)
        {
            yParameter = 4;
        }
        else if (yCoordinate >= -6 && yCoordinate < 0)
        {
            yParameter = 5;
        }
        else if (yCoordinate >= 0 && yCoordinate < 5)
        {
            yParameter = 6;
        }
        else if (yCoordinate >= 5 && yCoordinate < 11)
        {
            yParameter = 7;
        }
        else if (yCoordinate >= 11 && yCoordinate < 17)
        {
            yParameter = 8;
        }
        else if (yCoordinate >= 17 && yCoordinate < 23)
        {
            yParameter = 9;
        }
        else if (yCoordinate >= 23 && yCoordinate < 29)
        {
            yParameter = 10;
        }
        else if (yCoordinate >= 29 && yCoordinate < 35)
        {
            yParameter = 11;
        }
        else if (yCoordinate >= 35 && yCoordinate < 41)
        {
            yParameter = 12;
        }

        // 2. 获取 RawImage 的颜色
        Color color = rawImage.color;
        string colorParameter="";
        if (color == new Color(225f / 255f, 118f / 255f, 0))
        {
            colorParameter = "orange";
        }
        else if(color == new Color(100f / 255f, 149f / 255f, 237f / 255f))
        {
            colorParameter = "blue";
        }
        else if (color == new Color(130f / 255f, 34f / 255f, 243f / 255f))
        {
            colorParameter = "purple";
        }

        // 3. 将 RawImage 的纹理转换为字节数据
        Texture2D texture = rawImage.texture as Texture2D;
        if (texture == null)
        {
            Debug.LogError("RawImage texture is not a Texture2D!");
            yield break;
        }

        byte[] imageData = texture.EncodeToPNG();

        // 4. 构造包含 color 和 y 的数据对象
        var data = new
        {
            color = colorParameter,
            y = yParameter
        };

        string jsonData = JsonConvert.SerializeObject(data);

        // 5. 使用 WWWForm 构造请求
        WWWForm form = new WWWForm();
        form.AddField("data", jsonData);
        form.AddBinaryData("image", imageData, "image.png", "image/png");

        // 6. 发送请求，允许不安全的连接
        using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl, form))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            DownloadHandlerAudioClip downloadHandler = new DownloadHandlerAudioClip(webRequest.url, AudioType.WAV);
            downloadHandler.streamAudio = true;
            webRequest.downloadHandler = downloadHandler;

            // 发送请求
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                AudioClip audioClip = downloadHandler.audioClip;
                if (audioClip != null)
                {
                    if (audioSource != null)
                    {
                        audioSource.clip = audioClip;
                        audioSource.Play();
                        Debug.Log("Audio is playing...");
                    }
                    else
                    {
                        Debug.LogError("AudioSource component is not assigned!");
                    }
                }
                else
                {
                    Debug.LogError("Failed to get AudioClip from response!");
                }
            }
        }
    }

    private class AcceptAllCertificates : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}