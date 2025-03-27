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
        public List<RawImage> drawingAreas; // 修改为列表
        public int brushSize;
        public Color color = Color.white;
        public Color backgroundColor = Color.black;
        public LineManager lineManager;
        private Texture2D[] textures; // 存储每个RawImage对应的Texture2D
        private bool isDrawing = false;
        private List<Vector2> drawPoints = new List<Vector2>();
        public List<Button> brushColors;
        private int currentDrawingAreaIndex = 0; // 当前正在使用的RawImage索引

        void Start()
        {
            InitializeTextures();
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
                            color = new Color(225f / 255f, 118f / 255f, 0); // 设置为橙色
                            break;
                        case "blue":
                            color = new Color(100f / 255f, 149f / 255f, 237f / 255f); // 设置为蓝色
                            break;
                        case "purple":
                            color = new Color(130f / 255f, 34f / 255f, 243f / 255f); // 设置为紫色
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
            return textures[currentDrawingAreaIndex];
        }

        void InitializeTextures()
        {
            textures = new Texture2D[drawingAreas.Count];
            for (int i = 0; i < drawingAreas.Count; i++)
            {
                textures[i] = new Texture2D((int)drawingAreas[i].rectTransform.rect.width, (int)drawingAreas[i].rectTransform.rect.height, TextureFormat.RGB24, false);
                textures[i].wrapMode = TextureWrapMode.Clamp;
                textures[i].filterMode = FilterMode.Point;
                ClearCanvas(i);
                drawingAreas[i].texture = textures[i];
                if (i != currentDrawingAreaIndex)
                {
                    drawingAreas[i].gameObject.SetActive(false); // 禁用除第一个之外的所有RawImage
                }
            }
        }

        public void ClearCanvas(int index)
        {
            for (int x = 0; x < textures[index].width; x++)
                for (int y = 0; y < textures[index].height; y++)
                    textures[index].SetPixel(x, y, backgroundColor);
            textures[index].Apply();
        }

        public void SaveTextureAsPNG()
        {
            byte[] bytes = textures[currentDrawingAreaIndex].EncodeToPNG();
            SaveBytesToFile(bytes, System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png");
        }

        private void SaveBytesToFile(byte[] bytes, string filename)
        {
            string path = Path.Combine(Application.persistentDataPath, "SavedDrawings");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(path, filename);
            File.WriteAllBytes(path, bytes);
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
            Debug.Log($"Saved {filename} to {path}");
            ClearCanvas(currentDrawingAreaIndex);
            isDrawing = false;
            // 获取当前RawImage的坐标
            Vector3 rawImagePosition = drawingAreas[currentDrawingAreaIndex].rectTransform.anchoredPosition;

            lineManager.CreateLine(path, color, rawImagePosition);
            SwitchToNextDrawingArea();
        }

        void DrawWithMouse()
        {
            Vector2 mousePos = Input.mousePosition;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingAreas[currentDrawingAreaIndex].rectTransform, mousePos, canvasCamera, out Vector2 localPoint))
            {
                Vector2 texturePoint = GetTexturePoint(localPoint, currentDrawingAreaIndex);
                if (drawPoints.Count == 0 || Vector2.Distance(drawPoints[drawPoints.Count - 1], texturePoint) > 2)
                    drawPoints.Add(texturePoint);
                if (drawPoints.Count >= 3)
                    DrawBezierCurve(drawPoints[drawPoints.Count - 3], drawPoints[drawPoints.Count - 2], drawPoints[drawPoints.Count - 1], color, brushSize, currentDrawingAreaIndex);
            }
        }

        Vector2 GetTexturePoint(Vector2 localPoint, int index)
        {
            float width = drawingAreas[index].rectTransform.rect.width;
            float height = drawingAreas[index].rectTransform.rect.height;
            float xNormalized = Mathf.Clamp((localPoint.x / width + 0.5f), 0, 1);
            float yNormalized = Mathf.Clamp((localPoint.y / height + 0.5f), 0, 1);
            return new Vector2((int)(xNormalized * textures[index].width),
                               (int)(yNormalized * textures[index].height));
        }

        void DrawBezierCurve(Vector2 p0, Vector2 p1, Vector2 p2, Color color, int thickness, int index)
        {
            p0 = ClampPoint(p0, index);
            p1 = ClampPoint(p1, index);
            p2 = ClampPoint(p2, index);

            int steps = Mathf.Max(5, (int)(Vector2.Distance(p0, p2) / 10));
            for (int i = 0; i < steps; i++)
            {
                float t1 = (float)i / steps;
                float t2 = (float)(i + 1) / steps;
                Vector2 point1 = CalculateBezierPoint(t1, p0, p1, p2);
                Vector2 point2 = CalculateBezierPoint(t2, p0, p1, p2);
                DrawLine((int)point1.x, (int)point1.y, (int)point2.x, (int)point2.y, color, thickness, index);
            }
        }

        Vector2 CalculateBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }

        void DrawLine(int x0, int y0, int x1, int y1, Color color, int thickness, int index)
        {
            x0 = Mathf.Clamp(x0, 0, textures[index].width - 1);
            y0 = Mathf.Clamp(y0, 0, textures[index].height - 1);
            x1 = Mathf.Clamp(x1, 0, textures[index].width - 1);
            y1 = Mathf.Clamp(y1, 0, textures[index].height - 1);

            int dx = Mathf.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
            int dy = -Mathf.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;
            while (true)
            {
                DrawPoint(x0, y0, color, thickness, index);
                if (x0 == x1 && y0 == y1) break;
                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
            textures[index].Apply();
        }

        void DrawPoint(int x, int y, Color color, int thickness, int index)
        {
            for (int r = 1; r <= thickness; r++)
            {
                int d = 1 - r;
                int xCoord = 0, yCoord = r;
                while (xCoord <= yCoord)
                {
                    DrawSymmetricPixels(x, y, xCoord, yCoord, color, index);
                    if (d < 0) d += 2 * xCoord + 3;
                    else { d += 2 * (xCoord - yCoord) + 5; yCoord--; }
                    xCoord++;
                }
            }
        }

        void DrawSymmetricPixels(int centerX, int centerY, int x, int y, Color color, int index)
        {
            SetPixelSafe(centerX + x, centerY + y, color, index);
            SetPixelSafe(centerX - x, centerY + y, color, index);
            SetPixelSafe(centerX + x, centerY - y, color, index);
            SetPixelSafe(centerX - x, centerY - y, color, index);
            SetPixelSafe(centerX + y, centerY + x, color, index);
            SetPixelSafe(centerX - y, centerY + x, color, index);
            SetPixelSafe(centerX + y, centerY - x, color, index);
            SetPixelSafe(centerX - y, centerY - x, color, index);
        }

        void SetPixelSafe(int x, int y, Color color, int index)
        {
            if (x >= 0 && x < textures[index].width && y >= 0 && y < textures[index].height)
                textures[index].SetPixel(x, y, color);
        }

        // 切换到下一个RawImage
        private void SwitchToNextDrawingArea()
        {
            drawingAreas[currentDrawingAreaIndex].gameObject.SetActive(false); // 禁用当前的RawImage
            if (drawingAreas.Count > currentDrawingAreaIndex + 1)
            {
                currentDrawingAreaIndex = currentDrawingAreaIndex + 1; // 切换到下一个索引
                drawingAreas[currentDrawingAreaIndex].gameObject.SetActive(true); // 启用下一个RawImage
                drawPoints.Clear(); // 清空绘制点
            }
        }

        Vector2 ClampPoint(Vector2 point, int index)
        {
            return new Vector2(Mathf.Clamp((int)point.x, 0, textures[index].width - 1),
                               Mathf.Clamp((int)point.y, 0, textures[index].height - 1));
        }
    }
}