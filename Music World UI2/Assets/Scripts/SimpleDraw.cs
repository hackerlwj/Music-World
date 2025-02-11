using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace TheSimpleDraw
{
    public class SimpleDraw : MonoBehaviour
    {
        public Camera canvasCamera;

        public RawImage drawingArea;
        public int brushSize; // brush size

        public Color color = Color.white; //brush color
        public Color backgroundColor = Color.black; // bg color

        public LineManager lineManager;
        private Texture2D texture;
        private bool isDrawing = false;
        private Vector2 lastDrawPoint = Vector2.zero;

        public List<Button> brushColors;

        void Start()
        {
            InitializeTexture();

            RegBtn();
        }
        void RegBtn()
        {
            foreach (var item in brushColors)
            {
                item.onClick.AddListener(() => color = item.GetComponent<Image>().color);
            }
        }


        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDrawing = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrawing = false;
                lastDrawPoint = Vector2.zero;
            }

            if (isDrawing)
            {
                DrawWithMouse();
            }
        }

        public Texture2D outputTexture2D()
        {
            return texture;
        }

        void InitializeTexture()
        {
            texture = new Texture2D((int)drawingArea.rectTransform.rect.width, (int)drawingArea.rectTransform.rect.height, TextureFormat.RGB24, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Point;

            ClearCanvas();
            drawingArea.texture = texture;

        }

        public void ClearCanvas()
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    texture.SetPixel(i, j, backgroundColor);
                }
            }
            texture.Apply();
        }




        public void SaveTextureAsPNG()
        {

            byte[] bytes = texture.EncodeToPNG();
            SaveBytesToFile(bytes, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");

        }
        private void SaveBytesToFile(byte[] bytes, string filename)
        {
            if (!Directory.Exists(Application.streamingAssetsPath + "/SavedDrawings"))
            {
                Directory.CreateDirectory(Application.streamingAssetsPath + "/SavedDrawings");
            }
            string path = Application.streamingAssetsPath + "/SavedDrawings/" + filename;
            File.WriteAllBytes(path, bytes);

#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif

            Debug.Log($"Saved {filename} to {path}");
            ClearCanvas();
            isDrawing = false;
            lineManager.CreateLine(path);
        }


        void DrawWithMouse()
        {
            if (!isDrawing) return;

            Vector2 mousePos = Input.mousePosition;

            // 使用指定的摄像头进行坐标转换
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingArea.rectTransform, mousePos, canvasCamera, out Vector2 localPoint))
            {
                float width = drawingArea.rectTransform.rect.width;
                float height = drawingArea.rectTransform.rect.height;

                if (Mathf.Abs(localPoint.x / width) > 0.5f)
                {
                    return;
                }

                if (Mathf.Abs(localPoint.y / height) > 0.5f)
                {
                    return;
                }

                float xNormalized = (localPoint.x / width + 0.5f);
                float yNormalized = (localPoint.y / height + 0.5f);

                int x = Mathf.Clamp((int)(xNormalized * texture.width), 0, texture.width - 1);
                int y = Mathf.Clamp((int)(yNormalized * texture.height), 0, texture.height - 1);

                Vector2 texturePoint = new Vector2(x, y);

                if (lastDrawPoint == Vector2.zero)
                {
                    lastDrawPoint = texturePoint;
                }

                DrawLine((int)lastDrawPoint.x, (int)lastDrawPoint.y, (int)texturePoint.x, (int)texturePoint.y, color, brushSize);
                lastDrawPoint = texturePoint;
            }
        }

        void DrawLine(int x0, int y0, int x1, int y1, Color color, int thickness)
        {
            int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy, e2;

            for (; ; )
            {  /* loop */
                DrawPoint(x0, y0, color, thickness);
                if (x0 == x1 && y0 == y1) break;
                e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }

            texture.Apply();
        }

        void DrawPoint(int x, int y, Color color, int thickness)
        {
            for (int i = -thickness; i < thickness; i++)
            {
                for (int j = -thickness; j < thickness; j++)
                {
                    if (x + i >= 0 && x + i < texture.width && y + j >= 0 && y + j < texture.height)
                    {
                        texture.SetPixel(x + i, y + j, color);
                    }
                }
            }
            texture.Apply();
        }
    }
}
