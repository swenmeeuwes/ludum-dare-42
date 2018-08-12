using System;
using UnityEngine;
using UnityEngine.U2D;
using Zenject;

[Serializable]
public class PlayerMovementSettings
{
    public float MovementSpeed = 4f;
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region Injected

    private InputManager _inputManager;
    private float _joystickDeadZone;

    #endregion    

    [SerializeField] private PlayerMovementSettings _settings;
    public PlayerMovementSettings Settings {
        get { return _settings; }
    }

    private Rigidbody2D _rigidbody;

    private Vector2 _moveVelocity;

    [Inject]
    private void Construct(InputManager inputManager, ControllerSettings controllerSettings)
    {
        _inputManager = inputManager;
        _joystickDeadZone = controllerSettings.DeadZone;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var xAxis = Input.GetAxisRaw(InputAxes.Horizontal);
        var yAxis = Input.GetAxisRaw(InputAxes.Vertical);

        HandleMovement(xAxis, yAxis);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _moveVelocity;
    }

    private void HandleMovement(float xAxis, float yAxis)
    {
        var nextPosition = (Vector2)transform.position + new Vector2(xAxis, yAxis) * _settings.MovementSpeed * Time.deltaTime;
        if ((xAxis > _joystickDeadZone ||
            xAxis < -_joystickDeadZone ||
            yAxis > _joystickDeadZone ||
            yAxis < -_joystickDeadZone) && IsInBounds(nextPosition))
        {
            _moveVelocity = new Vector2(xAxis, yAxis) * _settings.MovementSpeed;
        }
        else
        {
            _moveVelocity = Vector2.zero;
        }
    }

    private bool IsInBounds(Vector2 position)
    {
        var width = PixelPerfectCameraUtil.Width;
        var height = PixelPerfectCameraUtil.Height;

        return position.x > -width / 2f + 0.5f &&
               position.x < width / 2f - 0.5f &&
               position.y > -height / 2f + 0.5f &&
               position.y < height / 2f - 0.5f;
    }
}
