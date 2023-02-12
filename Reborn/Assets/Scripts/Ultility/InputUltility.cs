using System;
using UnityEngine;


namespace Reborn
{
    public static class InputUltility
    {
        public static int ToInt(this bool boolean)
        {
            return Convert.ToInt32(boolean);
        }
    }
}

