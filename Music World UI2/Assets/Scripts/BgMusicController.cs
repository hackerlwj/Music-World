using UnityEngine;
using System.Collections;

public class BgMusicController : MonoBehaviour
{
    public AudioSource gameAudioSource;
    public AudioSource entity1AudioSource;
    public AudioSource entity2AudioSource;
    public AudioSource entity3AudioSource;
    public AudioSource aiPlayAudioSource1;
    public AudioSource aiPlayAudioSource2;
    public AudioSource endAudioSource;

    // 开始播放游戏音频
    public void StartGameAudio()
    {
        if (gameAudioSource != null)
        {
            gameAudioSource.Play();
        }
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
            StartCoroutine(PlayAudioWithDelay(entity1AudioSource, delay));
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
            StartCoroutine(PlayAudioWithDelay(entity2AudioSource, delay));
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
            StartCoroutine(PlayAudioWithDelay(entity3AudioSource, delay));
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
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource1, delay));
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
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource2, delay));
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
            endAudioSource.Play();
        }
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
    private IEnumerator PlayAudioWithDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}