using System;

namespace LordJZ.WinAPI
{
    [Flags]
    public enum WindowPlacementFlags
    {
        AsyncWindowPlacement = 4,
        RestoreToMaximized = 2,
        SetMinPosition = 1,
        None = 0
    }
}
