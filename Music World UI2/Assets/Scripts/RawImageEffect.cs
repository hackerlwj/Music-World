using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RawImageEffect : MonoBehaviour
{
    public float jumpStrength = 5f; // 跳动的强度
    public float duration = 0.3f; // 跳动的持续时间
    public int jumpCount = 1; // 跳动的次数
    private AudioSource audioSource;
    private RectTransform rectTransform;
    public LineManager lineManager;
    public GameObject persistentCanvas; // 目标画布，命名为 PersistentCanvas
    public TMP_Text noteText; // 已经存在的文本对象
    private float currentJumpHeight = 0f;
    public bool isCircling=false;//游戏模式缩圈判断
    public ClickableTarget clickableTarget = null;
    private Sequence jumpSequence;
    private Vector2 startPosition;

    void Start()
    {
        
        // 初始化跳跃序列
        jumpSequence = DOTween.Sequence();
        isCircling = false;
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
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/2_vibraphone_G3（小房子）");
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
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/12_guitar_D3（小人）");
                    break;
                case 10:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/13_GMS_B3（蓝色小鱼）");
                    break;
                case 11:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/14_featheredflute_C4（紫色云）");
                    break;
                case 12:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/15_featheredflute_D4（紫色云2）");
                    break;
                case 13:
                    clip = Resources.Load<AudioClip>("SoundSource/复赛视频演示/第2部分/16_violinpizzicato_G4（六芒星）");
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
        startPosition = rectTransform.anchoredPosition;
        // 添加 Button 组件
        Button button = gameObject.AddComponent<Button>();
        // 为 Button 的点击事件添加监听
        button.onClick.AddListener(OnClicking);
    }

    public void OnClicking()
    {
        if(isCircling)
        {
            clickableTarget.OnHaloClick();
        }
        // 播放音效
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // 触发跳动效果
        if (rectTransform != null)
        {
            // 停止当前正在进行的动画
            if (jumpSequence.IsActive())
            {
                jumpSequence.Kill();
            }

            // 让物体回到原点
            rectTransform.anchoredPosition = startPosition;

            // 开始新的跳跃动画
            jumpSequence = rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutQuad).SetAutoKill(false);
        }

        // 获取当前 UI 元素的 y 坐标
        if (rectTransform != null)
        {
            // 根据 y 坐标范围进行不同操作
            if (startPosition.y < -236)
            {
                UpdateExistingText("do");
                DestroySpecificPrefab("do(Clone)");
            }
            else if (startPosition.y >= -236 && startPosition.y < -178)
            {
                UpdateExistingText("re");
                DestroySpecificPrefab("re(Clone)");
            }
            else if (startPosition.y >= -178 && startPosition.y < -116)
            {
                UpdateExistingText("mi");
                DestroySpecificPrefab("mi(Clone)");
            }
            else if (startPosition.y >= -116 && startPosition.y < -69)
            {
                UpdateExistingText("fa");
                DestroySpecificPrefab("fa(Clone)");
            }
            else if (startPosition.y >= -69 && startPosition.y < -26)
            {
                UpdateExistingText("sol");
                DestroySpecificPrefab("sol(Clone)");
            }
            else if (startPosition.y >= -26 && startPosition.y < 51)
            {
                UpdateExistingText("la");
                DestroySpecificPrefab("la(Clone)");
            }
            else if (startPosition.y >= 51 && startPosition.y < 98)
            {
                UpdateExistingText("ti");
                DestroySpecificPrefab("ti(Clone)");
            }
            else if (startPosition.y >= 98 && startPosition.y < 161)
            {
                UpdateExistingText("do'");
                DestroySpecificPrefab("do'(Clone)");
            }
            else if (startPosition.y >= 161 && startPosition.y < 211)
            {
                UpdateExistingText("re'");
                DestroySpecificPrefab("re'(Clone)");
            }
            else if (startPosition.y >= 211 && startPosition.y < 268)
            {
                UpdateExistingText("mi'");
                DestroySpecificPrefab("mi'(Clone)");
            }
            else if (startPosition.y >= 268 && startPosition.y < 325)
            {
                UpdateExistingText("fa'");
                DestroySpecificPrefab("fa'(Clone)");
            }
            else if (startPosition.y >= 325 && startPosition.y < 405)
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