using System;

namespace DBManager.AppHost.Models;

public class ThemeChangedMessage
{
    public string Theme { get; }

    public ThemeChangedMessage(string theme)
    {
        Theme = theme;
    }
}