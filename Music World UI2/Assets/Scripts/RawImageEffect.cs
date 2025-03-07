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
        audioSource = gameObject.GetComponent<AudioSource>();

        // ���� lineList ���������ز�ͬ����Ƶ����
        if (lineManager != null)
        {
            AudioClip clip = null;
            switch (lineManager.lineList.Count)
            {
                case 1:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/1_grandpiano_G3����ɫ����ǣ�");
                    break;
                case 2:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/2_vibraphone_G4��С���ӣ�");
                    break;
                case 3:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/3_soulroadpiano_G3����ɫ����ǣ�");
                    break;
                case 4:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/4_violinarco_E3���������ϵ�һ��̫����");
                    break;
                case 5:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/5_violinarco_A3���ڶ���̫����");
                    break;
                case 6:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/6_violinarco_E4��������̫����");
                    break;
                case 7:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/10_Flute_F����ɫ������");
                    break;
                case 8:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/11_handpan_C3��С����");
                    break;
                case 9:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/12_mellotoon_D3��С�ˣ�");
                    break;
                case 10:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/13_GMS_B3����ɫС�㣩");
                    break;
                case 11:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/14_featheredflute_C4����ɫ�ƣ�");
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