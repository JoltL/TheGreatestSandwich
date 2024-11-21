using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _minZoom = 8f;
    [SerializeField] private float _maxZoom = 5f;
    //[SerializeField] private float _zoomLimit = 13f;

    [SerializeField] private float _zoomSpeed = 0.5f;

    public List<Transform> _targets = new();

    private Vector3 _velocity = Vector3.zero;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {

        if (_targets.Count == 0)
            return;

        if (_targets.Count == 1)
        {
            //transform.position = new Vector3(transform.position.x, _targets[0].position.y + _offset.y, transform.position.z);
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _maxZoom, _zoomSpeed * Time.deltaTime);
            return;
        }

        //if (_targets.Count < 2)
        //    return;

        Move();
        Zoom();

    }
    public void AddTarget(Transform target)
    {
        _targets.Add(target);
    }
    public void RemoveTarget(Transform target)
    {
        _targets.Remove(target);
    }

    private void Zoom()
    {
        float targetZoom = Mathf.Lerp(_minZoom, _maxZoom, GetGreatestDistance());
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, _zoomSpeed* Time.deltaTime);

    }

    private void Move()
    {
        float centerPoint = GetCenterPoint();
        Vector3 targetPosition = new Vector3(transform.position.x, centerPoint, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, _zoomSpeed);
    }

    public void EndZoom()
    {
        float greatestDistance = GetGreatestDistance();

        float targetZoom = greatestDistance + 2f;

        //// Clamping pour rester dans les limites spécifiées
        //targetZoom = Mathf.Clamp(targetZoom, _maxZoom, _minZoom);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, 10 * Time.deltaTime);
    }

    private float GetGreatestDistance()
    {
        float minY = _targets[0].position.y;
        float maxY = _targets[0].position.y;

        for (int i = 1; i < _targets.Count; i++)
        {
            minY = Mathf.Min(minY, _targets[i].position.y);
            maxY = Mathf.Max(maxY, _targets[i].position.y);
        }

        return maxY - minY;
    }

    private float GetCenterPoint()
    {
        float averageY = 0;

        for (int i = 0; i < _targets.Count; i++)
        {
            averageY += _targets[i].position.y;
        }

        return averageY / _targets.Count;
    }
}

