using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float[] spawnIntervals; // ÿ��Ԥ�����Ӧ�����ɼ��ʱ��
    public float spawnXScreen;
    public float spawnYScreen;
    public GameObject gameCanvas;

    private bool isSpawning = false; // ����Ƿ�ʼ����

    // ��ʼ����Ԥ����ĺ���
    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnPrefabs()); // ����Э�̿�ʼ����Ԥ����
    }

    private IEnumerator SpawnPrefabs()
    {
        for (int i = 0; i < spawnIntervals.Length; i++)
        {
            // �ȴ�ָ���ļ��ʱ��
            yield return new WaitForSeconds(spawnIntervals[i]);

            // ���� Spawn ��������Ԥ����
            Spawn(i);
        }
    }

    private void Spawn(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= prefabs.Length)
        {
            Debug.LogError("Ԥ�����ų�����Χ��");
            return;
        }

        GameObject selectedPrefab = prefabs[prefabIndex];
        Vector3 screenSpawnPosition = new Vector3(spawnXScreen, spawnYScreen, 0f);
        GameObject instantiatedObject = Instantiate(selectedPrefab, Vector3.zero, Quaternion.identity);

        if (gameCanvas != null)
        {
            instantiatedObject.transform.SetParent(gameCanvas.transform, false);
            RectTransform rectTransform = instantiatedObject.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = new Vector2(screenSpawnPosition.x, screenSpawnPosition.y);
            }
        }
    }
}
