using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController I;

    public new Camera camera;
    public Transform shakeTransform;


    float shakeIntensity = 0;
    float shakeDuration;
    public Vector3 startPosition;
   

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        startPosition = shakeTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            shakeDuration -= Time.deltaTime;
            shakeTransform.localPosition = startPosition + Vector3.ProjectOnPlane(UnityEngine.Random.insideUnitSphere * (shakeDuration / 10)* shakeIntensity, shakeTransform.forward);

            if (shakeDuration < 0)
            {
                shakeTransform.localPosition = startPosition;
                shakeIntensity = 0;
            }
        }
    }

    public void Shake(float duration, float intensity)
    {
        if (!enabled)return;
        if (shakeIntensity > intensity)return;

        shakeDuration = duration;
        shakeIntensity = intensity;
    }

    public bool InsideView(Vector3 position)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}