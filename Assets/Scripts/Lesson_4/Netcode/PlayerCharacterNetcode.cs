using UnityEngine;
public class PlayerCharacterNetcode : CharacterNetcode
{
    [SerializeField] [Range(0, 100)] private int health = 100;
    [SerializeField] [Range(0.5f, 10.0f)] private float movingSpeed = 8.0f;
    [SerializeField] private float acceleration = 3.0f;

    private const float gravity = -9.8f;
    private CharacterController _characterController;
    private MouseLookNetcode _mouseLook;
    private Vector3 _currentVelocity;
    private bool _hasAuthority;

    protected override FireActionNetcode fireAction { get; set; }

    protected override void Initiate()
    {
        base.Initiate();
        fireAction = gameObject.AddComponent<RayShooterNetcode>();
        fireAction.Reloading();
        _characterController = GetComponentInChildren<CharacterController>();
        _characterController ??= gameObject.AddComponent<CharacterController>();
        _mouseLook = GetComponentInChildren<MouseLookNetcode>();
        _mouseLook ??= gameObject.AddComponent<MouseLookNetcode>();
    }
    public override void Movement()
    {
        if (_mouseLook != null && _mouseLook.PlayerCamera != null)
        {
            _mouseLook.PlayerCamera.enabled = _hasAuthority;
        }
        if (_hasAuthority)
        {
            var moveX = Input.GetAxis("Horizontal") * movingSpeed;
            var moveZ = Input.GetAxis("Vertical") * movingSpeed;
            var movement = new Vector3(moveX, 0, moveZ);
            movement = Vector3.ClampMagnitude(movement, movingSpeed);
            movement *= Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                movement *= acceleration;
            }
            movement.y = gravity;
            movement = transform.TransformDirection(movement);
            _characterController.Move(movement);
            _mouseLook.Rotation();
            CmdUpdatePositionServerRpc(transform.position);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, serverPosition.Value, ref _currentVelocity, movingSpeed * Time.deltaTime);
        }
    }
    private void Start()
    {
        Initiate();
    }
    private void OnGUI()
    {
        if (Camera.main == null)
        {
            return;
        }
        var info = $"Health: {health}\nClip: {fireAction}";
        var size = 12;
        var bulletCountSize = 50;
        var posX = Camera.main.pixelWidth / 2 - size / 4;
        var posY = Camera.main.pixelHeight / 2 - size / 2;
        var posXBul = Camera.main.pixelWidth - bulletCountSize * 2;
        var posYBul = Camera.main.pixelHeight - bulletCountSize;
        GUI.Label(new Rect(posX, posY, size, size), "+");
        GUI.Label(new Rect(posXBul, posYBul, bulletCountSize * 2,
        bulletCountSize * 2), info);
    }
}