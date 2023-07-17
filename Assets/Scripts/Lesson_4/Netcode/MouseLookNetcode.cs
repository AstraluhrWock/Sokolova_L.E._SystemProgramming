using Unity.Netcode;
using UnityEngine;

public class MouseLookNetcode : NetworkBehaviour
{
    [SerializeField] [Range(0.1f, 10.0f)] private float _sensitivity = 2.0f;
    [SerializeField] [Range(-90.0f, .0f)] private float _minVert = -45.0f;
    [SerializeField] [Range(0.0f, 90.0f)] private float _maxVert = 45.0f;
    private Camera _camera;
    private Rigidbody _rigidbody;
    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;
    
    public Camera PlayerCamera => _camera;

    private void Start()
    {
        _camera = GetComponentInChildren<Camera>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
        if (_rigidbody != null)
            _rigidbody.freezeRotation = true;
    }
    public void Rotation()
    {
        _rotationX -= Input.GetAxis("Mouse Y") * _sensitivity;
        _rotationY += Input.GetAxis("Mouse X") * _sensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _minVert, _maxVert);
        transform.rotation = Quaternion.Euler(0, _rotationY, 0);
        _camera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    }
}