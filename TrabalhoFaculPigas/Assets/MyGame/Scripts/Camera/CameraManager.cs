using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraManager : UniversalAdditionalCameraData
{
    [SerializeField] private GameObject actualTarget;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private Vector2 camLimits;

    private static CameraManager instance;

    void Start()
    {
        actualTarget = actualTarget == null ? GameManagement.PlayerObject : actualTarget;

        if (instance == null)
        {
            instance = this;
           //  DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LateUpdate()
    {
        if (actualTarget != null)
        {
            Vector3 desiredPosition = new Vector3(actualTarget.transform.position.x, actualTarget.transform.position.y, -10f);

            // Limita a posição da câmera dentro dos limites
            float clampedX = Mathf.Clamp(desiredPosition.x, -camLimits.x, camLimits.x);
            float clampedY = Mathf.Clamp(desiredPosition.y, -camLimits.y, camLimits.y);

            Vector3 clampedPosition = new Vector3(clampedX, clampedY, desiredPosition.z);

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, clampedPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public CameraManager SetLevelLimits(Vector2 limits)
    {
        camLimits = limits;
        return this;
    }
}