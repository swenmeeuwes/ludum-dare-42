
using UnityEngine;
using UnityEngine.U2D;

public static class PixelPerfectCameraUtil
{
    public static PixelPerfectCamera PixelPerfectCamera { // todo: unsafe, but for a game jam... sure
        get { return Camera.main.GetComponent<PixelPerfectCamera>(); }
    }

    public static float Width {
        get { return (float)PixelPerfectCamera.refResolutionX / PixelPerfectCamera.assetsPPU; }
    }

    public static float Height {
        get { return (float)PixelPerfectCamera.refResolutionY / PixelPerfectCamera.assetsPPU; }
    }

    public static Rect Bounds {
        get
        {
            return new Rect(-Width / 2f,
                            -Height / 2f,
                            Width / 2f,
                            Height / 2f);
        }
    }

    public static bool IsInBounds(Vector2 position)
    {        
        return position.x > -Width / 2f &&
               position.x < Width / 2f &&
               position.y > -Height / 2f &&
               position.y < Height / 2f;
    }
}
