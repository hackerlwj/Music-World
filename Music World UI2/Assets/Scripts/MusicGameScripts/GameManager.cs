using UnityEngine;
using DG.Tweening;
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
    public CanvasGroup gameOverPanelCanvasGroup; // 游戏结束面板的CanvasGroup组件

    public float duration = 0.5f; // 动画持续时间
    public Ease easeType = Ease.OutBack; // 动画缓动类型

    void Awake()
    {
        Instance = this;
        scoreText.text = $"Score: 0";
        gameOverPanel.SetActive(false);
        // 初始时设置面板不可交互且无射线阻挡
        if (gameOverPanelCanvasGroup != null)
        {
            gameOverPanelCanvasGroup.alpha = 0;
            gameOverPanelCanvasGroup.transform.localScale = Vector3.zero;
            gameOverPanelCanvasGroup.interactable = false;
            gameOverPanelCanvasGroup.blocksRaycasts = false;
        }
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
        ShowGameOverPanel();
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanelCanvasGroup != null)
        {
            // 启用交互性
            gameOverPanelCanvasGroup.interactable = true;
            gameOverPanelCanvasGroup.blocksRaycasts = true;
            // 播放弹出动画
            gameOverPanelCanvasGroup.DOFade(1, duration).SetEase(easeType);
            gameOverPanelCanvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
        }
    }

    public void CloseGameOverPanel()
    {
        if (gameOverPanelCanvasGroup != null)
        {
            // 禁用交互性
            gameOverPanelCanvasGroup.interactable = false;
            gameOverPanelCanvasGroup.blocksRaycasts = false;

            // 播放隐藏动画
            Sequence sequence = DOTween.Sequence();
            sequence.Append(gameOverPanelCanvasGroup.DOFade(0, duration).SetEase(easeType));
            sequence.Join(gameOverPanelCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));

            // 动画完成后执行回调
            sequence.OnComplete(() =>
            {
                // 确保面板完全不可见
                gameOverPanelCanvasGroup.alpha = 0;
                gameOverPanelCanvasGroup.transform.localScale = Vector3.zero;
            });
        }
    }

    public void RestartGame()
    {
        // 重置得分和计数
        currentScore = 0;
        perfectCount = 0;
        goodCount = 0;
        missCount = 0;

        // 更新得分显示
        scoreText.text = $"Score: 0";

        // 关闭游戏结束面板
        CloseGameOverPanel();
    }
}