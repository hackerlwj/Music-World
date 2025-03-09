using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGenerator : MonoBehaviour
{
    public GameObject targetPrefab;
    public float[] spawnIntervals; // ��������ɼ��ʱ������
    public int[] spawnIndex; // ��������λ������
    public List<Transform> spawnPoints; // ����λ���б�
    private int spawnCount = 0;
    private bool isGenerating = false; // ��־λ�����ڿ����Ƿ�ʼ����

    public void StartGenerator()
    {
        isGenerating = true; // ���ñ�־λΪtrue����ʾ��ʼ����
        StartCoroutine(GenerateTargets()); // ����Э�̿�ʼ����Ԥ����
    }

    private IEnumerator GenerateTargets()
    {
        while (spawnCount < spawnIntervals.Length)
        {
            if (spawnPoints.Count > 0) // ȷ��spawnPoints����Ԫ��
            {
                // ����Ԥ����
                Instantiate(targetPrefab, spawnPoints[spawnIndex[spawnCount]].position + new Vector3(0, 0, -1), Quaternion.identity);
                spawnCount++;

                // �ȴ�ָ���ļ��ʱ��
                if (spawnCount < spawnIntervals.Length)
                {
                    yield return new WaitForSeconds(spawnIntervals[spawnCount]);
                }
            }
            else
            {
                Debug.LogError("No spawn points available!");
                break;
            }
        }

        // ����Ԥ���嶼�����ɣ���ʼ��ʾ��Ϸ�������
        StartCoroutine(ShowGameOverPanel());
        // ֹͣ���£������ظ�����
        this.enabled = false;
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3f); // ��ʱ
        GameManager.Instance.GameOver(); // ��ʾ��Ϸ�������
    }
}