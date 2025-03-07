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
        audioSource = gameObject.GetComponent<AudioSource>();

        // 根据 lineList 的数量加载不同的音频剪辑
        if (lineManager != null)
        {
            AudioClip clip = null;
            switch (lineManager.lineList.Count)
            {
                case 1:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/1_grandpiano_G3（橙色五角星）");
                    break;
                case 2:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/2_vibraphone_G4（小房子）");
                    break;
                case 3:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/3_soulroadpiano_G3（蓝色五角星）");
                    break;
                case 4:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/4_violinarco_E3（从下往上第一个太阳）");
                    break;
                case 5:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/5_violinarco_A3（第二个太阳）");
                    break;
                case 6:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/6_violinarco_E4（第三个太阳）");
                    break;
                case 7:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/10_Flute_F（蓝色的树）");
                    break;
                case 8:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/11_handpan_C3（小车）");
                    break;
                case 9:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/12_mellotoon_D3（小人）");
                    break;
                case 10:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/13_GMS_B3（蓝色小鱼）");
                    break;
                case 11:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/14_featheredflute_C4（紫色云）");
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
            if (screenPos.y < -23)
            {
                UpdateExistingText("do");
                DestroySpecificPrefab("do(Clone)");
            }
            else if (screenPos.y >= -23 && screenPos.y < -17)
            {
                UpdateExistingText("re");
                DestroySpecificPrefab("re(Clone)");
            }
            else if (screenPos.y >= -17 && screenPos.y < -10)
            {
                UpdateExistingText("mi");
                DestroySpecificPrefab("mi(Clone)");
            }
            else if (screenPos.y >= -10 && screenPos.y < -6)
            {
                UpdateExistingText("fa");
                DestroySpecificPrefab("fa(Clone)");
            }
            else if (screenPos.y >= -6 && screenPos.y < 0)
            {
                UpdateExistingText("sol");
                DestroySpecificPrefab("sol(Clone)");
            }
            else if (screenPos.y >= 0 && screenPos.y < 5)
            {
                UpdateExistingText("la");
                DestroySpecificPrefab("la(Clone)");
            }
            else if (screenPos.y >= 5 && screenPos.y < 11)
            {
                UpdateExistingText("ti");
                DestroySpecificPrefab("ti(Clone)");
            }
            else if (screenPos.y >= 11 && screenPos.y < 17)
            {
                UpdateExistingText("do'");
                DestroySpecificPrefab("do'(Clone)");
            }
            else if (screenPos.y >= 17 && screenPos.y < 23)
            {
                UpdateExistingText("re'");
                DestroySpecificPrefab("re'(Clone)");
            }
            else if (screenPos.y >= 23 && screenPos.y < 29)
            {
                UpdateExistingText("mi'");
                DestroySpecificPrefab("mi'(Clone)");
            }
            else if (screenPos.y >= 29 && screenPos.y < 35)
            {
                UpdateExistingText("fa'");
                DestroySpecificPrefab("fa'(Clone)");
            }
            else if (screenPos.y >= 35 && screenPos.y < 41)
            {
                UpdateExistingText("sol'");
                DestroySpecificPrefab("sol'(Clone)");
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
                obj.SuccessEffect();
                Destroy(obj.gameObject);
            }
        }
    }
}