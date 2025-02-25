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
    private int bufferLength = 60; // 录制时长（秒）

    private void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
        recordingBuffer = new float[sampleRate * bufferLength];
    }

    // 开始录制游戏内音频
    public void StartRecording()
    {
        if (isRecording) return;

        isRecording = true;
        bufferIndex = 0;
        Debug.Log("开始录制游戏内音频...");
    }

    // 停止录制游戏内音频
    public void StopRecording()
    {
        if (!isRecording) return;

        isRecording = false;
        // 声道数设置为 1
        recordedClip = AudioClip.Create("RecordedAudio", bufferIndex, 1, sampleRate, false);
        recordedClip.SetData(recordingBuffer, 0);

        Debug.Log("停止录制游戏内音频。");

        // 可以在这里对录制好的 AudioClip 进行其他操作
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (isRecording)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (bufferIndex < recordingBuffer.Length)
                {
                    // 单声道处理，简单存储数据
                    recordingBuffer[bufferIndex] = data[i];
                    bufferIndex++;
                }
                else
                {
                    // 缓冲区已满，停止录制
                    StopRecording();
                    break;
                }
            }
        }
    }
}