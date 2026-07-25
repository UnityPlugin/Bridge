using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace UnityPlugin.Bridge
{
    public partial class InputBridge
    {
        public static void CheckDefaultInputModule(bool autoCreateSystem = true, bool inputSystemFirst = true)
        {
            if (EventSystem.current == null)
            {
                if (autoCreateSystem)
                {
                    var go = new GameObject("EventSystem");
                    go.AddComponent<EventSystem>();
                    if (EventSystem.current == null) return;
                }
                else
                {
                    return;
                }
            }

#if ENABLE_INPUT_SYSTEM
            var inputNew = true;
#else
            var inputNew = false;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            var inputOld = true;
#else
            var inputOld = false;
#endif

            if (inputNew && inputOld)
            {
                if (inputSystemFirst) inputOld = false;
                else inputNew = false;
            }

#if ENABLE_INPUT_SYSTEM
            if (inputNew) AddUIInputModule();
            else RemoveUIInputModule();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            if (inputOld) AddStandaloneInputModule();
            else RemoveStandaloneInputModule();
#endif
        }

        static void DestroyObject(Object obj)
        {
            if (Application.isPlaying) Object.Destroy(obj);
            else Object.DestroyImmediate(obj);
        }

#if ENABLE_INPUT_SYSTEM

        static void AddUIInputModule()
        {
            if (EventSystem.current == null) return;
            if (EventSystem.current.GetComponent<InputSystemUIInputModule>()) return;
            EventSystem.current.gameObject.AddComponent<InputSystemUIInputModule>();
        }

        static void RemoveUIInputModule()
        {
            if (EventSystem.current == null) return;
            var module = EventSystem.current.GetComponent<InputSystemUIInputModule>();
            if (module) DestroyObject(module);
        }

#endif

#if ENABLE_LEGACY_INPUT_MANAGER

        static void AddStandaloneInputModule()
        {
            if (EventSystem.current == null) return;
            if (EventSystem.current.GetComponent<StandaloneInputModule>()) return;
            EventSystem.current.gameObject.AddComponent<StandaloneInputModule>();
        }

        static void RemoveStandaloneInputModule()
        {
            if (EventSystem.current == null) return;
            var module = EventSystem.current.GetComponent<StandaloneInputModule>();
            if (module) DestroyObject(module);
        }

#endif
    }
}
