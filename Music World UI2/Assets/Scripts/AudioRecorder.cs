using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioRecorder : MonoBehaviour
{
    private AudioClip recordedClip;
    private float[] recordingBuffer;
    private int bufferIndex = 0;
    private bool isRecording = false;
    private int sampleRate;
    private int bufferLength = 60; // ¼��ʱ�����룩

    private void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
        recordingBuffer = new float[sampleRate * bufferLength];
    }

    // ��ʼ¼����Ϸ����Ƶ
    public void StartRecording()
    {
        if (isRecording) return;

        isRecording = true;
        bufferIndex = 0;
        Debug.Log("��ʼ¼����Ϸ����Ƶ...");
    }

    // ֹͣ¼����Ϸ����Ƶ
    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        // ����������Ϊ 1
        recordedClip = AudioClip.Create("RecordedAudio", bufferIndex, 1, sampleRate, false);
        recordedClip.SetData(recordingBuffer, 0);

        Debug.Log("ֹͣ¼����Ϸ����Ƶ��");

        // �����������¼�ƺõ� AudioClip ������������
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (isRecording)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (bufferIndex < recordingBuffer.Length)
                {
                    // �����������򵥴洢����
                    recordingBuffer[bufferIndex] = data[i];
                    bufferIndex++;
                }
                else
                {
                    // ������������ֹͣ¼��
                    StopRecording();
                    break;
                }
            }
        }
    }
}