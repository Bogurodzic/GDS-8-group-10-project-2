using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuitWarning
{
    private static bool _visible;

    public static bool IsVisible()
    {
        return _visible;
    }

    public static void Hide()
    {
        _visible = false;
    }

    public static void Show()
    {
        _visible = true;
    }
}
