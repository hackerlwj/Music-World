using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float[] spawnIntervals; // 每个预制体对应的生成间隔时间
    public float spawnXScreen;
    public float spawnYScreen;
    public GameObject gameCanvas;

    private bool isSpawning = false; // 标记是否开始生成

    // 开始生成预制体的函数
    public void StartSpawning()
    {
        isSpawning = true;
        StartCoroutine(SpawnPrefabs()); // 启动协程开始生成预制体
    }

    private IEnumerator SpawnPrefabs()
    {
        for (int i = 0; i < spawnIntervals.Length; i++)
        {
            // 等待指定的间隔时间
            yield return new WaitForSeconds(spawnIntervals[i]);

            // 调用 Spawn 方法生成预制体
            Spawn(i);
        }
    }

    private void Spawn(int prefabIndex)
    {
        if (prefabIndex < 0 || prefabIndex >= prefabs.Length)
        {
            Debug.LogError("预制体编号超出范围！");
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
