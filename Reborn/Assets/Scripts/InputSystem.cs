using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reborn
{
    [DefaultExecutionOrder(DefaultExecutionOrder)]
    public class InputSystem : MonoBehaviour
    {
        private const int DefaultExecutionOrder = -1000;

        public enum InputMap
        {
            MoveUp,
            MoveDown,
            MoveLeft,
            MoveRight,
            AttackAction,
            SpecialAction,
        }

        [Serializable]
        public class InputBinding
        {
            public InputMap Action;
            public KeyCode Keyboard;
            public KeyCode Gamepad;
        }

        [SerializeField] private InputBinding[] _InputBindings;
        private Dictionary<InputMap, InputBinding> _InputMapDict;
        private Dictionary<InputMap, InputBinding> InputMapDict
        {
            get
            {
                if (_InputMapDict == null)
                {
                    _InputMapDict = new Dictionary<InputMap, InputBinding>();
                    foreach (var binding in _InputBindings)
                    {
                        _InputMapDict.Add(binding.Action, binding);
                    }
                }
                return _InputMapDict;
            }

            set => _InputMapDict = value;
        }

        private Dictionary<InputMap, InputBinding> GetDictType()
        {
            return InputMapDict;
        }


        private static InputSystem Singleton;

        private void Awake()
        {
            if (Singleton != null)
            {
                Destroy(gameObject);
                return;
            }

            Singleton = this;
            InputMapDict = InputMapDict;
        }
    
        public static bool IsDown(InputMap action)
        {
            var binding = Singleton.GetDictType()[action];
            return Input.GetKeyDown(binding.Keyboard) ||
                   Input.GetKeyDown(binding.Gamepad);
        }

        public static bool IsUp(InputMap action)
        {
            var binding = Singleton.GetDictType()[action];
            return Input.GetKeyUp(binding.Keyboard) ||
                   Input.GetKeyUp(binding.Gamepad); ;
        }

        public static bool IsKey(InputMap action)
        {
            var binding = Singleton.GetDictType()[action];
            return Input.GetKey(binding.Keyboard) ||
                   Input.GetKey(binding.Gamepad);
        }
    }

}
