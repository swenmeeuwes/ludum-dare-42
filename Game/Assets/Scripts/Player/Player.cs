using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private LookDirectionIndicator _lookDirectionIndicator;

    #region Injected

    private InputManager _inputManager;
    private PlayerMovement _movement; // todo: just use GetComponent<PlayerMovement>(); ?
    private ControllerSettings _controllerSettings;
    private PlayerSettings _settings;
    private BulletManager _bulletManager;

    #endregion

    public Vector2 LookDirection { get; private set; }

    private float _lastShot;    

    [Inject]
    private void Construct(InputManager inputManager, PlayerMovement movement, ControllerSettings controllerSettings, 
        GameplaySettings gameplaySettings, BulletManager bulletManager)
    {
        _inputManager = inputManager;
        _movement = movement;
        _controllerSettings = controllerSettings;
        _settings = gameplaySettings.PlayerSettings;
        _bulletManager = bulletManager;
    }

    private void Update()
    {
        LookDirection = GetLookDirection();
        _lookDirectionIndicator.LookDirection = LookDirection;        

        if (Input.GetButton(InputAxes.Fire1) || Input.GetAxisRaw(InputAxes.Fire1) > _controllerSettings.DeadZone)
            Shoot();
    }

    private Vector2 GetLookDirection()
    {
        switch (_inputManager.CurrentInputType)
        {
            case InputType.Controller:
                return GetLookDirectionController();
            case InputType.KeyboardAndMouse:
            default:
                return GetLookDirectionMouse();
        }
    }

    private Vector2 GetLookDirectionController()
    {
        var xAxisRight = Input.GetAxisRaw(InputAxes.HorizontalRight);
        var yAxisRight = Input.GetAxisRaw(InputAxes.VerticalRight);
        var axesRight = new Vector2(xAxisRight, yAxisRight);

        if (axesRight.magnitude > 0.2f)
            return axesRight;
        return LookDirection;
    }

    private Vector2 GetLookDirectionMouse()
    {
        var mouseScreenPosition = Input.mousePosition;
        var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        var relativePositionToMouse = mouseWorldPosition - transform.position;
        return relativePositionToMouse.normalized;
    }

    private void Shoot()
    {
        if (Time.time - _lastShot < _settings.FireRate)
            return;

        _lastShot = Time.time;

        _bulletManager.Create(BulletOwner.Player, transform.position, LookDirection.normalized * _settings.BulletSpeed);
    }
}
