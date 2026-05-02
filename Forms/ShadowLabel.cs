using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IskolRepository.Forms;

public class ShadowLabel : Label
{
    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color ShadowColor { get; set; } = Color.FromArgb(120, 0, 0, 0);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Point ShadowOffset { get; set; } = new Point(2, 2);

    [Browsable(false)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool UseShadow { get; set; } = true;

    protected override void OnPaint(PaintEventArgs e)
    {
        if (UseShadow && !string.IsNullOrEmpty(Text))
        {
            var shadowRect = ClientRectangle;
            shadowRect.Offset(ShadowOffset);
            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.WordBreak;
            TextRenderer.DrawText(e.Graphics, Text, Font, shadowRect, ShadowColor, flags);
        }

        base.OnPaint(e);
    }
}
