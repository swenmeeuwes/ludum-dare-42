using UnityEngine;

public class LookDirectionIndicator : MonoBehaviour
{
    private Vector2 _lookDirection;

    public Vector2 LookDirection
    {
        get { return _lookDirection; }
        set
        {
            _lookDirection = value;
            SetAngleAccordingToLookDirection(value);
        }
    }

    private float ComputeAngle(Vector2 direction)
    {
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    private void SetAngleAccordingToLookDirection(Vector2 direction)
    {
        transform.rotation = Quaternion.Euler(0, 0, ComputeAngle(direction) - 90);
    }
}
