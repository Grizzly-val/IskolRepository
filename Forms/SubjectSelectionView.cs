using System.ComponentModel;
using IskolRepository.Core;
using IskolRepository.Core.Interfaces;
using IskolRepository.Models;

namespace IskolRepository.Forms;

public partial class SubjectSelectionView : UserControl
{
    public SubjectSelectionView()
    {
        InitializeComponent();
    }

    public event EventHandler? AddSubjectRequested;
    public event EventHandler? ChangeSemesterRequested;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SemesterName
    {
        get => semesterNameValueLabel.Text;
        set => semesterNameValueLabel.Text = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SemesterPath
    {
        get => semesterPathValueLabel.Text;
        set => semesterPathValueLabel.Text = value;
    }

    public void PopulateSubjects(Action<FlowLayoutPanel> loader)
    {
        subjectCardsPanel.Controls.Clear();
        loader(subjectCardsPanel);
    }

    private void addSubjectButton_Click(object? sender, EventArgs e)
        => AddSubjectRequested?.Invoke(this, EventArgs.Empty);

    private void changeSemesterButton_Click(object? sender, EventArgs e)
        => ChangeSemesterRequested?.Invoke(this, EventArgs.Empty);
}