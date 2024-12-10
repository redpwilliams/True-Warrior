using System;

namespace Util
{
    [Flags]
    public enum PlayerInputFlags
    {
        None   = 0,
        Scroll = 1,
        Select = 2,
        Attack = 4
    }
}