using UnityEngine;

[ExecuteAlways, RequireComponent(typeof(Camera))]
public class OrbitingCamera : MonoBehaviour
{
    private Transform _transform;

    [Header("Orbit Settings")]
    public Transform Target;
    public float Distance = 5f, YOffset = 10;
    private Vector3 _targetPosition;
    [Range(0f, 360f)]
    public float Angle = 0f;
    public float OrbitSpeed = 0f;

    private void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        _targetPosition = Target == null ? Vector3.zero : Target.position;

        if (Distance <= 0f) Distance = 0.1f;

        if (OrbitSpeed != 0f)
        {
            Angle += OrbitSpeed * Time.deltaTime;
            if (Angle > 360f) Angle -= 360f;
            else if (Angle < 0f) Angle += 360f;
        }

        float rad = Angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad)) * Distance;
        offset.y = YOffset;
        Vector3 newPosition = _targetPosition + offset;

        _transform.position = newPosition;
        _transform.LookAt(_targetPosition);
    }
}
