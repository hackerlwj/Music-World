using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RawImageEffect : MonoBehaviour, IPointerClickHandler
{
    public float jumpStrength = 1f; // ������ǿ��
    public float duration = 0.5f; // �����ĳ���ʱ��
    public int jumpCount = 1; // �����Ĵ���
    private AudioSource audioSource;
    private RectTransform rectTransform;
    public LineManager lineManager;
    public GameObject persistentCanvas; // Ŀ�껭��������Ϊ PersistentCanvas
    public TMP_Text noteText; // �Ѿ����ڵ��ı�����

    void Start()
    {
        // ���� PersistentCanvas
        if (persistentCanvas == null)
        {
            persistentCanvas = GameObject.Find("PersistentCanvas");
        }

        // ��ȡ LineManager ���
        if (lineManager == null && persistentCanvas != null)
        {
            lineManager = persistentCanvas.GetComponent<LineManager>();
        }

        rectTransform = GetComponent<RectTransform>();
        audioSource = gameObject.AddComponent<AudioSource>();

        // ���� lineList ���������ز�ͬ����Ƶ����
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

        // ���� AudioSource ������
        audioSource.playOnAwake = false;

        // ��ȡ PersistentCanvas �����ı� NoteText
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
        // ������Ч
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // ��������Ч��
        if (rectTransform != null)
        {
            rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutBounce);
        }

        // ��ȡ��ǰ UI Ԫ�ص� y ����
        if (rectTransform != null)
        {
            Vector2 screenPos = rectTransform.anchoredPosition;

            // ���� y ���귶Χ���в�ͬ����
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