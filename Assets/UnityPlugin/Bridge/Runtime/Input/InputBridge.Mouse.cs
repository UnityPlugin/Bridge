using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
#endif

namespace UnityPlugin.Bridge
{
    public partial class InputBridge
    {
        public struct MouseButton
        {
            int _index;
#if ENABLE_INPUT_SYSTEM
            ButtonControl _mouseButton;
#endif
            internal MouseButton(int index)
            {
                _index = index;
#if ENABLE_INPUT_SYSTEM
                _mouseButton = null;
                GetMouseButtonControl(ref _mouseButton, _index);
#endif
            }

            public bool IsPressed()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetMouseButtonControl(ref _mouseButton, _index) && _mouseButton.isPressed) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetMouseButton(_index)) return true;
#endif
                return false;
            }

            public bool WasPressedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetMouseButtonControl(ref _mouseButton, _index) && _mouseButton.wasPressedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetMouseButtonDown(_index)) return true;
#endif
                return false;
            }

            public bool WasReleasedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetMouseButtonControl(ref _mouseButton, _index) && _mouseButton.wasReleasedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetMouseButtonUp(_index)) return true;
#endif
                return false;
            }
        }

        public struct MousePosition
        {
#if ENABLE_INPUT_SYSTEM
            Vector2Control _pos;
#endif
            public Vector2 Value()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetMousePositionControl(ref _pos)) return _pos.ReadValue();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.mousePresent) return Input.mousePosition;
#endif
                return Vector2.zero;
            }
        }

        public struct MouseScroll
        {
#if ENABLE_INPUT_SYSTEM
            Vector2Control _scroll;
#endif
            public Vector2 Value()
            {
#if ENABLE_INPUT_SYSTEM
                if (GetMousePositionControl(ref _scroll)) return _scroll.ReadValue();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.mousePresent) return Input.mouseScrollDelta;
#endif
                return Vector2.zero;
            }
        }

        public static MousePosition GetMousePosition()
        {
            return new MousePosition();
        }

        public static MouseScroll GetMouseScroll()
        {
            return new MouseScroll();
        }

        public static MouseButton GetMouseLeftButton() => GetMouseButton(0);
        public static MouseButton GetMouseRightButton() => GetMouseButton(1);
        public static MouseButton GetMouseMiddleButton() => GetMouseButton(2);
        public static MouseButton GetMouseForwardButton() => GetMouseButton(3);
        public static MouseButton GetMouseBackButton() => GetMouseButton(4);

        public static MouseButton GetMouseButton(int index)
        {
            return new MouseButton(index);
        }

#if ENABLE_INPUT_SYSTEM

        static bool GetMouseButtonControl(ref ButtonControl mouseCtrl, int index)
        {
            if (mouseCtrl != null && IsInputControlAvailable(mouseCtrl)) return true;

            mouseCtrl = null;
            var mouse = Mouse.current;
            if (mouse != null)
            {
                switch (index)
                {
                    case 0:
                        mouseCtrl = mouse.leftButton;
                        break;
                    case 1:
                        mouseCtrl = mouse.rightButton;
                        break;
                    case 2:
                        mouseCtrl = mouse.rightButton;
                        break;
                    case 3:
                        mouseCtrl = mouse.rightButton;
                        break;
                    case 4:
                        mouseCtrl = mouse.rightButton;
                        break;
                }

            }
            return mouseCtrl != null;
        }

        static bool GetMousePositionControl(ref Vector2Control mouseCtrl)
        {
            if (mouseCtrl != null && IsInputControlAvailable(mouseCtrl)) return true;

            mouseCtrl = null;
            var mouse = Mouse.current;
            if (mouse != null)
            {
                mouseCtrl = mouse.position;
            }

            return mouseCtrl != null;
        }

        static bool GetMouseScrollControl(ref Vector2Control mouseCtrl)
        {
            if (mouseCtrl != null && IsInputControlAvailable(mouseCtrl)) return true;

            mouseCtrl = null;
            var mouse = Mouse.current;
            if (mouse != null)
            {
                mouseCtrl = mouse.scroll;
            }

            return mouseCtrl != null;
        }
#endif
    }
}
