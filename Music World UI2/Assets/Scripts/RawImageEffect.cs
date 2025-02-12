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

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource=gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("SoundSource/Test");

        // 设置AudioSource的属性
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(audioSource.clip);
        // 触发跳动效果
        rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
    }
}