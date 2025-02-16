using UnityEngine;
using UnityEngine.UI;

public class BounceEffect : MonoBehaviour
{
    public RawImage rawImage; // 指定 RawImage 组件
    public Vector3 targetPosition; // 目标位置
    public float gravity = -9.8f; // 重力加速度
    public float heightOffset = 0.5f; // 抛物线的最大高度偏移
    public float rangeOffset = 0.1f; // 水平距离偏移

    private Rigidbody rb;

    private void Start()
    {
        // 获取 RawImage 的 RectTransform
        RectTransform rectTransform = rawImage.rectTransform;

        // 添加 Rigidbody 组件
        rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false; // 禁用运动学模式，让物理引擎控制
        rb.useGravity = true; // 启用重力

        // 设置初始位置
        rectTransform.position = rawImage.rectTransform.position;

        // 计算初始速度
        Vector3 initialVelocity = PhysicsUtil.GetParabolaInitVelocity(
            rectTransform.position,
            targetPosition,
            gravity,
            heightOffset,
            rangeOffset
        );

        // 施加初始力
        rb.AddForce(initialVelocity * rb.mass, ForceMode.Impulse);
    }

    private void Update()
    {
        // 如果到达目标位置，停止物理模拟
        if (Vector3.Distance(rb.position, targetPosition) < 0.1f)
        {
            rb.isKinematic = true; // 启用运动学模式，停止物理模拟
            rb.velocity = Vector3.zero; // 停止运动
            rawImage.rectTransform.position = targetPosition; // 设置为目标位置
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