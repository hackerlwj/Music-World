using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGenerator : MonoBehaviour
{
    public GameObject targetPrefab;
    public float[] spawnIntervals; // 节奏点生成间隔时间数组
    public int[] spawnIndex; // 节奏点出现位置数组
    public List<Transform> spawnPoints; // 生成位置列表
    private int spawnCount = 0;
    private bool isGenerating = false; // 标志位，用于控制是否开始生成

    public void StartGenerator()
    {
        isGenerating = true; // 设置标志位为true，表示开始生成
        StartCoroutine(GenerateTargets()); // 启动协程开始生成预制体
    }

    private IEnumerator GenerateTargets()
    {
        while (spawnCount < spawnIntervals.Length)
        {
            if (spawnPoints.Count > 0) // 确保spawnPoints中有元素
            {
                // 生成预制体
                Instantiate(targetPrefab, spawnPoints[spawnIndex[spawnCount]].position + new Vector3(0, 0, -1), Quaternion.identity);
                spawnCount++;

                // 等待指定的间隔时间
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

        // 所有预制体都已生成，开始显示游戏结束面板
        StartCoroutine(ShowGameOverPanel());
        // 停止更新，避免重复调用
        this.enabled = false;
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(3f); // 延时
        GameManager.Instance.GameOver(); // 显示游戏结束面板
    }
}