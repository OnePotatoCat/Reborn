using System;
using UnityEngine;

namespace Reborn
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 MoveAxis => new Vector2(
            IsKey(InputSystem.InputMap.MoveRight).ToInt() - IsKey(InputSystem.InputMap.MoveLeft).ToInt(),
            IsKey(InputSystem.InputMap.MoveUp).ToInt() - IsKey(InputSystem.InputMap.MoveDown).ToInt()
            );

        public bool IsAttack => InputSystem.IsDown(InputSystem.InputMap.AttackAction);
        public bool IsSpecial => InputSystem.IsDown(InputSystem. InputMap.SpecialAction);
        private bool IsKey(InputSystem.InputMap action) => InputSystem.IsKey(action);
    }
}