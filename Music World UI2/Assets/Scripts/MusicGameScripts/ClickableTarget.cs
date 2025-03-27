using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableTarget : MonoBehaviour
{
    public ParticleSystem hitEffect;
    public Image halo;
    public float scaleDuration = 1f; // ������ʱ��
    public AnimationCurve scaleCurve; // ������������
    public GameObject targetGameObject; // ��Ȧ��������λ���϶�Ӧ��Ϳѻ����

    private Vector3 initialScale = new Vector3(0.5f, 0.5f, 1f); // ��ʼ��С
    private bool isDestroying = false;

    void Start()
    {
        GameObject canvas = GameObject.Find("PersistentCanvas");
        transform.SetParent(canvas.transform);
        halo.transform.localScale = initialScale;
        StartCoroutine(ScaleHalo());
        // �Զ����ٱ���
        Destroy(gameObject, scaleDuration + 0.5f);
    }
    private void Update()
    {
        if (targetGameObject != null)
        {
            // ��ȡRawImageEffect���
            RawImageEffect rawImageEffect = targetGameObject.GetComponent<RawImageEffect>();

            // ���RawImageEffect����Ƿ����
            if (rawImageEffect != null)
            {
                rawImageEffect.isCircling = true;
                if(rawImageEffect.clickableTarget == null)
                    rawImageEffect.clickableTarget = this;
            }
            else
            {
                Debug.LogError("RawImageEffect component not found on the targetGameObject.");
            }
        }
        else
        {
            Debug.LogError("targetGameObject is null.");
        }
    }

    IEnumerator ScaleHalo()
    {
        float timer = 0;

        while (timer < scaleDuration)
        {
            float progress = timer / scaleDuration;
            // ʹ������ֵ��������
            float curveValue = scaleCurve.Evaluate(progress);
            halo.transform.localScale = initialScale * curveValue;

            timer += Time.deltaTime;
            yield return null;
        }

        // ���Ž���������
        if (!isDestroying)
        {
            OnMiss(); // δ�����ʱ����Miss
        }
    }

    public void OnHaloClick()
    {
        if (isDestroying) return;

        isDestroying = true;
        float scaleLevel = halo.transform.localScale.x / initialScale.x;
        if (scaleLevel < 0.3f)
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
        ParticleSystem hitEffectInstance = Instantiate(hitEffect, transform.position, Quaternion.identity);
        StartCoroutine(DestroyParticleSystemAfterPlay(hitEffectInstance));
        RawImageEffect rawImageEffect = targetGameObject.GetComponent<RawImageEffect>();
        rawImageEffect.isCircling = false;
        Destroy(gameObject);
    }

    void OnMiss()
    {
        //GameManager.Instance.ResetCombo();
        GameManager.Instance.AddMiss();
        ParticleSystem hitEffectInstance = Instantiate(hitEffect, transform.position, Quaternion.identity);
        StartCoroutine(DestroyParticleSystemAfterPlay(hitEffectInstance));
        Destroy(gameObject);
    }

    int CalculateScore()
    {
        float scaleProgress = 1 - halo.transform.localScale.x / initialScale.x;
        return Mathf.RoundToInt(Mathf.Lerp(50, 100, scaleProgress));
    }

    IEnumerator DestroyParticleSystemAfterPlay(ParticleSystem particleSystem)
    {
        // �ȴ�����ϵͳ�������
        while (particleSystem.isPlaying)
        {
            yield return null;
        }
        // ����ϵͳ������ɺ�����
        Destroy(particleSystem.gameObject);
    }
}