using System;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace UnityPlugin.Bridge
{
    public partial class InputBridge
    {
        public struct KeyButton
        {
            KeyCode _key;
#if ENABLE_INPUT_SYSTEM
            string _keyPath;
            KeyControl _keyCtrl;
#endif
            internal KeyButton(KeyCode key)
            {
                _key = key;
#if ENABLE_INPUT_SYSTEM
                _keyPath = null;
                _keyCtrl = null;
#endif
            }

#if ENABLE_INPUT_SYSTEM
            internal KeyButton(KeyCode key, string keyPath)
            {
                _key = key;
                _keyPath = keyPath;
                _keyCtrl = null;
                GetKeyControl(ref _keyCtrl, _keyPath);
            }
#endif

            public bool IsPressed()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetKeyControl(ref _keyCtrl, _keyPath) && _keyCtrl.isPressed) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKey(_key)) return true;
#endif
                return false;
            }

            public bool WasPressedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetKeyControl(ref _keyCtrl, _keyPath) && _keyCtrl.wasPressedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyDown(_key)) return true;
#endif
                return false;
            }

            public bool WasReleasedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetKeyControl(ref _keyCtrl, _keyPath) && _keyCtrl.wasReleasedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetKeyUp(_key)) return true;
#endif
                return false;
            }
        }

        public static KeyButton GetKeyButton(KeyCode key)
        {
#if ENABLE_INPUT_SYSTEM
            var path = key.ToString().ToLower();
            if (path.StartsWith("digit")) path = path.Substring(5);
            else if (path.Contains("control")) path = path.Replace("control", "ctrl");
            return new KeyButton(key, path);
#else
            return new KeyButton(key);
#endif
        }

#if ENABLE_INPUT_SYSTEM
        static bool GetKeyControl(ref KeyControl keyCtrl, string path)
        {
            if (keyCtrl != null && IsInputControlAvailable(keyCtrl)) return true;

            keyCtrl = null;
            if (string.IsNullOrEmpty(path)) return false;

            var keyboard = Keyboard.current;
            if (keyboard != null)
            {
                try
                {
                    keyCtrl = keyboard.GetChildControl<KeyControl>(path);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
            return keyCtrl != null;
        }
#endif
    }
}
