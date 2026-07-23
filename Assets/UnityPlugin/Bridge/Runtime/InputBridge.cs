using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityPlugin.Bridge
{
    public class InputBridge
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
            if (inputNew) InputSystemBridge.AddDefaultInputModule();
            else InputSystemBridge.RemoveDefaultInputModule();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
            var module = EventSystem.current.GetComponent<StandaloneInputModule>();
            if (inputOld)
            {
                if (module == null) EventSystem.current.gameObject.AddComponent<StandaloneInputModule>();
            }
            else
            {
                if (module)
                {
                    if (Application.isPlaying) Object.Destroy(module);
                    else Object.DestroyImmediate(module);
                }
            }
#endif
        }
    }
}
