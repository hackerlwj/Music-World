using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class RawImageEffect : MonoBehaviour, IPointerClickHandler
{
    public float jumpStrength = 1f; // ������ǿ��
    public float duration = 0.5f; // �����ĳ���ʱ��
    public int jumpCount = 1; // �����Ĵ���
    private AudioSource audioSource;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        audioSource=gameObject.AddComponent<AudioSource>();
        AudioClip clip = Resources.Load<AudioClip>("SoundSource/Test");

        // ����AudioSource������
        audioSource.clip = clip;
        audioSource.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(audioSource.clip);
        // ��������Ч��
        rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
    }
}