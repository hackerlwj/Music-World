using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RawImageEffect : MonoBehaviour, IPointerClickHandler
{
    public float jumpStrength = 1f; // 跳动的强度
    public float duration = 0.5f; // 跳动的持续时间
    public int jumpCount = 1; // 跳动的次数
    private AudioSource audioSource;
    private RectTransform rectTransform;
    public LineManager lineManager;
    public GameObject persistentCanvas; // 目标画布，命名为 PersistentCanvas
    public TMP_Text noteText; // 已经存在的文本对象

    void Start()
    {
        // 查找 PersistentCanvas
        if (persistentCanvas == null)
        {
            persistentCanvas = GameObject.Find("PersistentCanvas");
        }

        // 获取 LineManager 组件
        if (lineManager == null && persistentCanvas != null)
        {
            lineManager = persistentCanvas.GetComponent<LineManager>();
        }

        rectTransform = GetComponent<RectTransform>();
        audioSource = gameObject.AddComponent<AudioSource>();

        // 根据 lineList 的数量加载不同的音频剪辑
        if (lineManager != null)
        {
            AudioClip clip = null;
            switch (lineManager.lineList.Count)
            {
                case 1:
                    clip = Resources.Load<AudioClip>("SoundSource/8_violinpizzicato_E4");
                    break;
                case 2:
                    clip = Resources.Load<AudioClip>("SoundSource/3_glockenspiel_E5");
                    break;
                case 3:
                    clip = Resources.Load<AudioClip>("SoundSource/3_guitar_E2");
                    break;
                case 4:
                    clip = Resources.Load<AudioClip>("SoundSource/4_violinpizzicato_G3");
                    break;
            }
            audioSource.clip = clip;
        }

        // 设置 AudioSource 的属性
        audioSource.playOnAwake = false;

        // 获取 PersistentCanvas 的子文本 NoteText
        if (persistentCanvas != null)
        {
            Transform noteTextTransform = persistentCanvas.transform.Find("NoteText");
            if (noteTextTransform != null)
            {
                noteText = noteTextTransform.GetComponent<TMP_Text>();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // 播放音效
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // 触发跳动效果
        if (rectTransform != null)
        {
            rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
        }

        // 获取当前 UI 元素的 y 坐标
        if (rectTransform != null)
        {
            Vector2 screenPos = rectTransform.anchoredPosition;

            // 根据 y 坐标范围进行不同操作
            if (screenPos.y >= -100 && screenPos.y < 0)
            {
                UpdateExistingText("do");
                DestroySpecificPrefab("do(Clone)");
            }
            else if (screenPos.y >= 0 && screenPos.y < 100)
            {
                UpdateExistingText("re");
                DestroySpecificPrefab("re(Clone)");
            }
        }
    }

    private void UpdateExistingText(string textContent)
    {
        if (noteText != null)
        {
            noteText.text = textContent;
        }
    }

    private void DestroySpecificPrefab(string targetName)
    {
        MultiPrefabMoveAndDestroy[] movingObjects = FindObjectsOfType<MultiPrefabMoveAndDestroy>();
        foreach (MultiPrefabMoveAndDestroy obj in movingObjects)
        {
            if (obj.isCollidingWithLine && obj.gameObject.name == targetName)
            {
                Destroy(obj.gameObject);
            }
        }
    }
}