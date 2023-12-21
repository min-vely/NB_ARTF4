using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.Event.UI
{
    public static class UI_SetEvent
    {
        public static void SetEvent(this GameObject gameObject, UIEventType eventType, Action<PointerEventData> action)
        {
            var handler = gameObject.GetOrAddComponent<UI_EventHandler>();
            handler.BindEvent(eventType, action);
        }
    }
}