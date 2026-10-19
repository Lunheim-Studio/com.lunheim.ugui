using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    internal static class EventHelper
    {
        public static void SafeInvoke<T>(
            this Action<T> callbacks,
            T value) where T : UIBehaviour
        {
            if (callbacks == null)
                return;

            foreach (Action<T> callback in callbacks.GetInvocationList())
            {
                try
                {
                    callback(value);
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception, value);
                }
            }
        }
    }
}