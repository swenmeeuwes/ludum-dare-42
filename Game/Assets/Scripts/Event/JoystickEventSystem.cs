using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class JoystickEventSystem : EventSystem
{
    #region Injected

    private float _deadZone;

    #endregion

    [Inject]
    private void Construct(ControllerSettings controllerSettings)
    {
        _deadZone = controllerSettings.DeadZone;
    }

    protected override void Start()
    {
        base.Start();

        if (firstSelectedGameObject == null)
        {
            var firstButtonGameObject = FindObjectOfType<Button>().gameObject;
            if (firstButtonGameObject != null)
                firstSelectedGameObject = firstButtonGameObject;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (currentSelectedGameObject == null && (
            Input.GetAxisRaw(InputAxes.Horizontal) > _deadZone ||
            Input.GetAxisRaw(InputAxes.Horizontal) < -_deadZone ||
            Input.GetAxisRaw(InputAxes.Vertical) > _deadZone ||
            Input.GetAxisRaw(InputAxes.Vertical) < -_deadZone))
        {
            SetSelectedGameObject(firstSelectedGameObject);
        }
    }
}
