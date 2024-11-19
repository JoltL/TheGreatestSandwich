using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.TimeZoneInfo;

public class CameraFollows : MonoBehaviour
{
    [Header("General Settings")]
    [SerializeField] private float _followSpeed = 5f; // Vitesse du suivi vertical
    [SerializeField] private float _zoomSpeed = 5f;   // Vitesse du zoom
    [SerializeField] private float _zoom = 10f;

    [Header("Initial View Settings")]
    [SerializeField] private float _overviewSize = 7f;  // Taille initiale de l'orthographic size
    [SerializeField] private float _transitionDuration = 5f; // Durée de la transition vers le suivi

    private Transform _target;     // Cible actuelle suivie par la caméra
    private Camera _camera;        // Composant Camera
    private bool _isFollowing;     // Indique si la caméra suit une cible
    private bool _isZooming;       // Indique si la caméra effectue un zoom


    private void Start()
    {
        _camera = GetComponent<Camera>();

        // Garder la position initiale de la caméra définie dans l'éditeur
        _camera.orthographicSize = _overviewSize;
    }

    private void LateUpdate()
    {
        if (_isFollowing && _target != null)
        {
            // Suivre la cible uniquement sur l'axe Y
            Vector3 targetPosition = new Vector3(transform.position.x, _target.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, _followSpeed * Time.deltaTime);
        }

        if (_isZooming)
        {
            // Zoom progressif sur la cible
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _zoom, _zoomSpeed * Time.deltaTime);
        }

    }

    // Appelé lors du drop pour activer le suivi
    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
        _isFollowing = true;
        _isZooming = true;
    }

    // Revenir à la vue d'ensemble (appelée lors de StayStable)
    public void SetOverview()
    {
        _isFollowing = false;
        _isZooming = false;
        _camera.orthographicSize = _overviewSize;
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z); // Optional: ajuster la hauteur de la caméra
    }

}
