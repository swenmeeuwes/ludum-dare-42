using UnityEngine;
using Zenject;

[RequireComponent(typeof(PlayerMovement))]
public class Player : MonoBehaviour
{
    [SerializeField] private LookDirectionIndicator _lookDirectionIndicator;

    #region Injected

    private InputManager _inputManager;
    private PlayerMovement _movement; // todo: just use GetComponent<PlayerMovement>(); ?

    #endregion

    public Vector2 LookDirection { get; private set; }

    [Inject]
    private void Construct(InputManager inputManager, PlayerMovement movement)
    {
        _inputManager = inputManager;
        _movement = movement;
    }

    private void Update()
    {
        LookDirection = GetLookDirection();
        _lookDirectionIndicator.LookDirection = LookDirection;
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
}
