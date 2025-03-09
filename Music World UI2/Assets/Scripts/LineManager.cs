using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LineManager : MonoBehaviour
{
    public RhythmGenerator rhythmGenerator;
    private Canvas canvas;
    //public GameObject linePrefab;
    public Material lineMaterial;
    public List<GameObject> lineList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        canvas= gameObject.GetComponent<Canvas>();
    }

    //public void ImportImage()
    //{
    //    // ���ļ�ѡ����
    //    string path = UnityEditor.EditorUtility.OpenFilePanel("Select Image", "", "png,jpg,jpeg");

    //    if (!string.IsNullOrEmpty(path))
    //    {
    //        GameObject line = new GameObject("Line_" + lineList.Count.ToString());
    //        line.transform.SetParent(canvas.transform);
    //        // ��ȡͼƬ�ļ�
    //        byte[] bytes = File.ReadAllBytes(path);

    //        // ���� Texture2D ����
    //        Texture2D texture = new Texture2D(2, 2);
    //        texture.LoadImage(bytes);

    //        RawImage rawImage = line.AddComponent<RawImage>();
    //        rawImage.texture = texture;// ������ֵ��RawImage
    //        //rawImage.material = lineMaterial;// �����ʸ�ֵ��RawImage
    //        line.AddComponent<CanvasGroup>();
    //        line.AddComponent<DrawLine>();
    //        line.AddComponent<RawImageEffect>();

    //        rawImage.rectTransform.anchoredPosition = new Vector2(0, 0);
    //        rawImage.rectTransform.sizeDelta = new Vector2(400, 400);

    //        lineList.Add(line);
    //        rhythmGenerator.spawnPoints.Add(line.transform);
    //    }
    //}
    public void CreateLine(string filePath,Color color, Vector3 rawImagePosition)
    {
        GameObject line = new GameObject("Line_" + lineList.Count.ToString());
        line.transform.SetParent(canvas.transform);
        //GameObject line = Instantiate(linePrefab);
        
        Texture2D texture = LoadPNG(filePath);// ����PNGͼƬ
        RawImage rawImage=line.AddComponent<RawImage>();
        rawImage.color = color;
        rawImage.texture = texture;// ������ֵ��RawImage
        rawImage.material = lineMaterial;// �����ʸ�ֵ��RawImage
        line.AddComponent<AudioSource>();
        line.AddComponent<CanvasGroup>();
        line.AddComponent<RawImageEffect>();       
        line.AddComponent<DrawLine>();
        
        rawImage.rectTransform.localPosition = rawImagePosition;
        rawImage.rectTransform.sizeDelta = new Vector2(25f, 25f); // ����RawImage�Ĵ�С
        //RawImage rawImage=line.GetComponent<RawImage>();
        
        lineList.Add(line);
        rhythmGenerator.spawnPoints.Add(line.transform);
    }
    private Texture2D LoadPNG(string filePath)
    {
        // ��ȡ�ļ�����
        byte[] fileData = System.IO.File.ReadAllBytes(filePath);

        // ����һ���µ�Texture2D����
        Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        texture.LoadImage(fileData); // ����ͼ������
        texture.Apply(); // Ӧ���������

        return texture;
    }

    public void PreButton()
    {
        if(lineList.Count - 1>=0)
        {
            Destroy(lineList[lineList.Count - 1].gameObject);
            lineList.RemoveAt(lineList.Count - 1);
            rhythmGenerator.spawnPoints.RemoveAt(rhythmGenerator.spawnPoints.Count - 1);
        }
        
    }
    public void ClearButton()
    {
        for (int i = 0; i < lineList.Count; i++)
        {
            Destroy(lineList[i].gameObject);
        }
        lineList.Clear();
        rhythmGenerator.spawnPoints.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
