using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace UnityPlugin.Bridge
{
    public partial class InputBridge
    {
        public struct TouchScreen
        {
            public int TouchCount()
            {
#if ENABLE_INPUT_SYSTEM
                var touchscreen = Touchscreen.current;
                if (touchscreen != null)
                {
                    var count = 0;
                    for (var i = 0; i < touchscreen.touches.Count; i++)
                    {
                        if (touchscreen.touches[i].isInProgress) count++;
                    }
                    return count;
                }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.touchSupported) return Input.touchCount;
#endif
                return 0;
            }
        }

        public static TouchScreen GetTouchScreen()
        {
            return new TouchScreen();
        }
    }
}
