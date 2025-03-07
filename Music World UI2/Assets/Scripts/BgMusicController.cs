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

    // ��ʼ������Ϸ��Ƶ
    public void StartGameAudio()
    {
        if (gameAudioSource != null)
        {
            gameAudioSource.Play();
        }
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
            StartCoroutine(PlayAudioWithDelay(entity1AudioSource, delay));
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
            StartCoroutine(PlayAudioWithDelay(entity2AudioSource, delay));
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
            StartCoroutine(PlayAudioWithDelay(entity3AudioSource, delay));
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
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource1, delay));
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
            StartCoroutine(PlayAudioWithDelay(aiPlayAudioSource2, delay));
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
            endAudioSource.Play();
        }
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
    private IEnumerator PlayAudioWithDelay(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}