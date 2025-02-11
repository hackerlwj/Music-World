using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int perfectCount = 0; // Perfect次数
    public int goodCount = 0; // Good次数
    public int missCount = 0; // Miss次数

    public static GameManager Instance;
    public Text scoreText;
    private int currentScore;

    public Text scoreEndText; // 得分文本
    public Text perfectText; // Perfect文本
    public Text goodText; // Good文本
    public Text missText; // Miss文本
    public GameObject gameOverPanel; // 游戏结束面板

    void Awake()
    {
        Instance = this;
        scoreText.text = $"Score: 0";
        gameOverPanel.SetActive(false);
    }

    public void AddScore(int value)
    {
        currentScore += value;
        scoreText.text = $"Score: {currentScore}";
    }

    public void AddPerfect()
    {
        perfectCount++;
    }

    public void AddGood()
    {
        goodCount++;
    }

    public void AddMiss()
    {
        missCount++;
    }

    public void GameOver()
    {
        scoreEndText.text = "得分: " + currentScore;
        perfectText.text = "Perfect: " + perfectCount;
        goodText.text = "Good: " + goodCount;
        missText.text = "Miss: " + missCount;
        gameOverPanel.SetActive(true); // 显示游戏结束面板
    }
}