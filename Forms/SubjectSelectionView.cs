using System.ComponentModel;
using IskolRepository.Core;
using IskolRepository.Core.Interfaces;
using IskolRepository.Models;
using IskolRepository.Utilities;

namespace IskolRepository.Forms;

public partial class SubjectSelectionView : UserControl
{
    public SubjectSelectionView()
    {
        InitializeComponent();
        AnimationHelper.AnimateHover(addSubjectButton, 8, 70);
        AnimationHelper.AnimateHover(changeSemesterButton, 8, 70);
        SetupButton(addSubjectButton);
        SetupButton(changeSemesterButton);
    }

    private void SetupButton(Button button)
    {
        button.EnabledChanged += (s, e) => UpdateButtonColor(button);
        UpdateButtonColor(button);
    }

    private void UpdateButtonColor(Button button)
    {
        if (button.Enabled)
        {
            button.BackColor = Color.FromArgb(43, 87, 158);
        }
        else
        {
            button.BackColor = Color.FromArgb(120, 11, 42, 92);
        }
    }

    public event EventHandler? AddSubjectRequested;
    public event EventHandler? ChangeSemesterRequested;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SemesterName
    {
        get => semesterNameValueLabel.Text;
        set => semesterNameValueLabel.Text = value;
    }

    public void PopulateSubjects(Action<FlowLayoutPanel> loader)
    {
        subjectCardsPanel.Controls.Clear();
        loader(subjectCardsPanel);

        var delay = 0;
        foreach (Control control in subjectCardsPanel.Controls)
        {
            control.Visible = false;
            AnimationHelper.AnimateControl(control, 200, delay);
            delay += 80;
        }
    }

    private void addSubjectButton_Click(object? sender, EventArgs e)
        => AddSubjectRequested?.Invoke(this, EventArgs.Empty);

    private void changeSemesterButton_Click(object? sender, EventArgs e)
        => ChangeSemesterRequested?.Invoke(this, EventArgs.Empty);

}