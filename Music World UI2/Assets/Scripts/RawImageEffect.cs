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
    public LineManager lineManager;

    void Start()
    {
        GameObject persistentCanvas = GameObject.Find("PersistentCanvas");

        // ��ȡLineManager���
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
        //// ����AudioSource������
        //audioSource.clip = clip;
        audioSource.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.PlayOneShot(audioSource.clip);
        // ��������Ч��
        rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
    }
}