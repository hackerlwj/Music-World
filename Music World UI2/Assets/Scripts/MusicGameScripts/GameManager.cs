using UnityEngine;
using DG.Tweening;
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
    public CanvasGroup gameOverPanelCanvasGroup; // ��Ϸ��������CanvasGroup���

    public float duration = 0.5f; // ��������ʱ��
    public Ease easeType = Ease.OutBack; // ������������

    void Awake()
    {
        Instance = this;
        scoreText.text = $"Score: 0";
        gameOverPanel.SetActive(false);
        // ��ʼʱ������岻�ɽ������������赲
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
        scoreEndText.text = "�÷�: " + currentScore;
        perfectText.text = "Perfect: " + perfectCount;
        goodText.text = "Good: " + goodCount;
        missText.text = "Miss: " + missCount;
        gameOverPanel.SetActive(true); // ��ʾ��Ϸ�������
        ShowGameOverPanel();
    }

    private void ShowGameOverPanel()
    {
        if (gameOverPanelCanvasGroup != null)
        {
            // ���ý�����
            gameOverPanelCanvasGroup.interactable = true;
            gameOverPanelCanvasGroup.blocksRaycasts = true;
            // ���ŵ�������
            gameOverPanelCanvasGroup.DOFade(1, duration).SetEase(easeType);
            gameOverPanelCanvasGroup.transform.DOScale(Vector3.one, duration).SetEase(easeType);
        }
    }

    public void CloseGameOverPanel()
    {
        if (gameOverPanelCanvasGroup != null)
        {
            // ���ý�����
            gameOverPanelCanvasGroup.interactable = false;
            gameOverPanelCanvasGroup.blocksRaycasts = false;

            // �������ض���
            Sequence sequence = DOTween.Sequence();
            sequence.Append(gameOverPanelCanvasGroup.DOFade(0, duration).SetEase(easeType));
            sequence.Join(gameOverPanelCanvasGroup.transform.DOScale(Vector3.zero, duration).SetEase(easeType));

            // ������ɺ�ִ�лص�
            sequence.OnComplete(() =>
            {
                // ȷ�������ȫ���ɼ�
                gameOverPanelCanvasGroup.alpha = 0;
                gameOverPanelCanvasGroup.transform.localScale = Vector3.zero;
            });
        }
    }

    public void RestartGame()
    {
        // ���õ÷ֺͼ���
        currentScore = 0;
        perfectCount = 0;
        goodCount = 0;
        missCount = 0;

        // ���µ÷���ʾ
        scoreText.text = $"Score: 0";

        // �ر���Ϸ�������
        CloseGameOverPanel();
    }
}