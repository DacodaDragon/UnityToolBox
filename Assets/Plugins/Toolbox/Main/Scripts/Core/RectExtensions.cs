using UnityEngine;

namespace ToolBox
{
    public static class RectExtensions
    {
        public static Vector2 GetRandomPosition(this Rect src)
        {
            float x = Random.Range(src.xMin, src.xMax);
            float y = Random.Range(src.yMin, src.yMax);
            return new Vector2(x, y);
        }

        public static Rect AddY(this Rect src, float amount)
        {
            src.position = src.position + new Vector2(0, amount);
            return src;
        }

        public static Vector2 GetPosition(this Rect src, Vector2 normalizedVector)
        {
            float x = Mathf.LerpUnclamped(src.xMin, src.xMax, normalizedVector.x);
            float y = Mathf.LerpUnclamped(src.yMin, src.yMax, normalizedVector.y);
            return new Vector2(x, y);
        }
    }
}