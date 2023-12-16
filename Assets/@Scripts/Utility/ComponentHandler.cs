using UnityEngine;

namespace Scripts.Utility
{
    public static class ComponentHandler
    {
        public static T GetOrAdd<T>(GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();

            if (component == null) component = go.AddComponent<T>();

            return component;
        }
    }
}