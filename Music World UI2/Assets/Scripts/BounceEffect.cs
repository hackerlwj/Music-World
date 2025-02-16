using UnityEngine;
using UnityEngine.UI;

public class BounceEffect : MonoBehaviour
{
    public RawImage rawImage; // ָ�� RawImage ���
    public Vector3 targetPosition; // Ŀ��λ��
    public float gravity = -9.8f; // �������ٶ�
    public float heightOffset = 0.5f; // �����ߵ����߶�ƫ��
    public float rangeOffset = 0.1f; // ˮƽ����ƫ��

    private Rigidbody rb;

    private void Start()
    {
        // ��ȡ RawImage �� RectTransform
        RectTransform rectTransform = rawImage.rectTransform;

        // ��� Rigidbody ���
        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false; // �����˶�ѧģʽ���������������
        rb.useGravity = true; // ��������

        // ���ó�ʼλ��
        rectTransform.position = rawImage.rectTransform.position;

        // �����ʼ�ٶ�
        Vector3 initialVelocity = PhysicsUtil.GetParabolaInitVelocity(
            rectTransform.position,
            targetPosition,
            gravity,
            heightOffset,
            rangeOffset
        );

        // ʩ�ӳ�ʼ��
        rb.AddForce(initialVelocity * rb.mass, ForceMode.Impulse);
    }

    private void Update()
    {
        // �������Ŀ��λ�ã�ֹͣ����ģ��
        if (Vector3.Distance(rb.position, targetPosition) < 0.1f)
        {
            rb.isKinematic = true; // �����˶�ѧģʽ��ֹͣ����ģ��
            rb.velocity = Vector3.zero; // ֹͣ�˶�
            rawImage.rectTransform.position = targetPosition; // ����ΪĿ��λ��
        }
    }
}

public static class PhysicsUtil
{
    public static Vector3 GetParabolaInitVelocity(
        Vector3 from,
        Vector3 to,
        float gravity = -9.8f,
        float heightOffset = 0.0f,
        float rangeOffset = 0.1f
    )
    {
        Vector3 newVel = new Vector3();
        Vector3 direction = new Vector3(to.x, 0f, to.z) - new Vector3(from.x, 0f, from.z);
        float range = direction.magnitude + rangeOffset;
        Vector3 unitDirection = direction.normalized;
        float maxYPos = to.y + heightOffset;

        if (maxYPos < from.y)
        {
            maxYPos = from.y;
        }

        float ft = -2.0f * gravity * (maxYPos - from.y);
        if (ft < 0) ft = 0f;
        newVel.y = Mathf.Sqrt(ft);

        ft = -2.0f * (maxYPos - from.y) / gravity;
        if (ft < 0) ft = 0f;
        float timeToMax = Mathf.Sqrt(ft);

        ft = -2.0f * (maxYPos - to.y) / gravity;
        if (ft < 0) ft = 0f;
        float timeToTargetY = Mathf.Sqrt(ft);

        float totalFlightTime = timeToMax + timeToTargetY;

        float horizontalVelocityMagnitude = range / totalFlightTime;
        newVel.x = horizontalVelocityMagnitude * unitDirection.x;
        newVel.z = horizontalVelocityMagnitude * unitDirection.z;

        return newVel;
    }
}