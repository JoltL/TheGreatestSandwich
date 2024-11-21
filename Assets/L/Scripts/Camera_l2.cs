using System.Collections.Generic;
using UnityEngine;

public class Camera_l2 : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _minZoom = 8f;
    [SerializeField] private float _maxZoom = 5f;
    [SerializeField] private float _zoomOffset = 2f;

    [SerializeField] private float _zoomSpeed = 0.5f;

    [SerializeField] private List<Transform> _targets = new();


    private void Start()
    {
        _camera = GetComponent<Camera>();

    }

    public void AddTarget(Transform target)
    {
        _targets.Add(target);
    }

    public void RemoveTarget(Transform target)
    {
        if (_targets.Contains(target))
            _targets.Remove(target);
    }

    private void LateUpdate()
    {
       if (_targets.Count == 0)
            return;

        if (_targets.Count == 1)
        {
            transform.position = new Vector3(transform.position.x, _targets[0].position.y + _offset.y, transform.position.z);
            _camera.orthographicSize = _minZoom + _zoomOffset;
            return;
        }

        Move();
        Zoom();
    }

    private void Move()
    {
        float centerY = GetCenterPoint();
        Vector3 targetPosition = new Vector3(transform.position.x, centerY + _offset.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, _zoomSpeed);
    }


    private void Zoom()
    {
        // Calcul de la plus grande distance entre les cibles
        float greatestDistance = GetGreatestDistance();

        // Le zoom minimum est défini comme la distance maximale entre les cibles, avec un offset supplémentaire
        float targetZoom = greatestDistance + _zoomOffset;

        // Clamping pour rester dans les limites spécifiées
        targetZoom = Mathf.Clamp(targetZoom, _maxZoom, _minZoom);

        // Application du zoom avec interpolation pour une transition fluide
        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, targetZoom, 0.1f);
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
