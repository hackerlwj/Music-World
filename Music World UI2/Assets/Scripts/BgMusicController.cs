using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class BgMusicController : MonoBehaviour
{
    public GameObject playPromptImage;
    public Sprite[] endDog1Frames; // �洢 endDog1 ��ͼƬ֡
    public Sprite[] endDog2Frames; // �洢 endDog2 ��ͼƬ֡
    public Image endDog1;
    public Image endDog2;
    public float fadeDuration = 1f;
    public GameObject aiPlayPromptImage;
    public GameObject aiMelodyPromptImage;
    public GameObject aiMelodyPromptImage2;
    public float rotationSpeed = 90f;
    public float duration = 0.5f;
    public Ease easeType = Ease.OutBack;
    public GameObject entity1Circle;
    public GameObject entity2Circle;
    public GameObject entity3Circle;
    public AudioSource gameAudioSource;
    public AudioSource entity1AudioSource;
    public AudioSource entity2AudioSource;
    public AudioSource entity3AudioSource;
    public AudioSource aiPlayAudioSource1;
    public AudioSource aiPlayAudioSource2;
    public AudioSource endAudioSource;
    public float gifFrameRate = 10f; // GIF ������֡��

    void Start()
    {
        Application.targetFrameRate = 280;
        entity1Circle.transform.localScale = Vector3.zero; // ��ʼʱ��������С
        entity2Circle.transform.localScale = Vector3.zero;
        entity3Circle.transform.localScale = Vector3.zero;
        playPromptImage.SetActive(true);
        aiMelodyPromptImage.SetActive(false);
        aiPlayPromptImage.SetActive(false);
        aiMelodyPromptImage2.SetActive(false);
        endDog1.color = new Color(endDog1.color.r, endDog1.color.g, endDog1.color.b, 0f);
        endDog2.color = new Color(endDog2.color.r, endDog2.color.g, endDog2.color.b, 0f);
    }

    public void Reset()
    {
        StopAllCoroutines();
        playPromptImage.SetActive(true);
        aiMelodyPromptImage.SetActive(false);
        aiPlayPromptImage.SetActive(false);
        aiMelodyPromptImage2.SetActive(false);
        endDog1.color = new Color(endDog1.color.r, endDog1.color.g, endDog1.color.b, 0f);
        endDog2.color = new Color(endDog2.color.r, endDog2.color.g, endDog2.color.b, 0f);
    }

    // ��ʼ������Ϸ��Ƶ
    public void StartGameAudio()
    {
        if (gameAudioSource != null)
        {
            StartCoroutine(PlayAudioAfterDelay(2f));
        }
    }

    private IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameAudioSource.Play();
    }

    // ֹͣ������Ϸ��Ƶ
    public void StopGameAudio()
    {
        if (gameAudioSource != null)
        {
            gameAudioSource.Stop();
        }
    }

    // ��ʼ��ʱ����ʵ��1��Ƶ
    public void StartEntity1AudioWithDelay(float delay)
    {
        if (entity1AudioSource != null)
        {
            StartCoroutine(PlayAudioWithDelay(entity1AudioSource, delay, entity1Circle));
        }
    }

    // ֹͣ����ʵ��1��Ƶ
    public void StopEntity1Audio()
    {
        if (entity1AudioSource != null)
        {
            entity1AudioSource.Stop();
        }
    }

    // ��ʼ��ʱ����ʵ��2��Ƶ
    public void StartEntity2AudioWithDelay(float delay)
    {
        if (entity2AudioSource != null)
        {
            StartCoroutine(PlayAudioWithDelay(entity2AudioSource, delay, entity2Circle));
        }
    }

    // ֹͣ����ʵ��2��Ƶ
    public void StopEntity2Audio()
    {
        if (entity2AudioSource != null)
        {
            entity2AudioSource.Stop();
        }
    }

    // ��ʼ��ʱ����ʵ��3��Ƶ
    public void StartEntity3AudioWithDelay(float delay)
    {
        if (entity3AudioSource != null)
        {
            StartCoroutine(PlayAudioWithDelay(entity3AudioSource, delay, entity3Circle));
        }
    }

    // ֹͣ����ʵ��3��Ƶ
    public void StopEntity3Audio()
    {
        if (entity3AudioSource != null)
        {
            entity3AudioSource.Stop();
        }
    }

    // ��ʼ��ʱ����AI1��Ƶ
    public void StartAIPlayAudio1WithDelay(float delay)
    {
        if (aiPlayAudioSource1 != null)
        {
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource1, delay, null));
        }
    }

    // ֹͣ����AI1��Ƶ
    public void StopAIPlayAudio1()
    {
        if (aiPlayAudioSource1 != null)
        {
            aiPlayAudioSource1.Stop();
        }
    }

    // ��ʼ��ʱ����AI2��Ƶ
    public void StartAIPlayAudio2WithDelay(float delay)
    {
        if (aiPlayAudioSource2 != null)
        {
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource2, delay, null));
        }
    }

    // ֹͣ����AI2��Ƶ
    public void StopAIPlayAudio2()
    {
        if (aiPlayAudioSource2 != null)
        {
            aiPlayAudioSource2.Stop();
        }
    }

    // ��ʼ���Ž�����Ƶ
    public void StartEndAudio()
    {
        if (endAudioSource != null)
        {
            StartCoroutine(FadeInImage());
            endAudioSource.Play();
        }
    }

    IEnumerator FadeInImage()
    {
        // ʹ�� DOTween ʵ��͸���ȴ� 0 �� 1 �Ľ���
        Tween fadeTween = endDog1.DOFade(1f, fadeDuration);
        endDog2.DOFade(1f, fadeDuration);

        // �ȴ��������
        yield return fadeTween.WaitForCompletion();

        // ��ʼ���� GIF ����
        StartCoroutine(PlayGIF(endDog1, endDog1Frames));
        StartCoroutine(PlayGIF(endDog2, endDog2Frames));
    }

    // ֹͣ���Ž�����Ƶ
    public void StopEndAudio()
    {
        if (endAudioSource != null)
        {
            endAudioSource.Stop();
        }
    }

    // Э�̺�����������ʱ������Ƶ
    private IEnumerator PlayAudioWithDelay(AudioSource audioSource, float delay, GameObject circle)
    {
        yield return new WaitForSeconds(delay);
        if (audioSource == aiPlayAudioSource1 || audioSource == aiPlayAudioSource2)
        {
            playPromptImage.SetActive(false);
            aiPlayPromptImage.SetActive(true);
            yield return new WaitForSeconds(4f);
            aiPlayPromptImage.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
        audioSource.Play();

        if (audioSource == aiPlayAudioSource1)
        {
            aiMelodyPromptImage.SetActive(true);
        }
        else if (audioSource == aiPlayAudioSource2)
        {
            aiMelodyPromptImage.SetActive(false);
            aiMelodyPromptImage2.SetActive(true);
        }
        if (circle != null)
        {
            // ʹ�� DOTween ʵ�ִ�С���Ķ���
            circle.transform.DOScale(new Vector3(2, 2, 2), duration).SetEase(easeType);
            circle.transform.DORotate(new Vector3(0, 0, 360), 360f / rotationSpeed, RotateMode.FastBeyond360)
               .SetEase(Ease.Linear)
               .SetLoops(-1, LoopType.Incremental);
        }
    }

    // ���� GIF ������Э��
    private IEnumerator PlayGIF(Image image, Sprite[] frames)
    {
        int currentFrame = 0;
        while (true)
        {
            if (currentFrame >= frames.Length)
            {
                currentFrame = 0;
            }
            image.sprite = frames[currentFrame];
            currentFrame++;
            yield return new WaitForSeconds(1f / gifFrameRate);
        }
    }
}
