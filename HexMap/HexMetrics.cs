using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mao{
    public static class HexMetrics
    {
        public const float innerRadius= 1.0f;
        public const float outerRadius = innerRadius/0.866025404f;

        public static Vector3[] corners =
        {
            new Vector3(0f,0f,outerRadius),
            new Vector3(innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(0f, 0f, -outerRadius),
            new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
            new Vector3(-innerRadius, 0f, 0.5f * outerRadius),
            new Vector3(0f,0f,outerRadius)
        };

    }

}
