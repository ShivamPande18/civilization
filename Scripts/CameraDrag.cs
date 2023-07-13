using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{

    Vector3 _origin;
    Vector3 _difference;

    Camera _mainCamera;

    bool _isDragging;


    private void Awake() => _mainCamera = Camera.main;

    public void OnDrag(InputAction.CallbackContext ctx)
    {
        if (ctx.started) _origin = GetMousePosition;
        _isDragging = ctx.started || ctx.performed;
    }

    private void LateUpdate()
    {
        if (!_isDragging) return;

        _difference = GetMousePosition - transform.position;
        transform.position = _origin - _difference;
    }

    private Vector3 GetMousePosition => _mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}