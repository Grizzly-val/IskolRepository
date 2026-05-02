namespace IskolRepository.Forms;

public partial class StartupView : UserControl
{
    public StartupView()
    {
        InitializeComponent();
    }

    public event EventHandler? OpenSemesterRequested;
    public event EventHandler? NewSemesterRequested;

    private void openSemesterButton_Click(object? sender, EventArgs e)
        => OpenSemesterRequested?.Invoke(this, EventArgs.Empty);

    private void newSemesterButton_Click(object? sender, EventArgs e)
        => NewSemesterRequested?.Invoke(this, EventArgs.Empty);
}
