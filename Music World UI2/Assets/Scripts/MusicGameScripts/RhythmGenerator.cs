using System.Collections;
using System.Collections.Generic; // ����List����������ռ�
using UnityEngine;

public class RhythmGenerator : MonoBehaviour
{
    public GameObject targetPrefab;
    public float[] spawnTimes; // ��������ʱ������
    public List<Transform> spawnPoints; // ����λ���б�

    void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets()
    {
        foreach (float time in spawnTimes)
        {
            yield return new WaitForSeconds(time);
            if (spawnPoints.Count > 0) // ȷ��spawnPoints����Ԫ��
            {
                int posIndex = Random.Range(0, spawnPoints.Count); // ʹ��Count��ȡ�б���
                Instantiate(targetPrefab, spawnPoints[posIndex].position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("No spawn points available!");
            }
        }
        StartCoroutine(ShowGameOverPanel());
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3f); // ��ʱ
        GameManager.Instance.GameOver(); // ��ʾ��Ϸ�������
    }
}