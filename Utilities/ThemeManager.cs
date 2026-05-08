using System.Drawing;
using System.Windows.Forms;

namespace IskolRepository.Utilities;

public enum AppTheme { Dark, Light }

public static class ThemeManager
{
    public static AppTheme CurrentTheme { get; private set; } = AppTheme.Dark;

    public static Color FormBackColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(14, 15, 26)
            : Color.FromArgb(235, 238, 245);

    public static Color PanelBackColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(17, 18, 35)
            : Color.FromArgb(225, 228, 238);

    public static Color SecondaryPanelColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(12, 14, 24)
            : Color.FromArgb(210, 215, 230);

    public static Color TextColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.White
            : Color.Black;

    public static Color MutedTextColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(180, 190, 205)
            : Color.FromArgb(80, 85, 105);

    public static Color ButtonDefaultColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(24, 47, 83)
            : Color.FromArgb(60, 100, 175);       // visible blue in light mode

    public static Color ButtonDisabledColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(30, 36, 52)
            : Color.FromArgb(175, 180, 200);

    public static Color ButtonHoverColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(75, 143, 218)
            : Color.FromArgb(90, 140, 220);

    public static Color HeaderColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(10, 12, 20)
            : Color.FromArgb(45, 75, 140);        // deep blue so white logo stays readable

    public static Color WorkspaceColor =>
        CurrentTheme == AppTheme.Dark
            ? Color.FromArgb(12, 14, 24)
            : Color.FromArgb(220, 225, 238);

    public static void ToggleTheme()
    {
        CurrentTheme = CurrentTheme == AppTheme.Dark ? AppTheme.Light : AppTheme.Dark;
    }

    public static void ApplyTheme(Control parent)
    {
        ApplyControlTheme(parent);
        foreach (Control c in parent.Controls)
            ApplyTheme(c);
    }

    private static void ApplyControlTheme(Control control)
    {
        switch (control)
        {
            case Form form:
                form.BackColor = FormBackColor;
                form.ForeColor = TextColor;
                break;

            case Panel panel:
                panel.BackColor = PanelBackColor;
                panel.ForeColor = TextColor;
                break;

            case GroupBox gb:
                gb.BackColor = SecondaryPanelColor;
                gb.ForeColor = TextColor;
                break;

            case Label label:
                label.ForeColor = TextColor;
                if (label.BackColor != Color.Transparent)
                    label.BackColor = Color.Transparent;
                break;

            case Button btn:
                btn.BackColor = btn.Enabled ? ButtonDefaultColor : ButtonDisabledColor;
                btn.ForeColor = Color.White;          // always white on colored button
                btn.FlatAppearance.MouseOverBackColor = ButtonHoverColor;
                break;

            case TreeView tv:
                tv.BackColor = SecondaryPanelColor;
                tv.ForeColor = TextColor;
                break;

            case ListView lv:
                lv.BackColor = SecondaryPanelColor;
                lv.ForeColor = TextColor;
                break;

            case ListBox lb:
                lb.BackColor = SecondaryPanelColor;
                lb.ForeColor = TextColor;
                break;

            case ComboBox cb:
                cb.BackColor = SecondaryPanelColor;
                cb.ForeColor = TextColor;
                break;

            case DateTimePicker dtp:
                dtp.CalendarMonthBackground = SecondaryPanelColor;
                dtp.CalendarForeColor = TextColor;
                break;
        }
    }
}