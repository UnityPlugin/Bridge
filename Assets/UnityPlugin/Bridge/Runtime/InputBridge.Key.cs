using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace UnityPlugin.Bridge
{
    public partial class InputBridge
    {
        public struct Key
        {
            KeyCode _key;
#if ENABLE_INPUT_SYSTEM
            KeyControl _keyCtrl;
#endif
            internal Key(KeyCode key)
            {
                _key = key;
#if ENABLE_INPUT_SYSTEM
                _keyCtrl = null;
#endif
            }

#if ENABLE_INPUT_SYSTEM
            internal Key(KeyCode key, KeyControl keyCtrl)
            {
                _key = key;
                _keyCtrl = keyCtrl;
            }
#endif

            public bool IsPressed()
            {
#if ENABLE_INPUT_SYSTEM
                if (_keyCtrl != null && _keyCtrl.isPressed) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKey(_key)) return true;
#endif
                return false;
            }

            public bool WasPressedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (_keyCtrl != null && _keyCtrl.wasPressedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyDown(_key)) return true;
#endif
                return false;
            }

            public bool WasReleasedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (_keyCtrl != null && _keyCtrl.wasReleasedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyUp(_key)) return true;
#endif
                return false;
            }
        }

        public static Key GetKeyCtrl(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM
            var keyboard = Keyboard.current;
            var path = key.ToString();
            var keyCtrl = keyboard[path.StartsWith("Digit") ? path.Substring(5) : path] as KeyControl;
            return new Key(key, keyCtrl);
#else
            return new Key(key);
#endif
        }
    }
}
