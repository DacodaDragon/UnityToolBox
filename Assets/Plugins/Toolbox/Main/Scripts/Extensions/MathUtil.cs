using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToolBox
{
    public static class MathUtil
    {
        public static float ScrollbarWidth(Rect area, Rect viewport, float scrollbarWidth)
        {
            float diffa = area.xMin - viewport.xMin;
            float diffb = area.xMax - viewport.xMax;
            float delta = diffa - diffb;

            return Math.Max(5, (area.width / delta) * scrollbarWidth);
        }

        public static Vector2 Vec2Bezier(Vector2 start, Vector2 handle1, Vector2 handle2, Vector2 end, float t)
        {
            float s = 1 - t;

            Vector2 AB = start * s + handle1 * t;
            Vector2 BC = handle1 * s + handle2 * t;
            Vector2 CD = handle2 * s + end * t;

            Vector2 ABC = AB * s + BC * t;
            Vector2 BCD = BC * s + CD * t;
            return ABC * s + BCD * t;
        }

        // blatently stolen from https://gamedev.stackexchange.com/questions/154068/calculating-the-geometry-of-a-thick-3-way-miter-joint
        public static void PolyLineToTriangleStrip(Vector2[] pts, ref Vector2[] poly, float thickness)
        {
            var numPts = pts.Length;
            int k = 0;

            for (int i = 0; i < numPts; ++i)
            {
                int a = ((i - 1) < 0) ? 0 : (i - 1);
                int b = i;
                int c = ((i + 1) >= numPts) ? numPts - 1 : (i + 1);
                int d = ((i + 2) >= numPts) ? numPts - 1 : (i + 2);
                var p0 = pts[a];
                var p1 = pts[b];
                var p2 = pts[c];
                var p3 = pts[d];

                if (p1 == p2)
                    continue;

                // 1) define the line between the two points
                var line = (p2 - p1).normalized;

                // 2) find the normal vector of this line
                var normal = new Vector2(-line.y, line.x).normalized;

                // 3) find the tangent vector at both the end points:
                //      -if there are no segments before or after this one, use the line itself
                //      -otherwise, add the two normalized lines and average them by normalizing again
                var tangent1 = (p0 == p1) ? line : ((p1 - p0).normalized + line).normalized;
                var tangent2 = (p2 == p3) ? line : ((p3 - p2).normalized + line).normalized;

                // 4) find the miter line, which is the normal of the tangent
                var miter1 = new Vector2(-tangent1.y, tangent1.x);
                var miter2 = new Vector2(-tangent2.y, tangent2.x);

                // find length of miter by projecting the miter onto the normal,
                // take the length of the projection, invert it and multiply it by the thickness:
                //      length = thickness * ( 1 / |normal|.|miter| )
                float length1 = Math.Max(thickness, thickness / Vector2.Dot(normal, miter1));
                float length2 = Math.Min(-thickness, thickness / Vector2.Dot(normal, miter2));

                if (i == 0)
                {
                    poly[k++] = (p1 + length1 * miter1);
                    poly[k++] = (p1 - length1 * miter1);
                }

                poly[k++] = (p2 - length2 * miter2);
                poly[k++] = (p2 + length2 * miter2);
            }
        }

    }
}
