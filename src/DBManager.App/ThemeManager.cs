using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;

namespace DBManager.App
{
    public class ThemeManager
    {
        private readonly Application _application;
        private readonly Dictionary<string, IResourceDictionary> _themes;
        private string _currentTheme = "Light";

        public ThemeManager(Application application)
        {
            _application = application;
            _themes = new Dictionary<string, IResourceDictionary>();
            
            // 加载主题资源
            var resourceDictionary = AvaloniaXamlLoader.Load(new Uri("avares://DBManager.App/Styles/Themes.axaml")) as ResourceDictionary;
            
            if (resourceDictionary != null)
            {
                // 修正：使用正确的TryGetResource方法签名，添加ThemeVariant参数
                object? lightRes = null;
                object? darkRes = null;
                
                // 使用默认主题变体（null）
                if (resourceDictionary.TryGetResource("LightTheme", null, out lightRes) && lightRes is ResourceDictionary lightDict)
                {
                    _themes["Light"] = lightDict;
                }
                
                if (resourceDictionary.TryGetResource("DarkTheme", null, out darkRes) && darkRes is ResourceDictionary darkDict)
                {
                    _themes["Dark"] = darkDict;
                }
            }
            
            // 默认使用浅色主题
            SetTheme("Light");
        }

        public void SetTheme(string themeName)
        {
            if (!_themes.ContainsKey(themeName))
                return;
            
            // 移除当前主题
            if (_themes.ContainsKey(_currentTheme))
            {
                _application.Resources.MergedDictionaries.Remove(_themes[_currentTheme]);
            }
            
            // 应用新主题
            _application.Resources.MergedDictionaries.Add(_themes[themeName]);
            _currentTheme = themeName;
        }

        public string CurrentTheme => _currentTheme;
    }
}
