using UnityEngine;

namespace ToolBox
{
    public static class Vector2Extensions
    {
        private static float Angle(this Vector2 src) => Mathf.Atan2(src.y, src.x);
    }
}

