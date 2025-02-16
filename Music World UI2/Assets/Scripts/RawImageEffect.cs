using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RawImageEffect : MonoBehaviour, IPointerClickHandler
{
    public float jumpStrength = 1f; // 跳动的强度
    public float duration = 0.5f; // 跳动的持续时间
    public int jumpCount = 1; // 跳动的次数
    private AudioSource audioSource;

    private RectTransform rectTransform;
    public LineManager lineManager;

    void Start()
    {
        GameObject persistentCanvas = GameObject.Find("PersistentCanvas");

        // 获取LineManager组件
        lineManager = persistentCanvas.GetComponent<LineManager>();
        rectTransform = GetComponent<RectTransform>();
        audioSource=gameObject.AddComponent<AudioSource>();
        if(lineManager.lineList.Count == 1)
        {
            AudioClip clip = Resources.Load<AudioClip>("SoundSource/8_violinpizzicato_E4");
            audioSource.clip = clip;
        }
        if (lineManager.lineList.Count == 2)
        {
            AudioClip clip = Resources.Load<AudioClip>("SoundSource/3_glockenspiel_E5");
            audioSource.clip = clip;
        }
        if (lineManager.lineList.Count == 3)
        {
            AudioClip clip = Resources.Load<AudioClip>("SoundSource/3_guitar_E2");
            audioSource.clip = clip;
        }
        if (lineManager.lineList.Count == 4)
        {
            AudioClip clip = Resources.Load<AudioClip>("SoundSource/4_violinpizzicato_G3");
            audioSource.clip = clip;
        }
        //// 设置AudioSource的属性
        //audioSource.clip = clip;
        audioSource.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(audioSource.clip);
        // 触发跳动效果
        rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
    }
}