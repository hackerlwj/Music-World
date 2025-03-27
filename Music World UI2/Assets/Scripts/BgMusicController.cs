using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class BgMusicController : MonoBehaviour
{
    public GameObject playPromptImage;
    public Sprite[] endDog1Frames; // 存储 endDog1 的图片帧
    public Sprite[] endDog2Frames; // 存储 endDog2 的图片帧
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
    public float gifFrameRate = 10f; // GIF 动画的帧率

    void Start()
    {
        Application.targetFrameRate = 280;
        entity1Circle.transform.localScale = Vector3.zero; // 初始时将物体缩小
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

    // 开始播放游戏音频
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

    // 停止播放游戏音频
    public void StopGameAudio()
    {
        if (gameAudioSource != null)
        {
            gameAudioSource.Stop();
        }
    }

    // 开始延时播放实体1音频
    public void StartEntity1AudioWithDelay(float delay)
    {
        if (entity1AudioSource != null)
        {
            StartCoroutine(PlayAudioWithDelay(entity1AudioSource, delay, entity1Circle));
        }
    }

    // 停止播放实体1音频
    public void StopEntity1Audio()
    {
        if (entity1AudioSource != null)
        {
            entity1AudioSource.Stop();
        }
    }

    // 开始延时播放实体2音频
    public void StartEntity2AudioWithDelay(float delay)
    {
        if (entity2AudioSource != null)
        {
            StartCoroutine(PlayAudioWithDelay(entity2AudioSource, delay, entity2Circle));
        }
    }

    // 停止播放实体2音频
    public void StopEntity2Audio()
    {
        if (entity2AudioSource != null)
        {
            entity2AudioSource.Stop();
        }
    }

    // 开始延时播放实体3音频
    public void StartEntity3AudioWithDelay(float delay)
    {
        if (entity3AudioSource != null)
        {
            StartCoroutine(PlayAudioWithDelay(entity3AudioSource, delay, entity3Circle));
        }
    }

    // 停止播放实体3音频
    public void StopEntity3Audio()
    {
        if (entity3AudioSource != null)
        {
            entity3AudioSource.Stop();
        }
    }

    // 开始延时播放AI1音频
    public void StartAIPlayAudio1WithDelay(float delay)
    {
        if (aiPlayAudioSource1 != null)
        {
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource1, delay, null));
        }
    }

    // 停止播放AI1音频
    public void StopAIPlayAudio1()
    {
        if (aiPlayAudioSource1 != null)
        {
            aiPlayAudioSource1.Stop();
        }
    }

    // 开始延时播放AI2音频
    public void StartAIPlayAudio2WithDelay(float delay)
    {
        if (aiPlayAudioSource2 != null)
        {
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource2, delay, null));
        }
    }

    // 停止播放AI2音频
    public void StopAIPlayAudio2()
    {
        if (aiPlayAudioSource2 != null)
        {
            aiPlayAudioSource2.Stop();
        }
    }

    // 开始播放结束音频
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
        // 使用 DOTween 实现透明度从 0 到 1 的渐变
        Tween fadeTween = endDog1.DOFade(1f, fadeDuration);
        endDog2.DOFade(1f, fadeDuration);

        // 等待渐变完成
        yield return fadeTween.WaitForCompletion();

        // 开始播放 GIF 动画
        StartCoroutine(PlayGIF(endDog1, endDog1Frames));
        StartCoroutine(PlayGIF(endDog2, endDog2Frames));
    }

    // 停止播放结束音频
    public void StopEndAudio()
    {
        if (endAudioSource != null)
        {
            endAudioSource.Stop();
        }
    }

    // 协程函数，用于延时播放音频
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
            // 使用 DOTween 实现从小变大的动画
            circle.transform.DOScale(new Vector3(2, 2, 2), duration).SetEase(easeType);
            circle.transform.DORotate(new Vector3(0, 0, 360), 360f / rotationSpeed, RotateMode.FastBeyond360)
               .SetEase(Ease.Linear)
               .SetLoops(-1, LoopType.Incremental);
        }
    }

    // 播放 GIF 动画的协程
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
