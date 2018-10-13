using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController I;

    public new Camera camera;
    public Transform shakeTransform;


    float shakeIntensity = 0;
    float shakeDuration;
    Vector3 startPosition, shakeStartPosition;
   

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        shakeStartPosition = shakeTransform.localPosition;
        startPosition = transform.position;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            shakeDuration -= Time.deltaTime;
            shakeTransform.localPosition = shakeStartPosition + Vector3.ProjectOnPlane(UnityEngine.Random.insideUnitSphere * (shakeDuration / 10)* shakeIntensity, shakeTransform.forward);

            if (shakeDuration < 0)
            {
                shakeTransform.localPosition = shakeStartPosition;
                shakeIntensity = 0;
            }
        }
        
        transform.position = Vector3.Lerp(transform.position, startPosition + Vector3.down * moveYTarget, leprSpeed * Time.deltaTime);
    }
    
    float leprSpeed = 5f;
    float moveYTarget = 0;
    
    public void MoveDownOneFloor()
    {
        moveYTarget += 4;
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