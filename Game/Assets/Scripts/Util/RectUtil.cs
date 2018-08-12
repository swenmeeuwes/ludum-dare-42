using UnityEngine;

public static class RectUtil
{
    public static Rect Expand(this Rect rect, float amount)
    {
        return new Rect(rect.x - amount, rect.y - amount, 
            rect.width + amount, rect.height + amount);
    }
}
