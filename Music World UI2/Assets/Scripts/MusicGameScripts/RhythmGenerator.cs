using System.Collections;
using System.Collections.Generic; // 引入List所需的命名空间
using UnityEngine;

public class RhythmGenerator : MonoBehaviour
{
    public GameObject targetPrefab;
    public float[] spawnTimes; // 节奏点出现时间数组
    public List<Transform> spawnPoints; // 生成位置列表

    void Start()
    {
        StartCoroutine(SpawnTargets());
    }

    IEnumerator SpawnTargets()
    {
        foreach (float time in spawnTimes)
        {
            yield return new WaitForSeconds(time);
            if (spawnPoints.Count > 0) // 确保spawnPoints中有元素
            {
                int posIndex = Random.Range(0, spawnPoints.Count); // 使用Count获取列表长度
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
        yield return new WaitForSeconds(3f); // 延时
        GameManager.Instance.GameOver(); // 显示游戏结束面板
    }
}