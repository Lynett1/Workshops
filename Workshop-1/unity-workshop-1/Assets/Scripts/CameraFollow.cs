using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 3f, 0f);
    [SerializeField] private float smoothSpeed = 5f;

    void LateUpdate()
    {
        if (!target) return;
        
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
