using UnityEngine;

public class Fog : MonoBehaviour
{
    [SerializeField] private Vector2 _offset; // todo: maybe we could use spriteRender.size to determine the offset, but we would need a direction

    public void SetPosition(Vector2 position)
    {
        transform.position = position + _offset;
    }
}
