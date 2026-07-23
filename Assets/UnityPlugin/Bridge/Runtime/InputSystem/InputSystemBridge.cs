#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace UnityPlugin.Bridge
{
    public class InputSystemBridge
    {
        public static void AddDefaultInputModule()
        {
            if (EventSystem.current == null) return;

            if (EventSystem.current.GetComponent<InputSystemUIInputModule>())
            {
                return;
            }

            EventSystem.current.gameObject.AddComponent<InputSystemUIInputModule>();
        }

        public static void RemoveDefaultInputModule()
        {
            if (EventSystem.current == null) return;

            var module = EventSystem.current.GetComponent<InputSystemUIInputModule>();

            if (module)
            {
                if (Application.isPlaying) Object.Destroy(module);
                else Object.DestroyImmediate(module);
            }
        }
    }
}
#endif
