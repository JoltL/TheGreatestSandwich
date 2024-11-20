using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    private Vector3 _camerapos;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _minZoom = 8f;
    [SerializeField] private float _maxZoom = 5f;
    [SerializeField] private float _zoomLimit = 13f;

    [SerializeField] private float _zoomSpeed = 0.5f;

    [SerializeField] private List<Transform> _targets = new();

    private Vector3 _velocity;


    private void Start()
    {
        _camera = GetComponent<Camera>();

        _camerapos = transform.position;
    }

    private void Update()
    {

            if (_targets.Count < 2)
                return;

            Move();
            Zoom();

    }
    public void AddTarget(Transform target)
    {
        _targets.Add(target);
    }

    private void Zoom()
    {
        float targetZoom = Mathf.Lerp(_minZoom, _maxZoom,
            GetGreatestDistance() / _zoomLimit);
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize,
            targetZoom, 10 * Time.deltaTime);
    }

    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 targetPosition = new Vector3(transform.position.x, centerPoint.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _zoomSpeed);
    }

    public void RemoveTarget(Transform target)
    {
        _targets.Remove(target);
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(_targets[0].position, Vector3.zero);

        for (int i = 0; i < _targets.Count; i++)
        {
            bounds.Encapsulate(_targets[i].position);
        }

        return Mathf.Max(bounds.size.x, bounds.size.y);
    }

    private Vector3 GetCenterPoint()
    {
        if (_targets.Count == 1)
            return _targets[0].position;

        var bounds = new Bounds(_targets[0].position, Vector3.zero);
        for (int i = 0; i < _targets.Count; i++)
        {
            bounds.Encapsulate(_targets[i].position);
        }

        return bounds.center;
    }
}

