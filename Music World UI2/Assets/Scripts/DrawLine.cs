using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using Newtonsoft.Json; // ��Ҫ��װ Newtonsoft.Json ��

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
        // ���ð�͸����ʾ������ק
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;

        // ���������λ����RawImage֮���ƫ����
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            offset = rectTransform.position - globalMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ����RawImage��λ��
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos + offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �ָ�͸���Ⱥ����߼��
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(CallApi());
    }

    IEnumerator CallApi()
    {
        // 1. ��ȡ RectTransform �� Y ����
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

        // 2. ��ȡ RawImage ����ɫ
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

        // 3. �� RawImage ������ת��Ϊ�ֽ�����
        Texture2D texture = rawImage.texture as Texture2D;
        if (texture == null)
        {
            Debug.LogError("RawImage texture is not a Texture2D!");
            yield break;
        }

        byte[] imageData = texture.EncodeToPNG();

        // 4. ������� color �� y �����ݶ���
        var data = new
        {
            color = colorParameter,
            y = yParameter
        };

        string jsonData = JsonConvert.SerializeObject(data);

        // 5. ʹ�� WWWForm ��������
        WWWForm form = new WWWForm();
        form.AddField("data", jsonData);
        form.AddBinaryData("image", imageData, "image.png", "image/png");

        // 6. ��������������ȫ������
        using (UnityWebRequest webRequest = UnityWebRequest.Post(apiUrl, form))
        {
            webRequest.certificateHandler = new AcceptAllCertificates();
            DownloadHandlerAudioClip downloadHandler = new DownloadHandlerAudioClip(webRequest.url, AudioType.WAV);
            downloadHandler.streamAudio = true;
            webRequest.downloadHandler = downloadHandler;

            // ��������
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