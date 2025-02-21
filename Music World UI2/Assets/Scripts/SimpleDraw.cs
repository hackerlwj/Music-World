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
        public int brushSize;
        public Color color = Color.white;
        public Color backgroundColor = Color.black;
        public LineManager lineManager;
        private Texture2D texture;
        private bool isDrawing = false;
        private List<Vector2> drawPoints = new List<Vector2>();
        public List<Button> brushColors;

        void Start()
        {
            InitializeTexture();
            RegBtn();
        }

        void RegBtn()
        {
            foreach (var btn in brushColors)
            {
                btn.onClick.AddListener(() =>
                {
                    string tag = btn.tag; // 获取按钮的标签
                    switch (tag)
                    {
                        case "orange":
                            color = new Color(225f/255f, 118f/255f, 0); // 设置为橙色
                            break;
                        case "blue":
                            color = new Color(100f/255f, 149f/255f, 237f / 255f); // 设置为蓝色
                            break;
                        case "purple":
                            color = new Color(130f / 255f, 34f/ 255f, 243f/255f); // 设置为紫色
                            break;
                        default:
                            break;
                    }
                });
            }
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDrawing = true;
                drawPoints.Clear();
            }
            if (Input.GetMouseButtonUp(0))
            {
                isDrawing = false;
                drawPoints.Clear();
            }
            if (isDrawing) DrawWithMouse();
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
            for (int x = 0; x < texture.width; x++)
                for (int y = 0; y < texture.height; y++)
                    texture.SetPixel(x, y, backgroundColor);
            texture.Apply();
        }

        public void SaveTextureAsPNG()
        {
            byte[] bytes = texture.EncodeToPNG();
            SaveBytesToFile(bytes, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
        }

        private void SaveBytesToFile(byte[] bytes, string filename)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "SavedDrawings");
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, filename);
            File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            Debug.Log($"Saved {filename} to {path}");
            ClearCanvas();
            isDrawing = false;
            lineManager.CreateLine(path, color);
        }

        void DrawWithMouse()
        {
            Vector2 mousePos = Input.mousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingArea.rectTransform, mousePos, canvasCamera, out Vector2 localPoint))
            {
                Vector2 texturePoint = GetTexturePoint(localPoint);
                if (drawPoints.Count == 0 || Vector2.Distance(drawPoints[drawPoints.Count - 1], texturePoint) > 2)
                    drawPoints.Add(texturePoint);
                if (drawPoints.Count >= 3)
                    DrawBezierCurve(drawPoints[drawPoints.Count - 3], drawPoints[drawPoints.Count - 2], drawPoints[drawPoints.Count - 1], color, brushSize);
            }
        }

        Vector2 GetTexturePoint(Vector2 localPoint)
        {
            float width = drawingArea.rectTransform.rect.width;
            float height = drawingArea.rectTransform.rect.height;
            if (Mathf.Abs(localPoint.x / width) > 0.5f || Mathf.Abs(localPoint.y / height) > 0.5f) return Vector2.zero;
            float xNormalized = (localPoint.x / width + 0.5f);
            float yNormalized = (localPoint.y / height + 0.5f);
            return new Vector2(Mathf.Clamp((int)(xNormalized * texture.width), 0, texture.width - 1),
                               Mathf.Clamp((int)(yNormalized * texture.height), 0, texture.height - 1));
        }

        void DrawBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Color color, int thickness)
        {
            int steps = Mathf.Max(5, (int)(Vector2.Distance(p0, p2) / 10));
            for (int i = 0; i < steps; i++)
            {
                float t1 = (float)i / steps;
                float t2 = (float)(i + 1) / steps;
                Vector2 point1 = CalculateBezierPoint(t1, p0, p1, p2);
                Vector2 point2 = CalculateBezierPoint(t2, p0, p1, p2);
                DrawLine((int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y, color, thickness);
            }
        }

        Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }

        void DrawLine(int x0, int y0, int x1, int y1, Color color, int thickness)
        {
            int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while (true)
            {
                DrawPoint(x0, y0, color, thickness);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
            texture.Apply();
        }

        void DrawPoint(int x, int y, Color color, int thickness)
        {
            for (int r = 1; r <= thickness; r++)
            {
                int d = 1 - r;
                int xCoord = 0, yCoord = r;
                while (xCoord <= yCoord)
                {
                    DrawSymmetricPixels(x, y, xCoord, yCoord, color);
                    if (d < 0) d += 2 * xCoord + 3;
                    else { d += 2 * (xCoord - yCoord) + 5; yCoord--; }
                    xCoord++;
                }
            }
        }

        void DrawSymmetricPixels(int centerX, int centerY, int x, int y, Color color)
        {
            SetPixelSafe(centerX + x, centerY + y, color);
            SetPixelSafe(centerX - x, centerY + y, color);
            SetPixelSafe(centerX + x, centerY - y, color);
            SetPixelSafe(centerX - x, centerY - y, color);
            SetPixelSafe(centerX + y, centerY + x, color);
            SetPixelSafe(centerX - y, centerY + x, color);
            SetPixelSafe(centerX + y, centerY - x, color);
            SetPixelSafe(centerX - y, centerY - x, color);
        }

        void SetPixelSafe(int x, int y, Color color)
        {
            if (x >= 0 && x < texture.width && y >= 0 && y < texture.height)
                texture.SetPixel(x, y, color);
        }
    }
}