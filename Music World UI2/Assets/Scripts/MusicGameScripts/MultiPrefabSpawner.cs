using UnityEngine;

public class MultiPrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float spawnRate = 1f;
    public float spawnXScreen;
    public float fixedYScreen;
    public GameObject gameCanvas;

    private void OnEnable()
    {
        // 当 GameCanvas 激活时，启动重复调用 Spawn 方法的协程
        InvokeRepeating("Spawn", 0f, spawnRate);
    }

    private void OnDisable()
    {
        // 当 GameCanvas 禁用时，停止重复调用 Spawn 方法
        CancelInvoke("Spawn");
    }

    private void Spawn()
    {
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject selectedPrefab = prefabs[randomIndex];
        Vector3 screenSpawnPosition = new Vector3(spawnXScreen, fixedYScreen, 0f);
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