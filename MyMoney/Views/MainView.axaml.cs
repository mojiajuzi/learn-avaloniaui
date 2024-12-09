using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Projektanker.Icons.Avalonia;

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
        var themeIcon = this.FindControl<Icon>("IconToggle");
        var ThemeText = this.FindControl<TextBlock>("ThemeText");
        if (themeIcon != null)
        {
            themeIcon.Value = _isDarkTheme ? "fa-thin fa-moon" : "fa-thin fa-sun";
            ThemeText.Text = _isDarkTheme ? "Dark" : "Light";
        }
    }
}