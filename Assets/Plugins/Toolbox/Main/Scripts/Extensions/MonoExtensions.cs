using UnityEngine;

namespace ToolBox
{
    public static class ObjectExtensions
    {
        public static T SpawnOn<T>(this T behaviour, Transform position) where T : Object
            => Object.Instantiate(behaviour, position.position, position.rotation);
        public static T SpawnOn<T>(this T behaviour, Vector3 position) where T : Object
            => Object.Instantiate(behaviour, position, Quaternion.identity);
    }
}