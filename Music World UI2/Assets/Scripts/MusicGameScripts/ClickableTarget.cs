using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableTarget : MonoBehaviour, IPointerClickHandler
{
    public ParticleSystem hitEffect;
    public Image halo;
    public float scaleDuration = 1.5f; // 总缩放时间
    public AnimationCurve scaleCurve; // 控制缩放曲线

    private Vector3 initialScale = new Vector3(3f, 3f, 1f); // 初始大小
    private bool isDestroying = false;

    void Start()
    {
        GameObject canvas = GameObject.Find("GameCanvas");
        transform.SetParent(canvas.transform);
        halo.transform.localScale = initialScale;
        StartCoroutine(ScaleHalo());
        // 自动销毁保护
        Destroy(gameObject, scaleDuration + 0.5f);
    }

    IEnumerator ScaleHalo()
    {
        float timer = 0;

        while (timer < scaleDuration)
        {
            float progress = timer / scaleDuration;
            // 使用曲线值控制缩放
            float curveValue = scaleCurve.Evaluate(progress);
            halo.transform.localScale = initialScale * curveValue;

            timer += Time.deltaTime;
            yield return null;
        }

        // 缩放结束后销毁
        if (!isDestroying)
        {
            OnMiss(); // 未被点击时触发Miss
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDestroying) return;

        isDestroying = true;

        float scaleLevel = halo.transform.localScale.x / initialScale.x;
        if (scaleLevel<0.3f)
        {
            Debug.Log("perfect");
            GameManager.Instance.AddPerfect();
        }
        else
        {
            Debug.Log("good");
            GameManager.Instance.AddGood();
        }
        GameManager.Instance.AddScore(CalculateScore());
        ParticleSystem hitEffectprefab = Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnMiss()
    {
        //GameManager.Instance.ResetCombo();
        GameManager.Instance.AddMiss();
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    int CalculateScore()
    {
        float scaleProgress = 1 - halo.transform.localScale.x / initialScale.x;
        return Mathf.RoundToInt(Mathf.Lerp(50, 100, scaleProgress));
    }
}