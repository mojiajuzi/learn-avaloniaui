using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;

namespace MyMoney.Views;

public partial class MainView : UserControl
{
    private bool _isDarkTheme = false;

    public MainView()
    {
        InitializeComponent();
        InitializeTheme();
    }

    private void InitializeTheme()
    {
        var app = Application.Current;
        if (app != null)
        {
            // 获取当前系统主题或保存的主题设置
            var currentTheme = app.ActualThemeVariant;
            _isDarkTheme = currentTheme == ThemeVariant.Dark;
            
            // 设置初始主题
            app.RequestedThemeVariant = _isDarkTheme ? ThemeVariant.Dark : ThemeVariant.Light;
            
            // 更新UI状态
            var toggleButton = this.FindControl<ToggleButton>("ThemeToggleButton");
            if (toggleButton != null)
            {
                toggleButton.IsChecked = _isDarkTheme;
            }
            UpdateThemeIcon();
        }
    }

    private void ThemeToggleButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton toggleButton)
        {
            SetTheme(toggleButton.IsChecked ?? false);
        }
    }

    private void SetTheme(bool isDark)
    {
        _isDarkTheme = isDark;
        var app = Application.Current;
        if (app != null)
        {
            app.RequestedThemeVariant = isDark ? ThemeVariant.Dark : ThemeVariant.Light;
        }
        UpdateThemeIcon();
    }

    private void UpdateThemeIcon()
    {
        var themeIcon = this.FindControl<PathIcon>("ThemeIcon");
        var themeText = this.FindControl<TextBlock>("ThemeText");
        
        if (themeIcon != null)
        {
            // 月亮图标（暗色主题）
            string moonIcon = "M12 3c-4.97 0-9 4.03-9 9s4.03 9 9 9 9-4.03 9-9c0-.46-.04-.92-.1-1.36-.98 1.37-2.58 2.26-4.4 2.26-3.03 0-5.5-2.47-5.5-5.5 0-1.82.89-3.42 2.26-4.4-.44-.06-.9-.1-1.36-.1z";
            // 太阳图标（亮色主题）
            string sunIcon = "M12 7c-2.76 0-5 2.24-5 5s2.24 5 5 5 5-2.24 5-5-2.24-5-5-5zM2 13h2c.55 0 1-.45 1-1s-.45-1-1-1H2c-.55 0-1 .45-1 1s.45 1 1 1zm18 0h2c.55 0 1-.45 1-1s-.45-1-1-1h-2c-.55 0-1 .45-1 1s.45 1 1 1zM11 2v2c0 .55.45 1 1 1s1-.45 1-1V2c0-.55-.45-1-1-1s-1 .45-1 1zm0 18v2c0 .55.45 1 1 1s1-.45 1-1v-2c0-.55-.45-1-1-1s-1 .45-1 1zM5.99 4.58c-.39-.39-1.03-.39-1.41 0-.39.39-.39 1.03 0 1.41l1.06 1.06c.39.39 1.03.39 1.41 0s.39-1.03 0-1.41L5.99 4.58zm12.37 12.37c-.39-.39-1.03-.39-1.41 0-.39.39-.39 1.03 0 1.41l1.06 1.06c.39.39 1.03.39 1.41 0 .39-.39.39-1.03 0-1.41l-1.06-1.06zm1.06-10.96c.39-.39.39-1.03 0-1.41-.39-.39-1.03-.39-1.41 0l-1.06 1.06c-.39.39-.39 1.03 0 1.41s1.03.39 1.41 0l1.06-1.06zM7.05 18.36c.39-.39.39-1.03 0-1.41-.39-.39-1.03-.39-1.41 0l-1.06 1.06c-.39.39-.39 1.03 0 1.41s1.03.39 1.41 0l1.06-1.06z";

            themeIcon.Data = Geometry.Parse(_isDarkTheme ? sunIcon : moonIcon);
        }

        if (themeText != null)
        {
            themeText.Text = _isDarkTheme ? "Dark" : "Light";
        }
    }
}