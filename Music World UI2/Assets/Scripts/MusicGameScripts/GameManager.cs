using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int perfectCount = 0; // Perfect����
    public int goodCount = 0; // Good����
    public int missCount = 0; // Miss����

    public static GameManager Instance;
    public Text scoreText;
    private int currentScore;

    public Text scoreEndText; // �÷��ı�
    public Text perfectText; // Perfect�ı�
    public Text goodText; // Good�ı�
    public Text missText; // Miss�ı�
    public GameObject gameOverPanel; // ��Ϸ�������

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
        scoreEndText.text = "�÷�: " + currentScore;
        perfectText.text = "Perfect: " + perfectCount;
        goodText.text = "Good: " + goodCount;
        missText.text = "Miss: " + missCount;
        gameOverPanel.SetActive(true); // ��ʾ��Ϸ�������
    }
}