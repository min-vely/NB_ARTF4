using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scripts.UI
{
    public class Binder
    {
        public void Binding<T>(GameObject parentObject, Type type, Dictionary<Type, Object[]> objectDictionary) where T : Object
        {
            string[] names = Enum.GetNames(type);
            Object[] objects = new Object[names.Length];
            objectDictionary.Add(typeof(T), objects);
            AssignmentComponent<T>(parentObject, names, objects);
        }

        private void AssignmentComponent<T>(GameObject parentObject, IReadOnlyList<string> names, IList<Object> objects) where T : Object
        {
            for (int i = 0; i < names.Count; i++)
            {
                objects[i] = FindComponent<T>(parentObject, names[i], true);
                if (objects[i] == null) Debug.Log($"바인드 실패 : {names[i]}");
            }
        }

        private T FindComponent<T>(GameObject parentObject, string name, bool recursive) where T : Object
        {
            if (parentObject == null) return null;

            return recursive 
                ? FindComponentRecursive<T>(parentObject, name)
                : FindComponentNonRecursive<T>(parentObject, name);
        }

        private static T FindComponentNonRecursive<T>(GameObject parentObject, string name) where T : Object
        {
            for (int i = 0; i < parentObject.transform.childCount; i++)
            {
                Transform child = parentObject.transform.GetChild(i);
                if (!string.IsNullOrEmpty(name) && child.name != name) continue;
                T component = child.GetComponent<T>();
                if (component != null) return component;
            }
            return null;
        }

        private static T FindComponentRecursive<T>(GameObject parentObject, string name) where T : Object
        {
            return parentObject.GetComponentsInChildren<T>()
                .FirstOrDefault(component => string.IsNullOrEmpty(name) || component.name == name);
        }

        public T Getter<T>(int componentIndex, Dictionary<Type, Object[]> objectsDictionary) where T : Object
        {
            if (!objectsDictionary.TryGetValue(typeof(T), out Object[] objects)) return null;
            return objects[componentIndex] as T;
        }
    }
}