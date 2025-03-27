using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class RawImageEffect : MonoBehaviour
{
    public float jumpStrength = 5f; // ������ǿ��
    public float duration = 0.3f; // �����ĳ���ʱ��
    public int jumpCount = 1; // �����Ĵ���
    private AudioSource audioSource;
    private RectTransform rectTransform;
    public LineManager lineManager;
    public GameObject persistentCanvas; // Ŀ�껭��������Ϊ PersistentCanvas
    public TMP_Text noteText; // �Ѿ����ڵ��ı�����
    private float currentJumpHeight = 0f;
    public bool isCircling=false;//��Ϸģʽ��Ȧ�ж�
    public ClickableTarget clickableTarget = null;
    private Sequence jumpSequence;
    private Vector2 startPosition;

    void Start()
    {
        
        // ��ʼ����Ծ����
        jumpSequence = DOTween.Sequence();
        isCircling = false;
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
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/2_vibraphone_G3��С���ӣ�");
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
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/12_guitar_D3��С�ˣ�");
                    break;
                case 10:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/13_GMS_B3����ɫС�㣩");
                    break;
                case 11:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/14_featheredflute_C4����ɫ�ƣ�");
                    break;
                case 12:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/15_featheredflute_D4����ɫ��2��");
                    break;
                case 13:
                    clip = Resources.Load<AudioClip>("SoundSource/������Ƶ��ʾ/��2����/16_violinpizzicato_G4����â�ǣ�");
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
        startPosition = rectTransform.anchoredPosition;
        // ��� Button ���
        Button button = gameObject.AddComponent<Button>();
        // Ϊ Button �ĵ���¼���Ӽ���
        button.onClick.AddListener(OnClicking);
    }

    public void OnClicking()
    {
        if(isCircling)
        {
            clickableTarget.OnHaloClick();
        }
        // ������Ч
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        // ��������Ч��
        if (rectTransform != null)
        {
            // ֹͣ��ǰ���ڽ��еĶ���
            if (jumpSequence.IsActive())
            {
                jumpSequence.Kill();
            }

            // ������ص�ԭ��
            rectTransform.anchoredPosition = startPosition;

            // ��ʼ�µ���Ծ����
            jumpSequence = rectTransform.DOJump(rectTransform.position, jumpStrength, jumpCount, duration).SetEase(Ease.OutQuad).SetAutoKill(false);
        }

        // ��ȡ��ǰ UI Ԫ�ص� y ����
        if (rectTransform != null)
        {
            // ���� y ���귶Χ���в�ͬ����
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