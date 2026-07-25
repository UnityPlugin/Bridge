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
#endif
            }

#if ENABLE_INPUT_SYSTEM
            internal MouseButton(int index, ButtonControl mouseButton)
            {
                _index = index;
                _mouseButton = mouseButton;
            }
#endif

            public bool IsPressed()
            {
#if ENABLE_INPUT_SYSTEM
                if (_mouseButton != null && _mouseButton.isPressed) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetMouseButton(_index)) return true;
#endif
                return false;
            }

            public bool WasPressedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (_mouseButton != null && _mouseButton.wasPressedThisFrame) return true;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.GetMouseButtonDown(_index)) return true;
#endif
                return false;
            }

            public bool WasReleasedThisFrame()
            {
#if ENABLE_INPUT_SYSTEM
                if (_mouseButton != null && _mouseButton.wasReleasedThisFrame) return true;
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
            public MousePosition(Vector2Control pos)
            {
#if ENABLE_INPUT_SYSTEM
                _pos = pos;
#endif
            }

            public Vector2 Value()
            {
#if ENABLE_INPUT_SYSTEM
                if (_pos != null) return _pos.ReadValue();
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
            public MouseScroll(Vector2Control pos)
            {
#if ENABLE_INPUT_SYSTEM
                _scroll = pos;
#endif
            }

            public Vector2 Value()
            {
#if ENABLE_INPUT_SYSTEM
                if (_scroll != null) return _scroll.ReadValue();
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
                if (Input.mousePresent) return Input.mouseScrollDelta;
#endif
                return Vector2.zero;
            }
        }

        public static MousePosition GetMousePosition()
        {
#if ENABLE_INPUT_SYSTEM
            var mouse = Mouse.current;
            if (mouse != null)
            {
                return new MousePosition(mouse.position);
            }
#endif
            return new MousePosition();
        }

        public static MouseScroll GetMouseScroll()
        {
#if ENABLE_INPUT_SYSTEM
            var mouse = Mouse.current;
            if (mouse != null)
            {
                return new MouseScroll(mouse.scroll);
            }
#endif
            return new MouseScroll();
        }

        public static MouseButton GetMouseLeftButton() => GetMouseButton(0);
        public static MouseButton GetMouseRightButton() => GetMouseButton(1);
        public static MouseButton GetMouseMiddleButton() => GetMouseButton(2);
        public static MouseButton GetMouseForwardButton() => GetMouseButton(3);
        public static MouseButton GetMouseBackButton() => GetMouseButton(4);

        public static MouseButton GetMouseButton(int index)
        {
#if ENABLE_INPUT_SYSTEM
            var mouse = Mouse.current;
            if (mouse != null)
            {
                switch (index)
                {
                    case 0:
                        return new MouseButton(0, mouse.leftButton);
                    case 1:
                        return new MouseButton(1, mouse.rightButton);
                    case 2:
                        return new MouseButton(2, mouse.middleButton);
                    case 3:
                        return new MouseButton(2, mouse.forwardButton);
                    case 4:
                        return new MouseButton(2, mouse.backButton);
                }
            }
#endif
            return new MouseButton(index);
        }
    }
}
