using System;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace OcUtility
{
    public class ShortcutListener : MonoBehaviour
    {
        void Awake()
        {
#if DEBUG
            DontDestroyOnLoad(gameObject);
#else
            Destroy(gameObject);
#endif
        }

        void Update()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current.altKey.isPressed)
            {
                
                if(Keyboard.current.leftBracketKey.wasPressedThisFrame)
                    if(Keyboard.current.leftCtrlKey.isPressed) Printer.ClearLogs();
                    else Printer.PrintDivider();
                if(Keyboard.current.rightBracketKey.wasPressedThisFrame) Printer.PrintColorDivider();
            }
#endif
        }
    }
}