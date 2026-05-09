namespace IskolRepository.Forms;

partial class StartupView
{
    private System.ComponentModel.IContainer components = null;

    private TableLayoutPanel rootLayout = null!;
    private TableLayoutPanel contentLayout = null!;
    private FlowLayoutPanel buttonLayout = null!;
    private PictureBox logoMarkPictureBox = null!;
    private Label appNameLabel = null!;
    private Label taglineLabel = null!;
    private Button openSemesterButton = null!;
    private Button newSemesterButton = null!;
    private Label orLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartupView));
        rootLayout = new TableLayoutPanel();
        contentLayout = new TableLayoutPanel();
        logoMarkPictureBox = new PictureBox();
        appNameLabel = new Label();
        taglineLabel = new Label();
        buttonLayout = new FlowLayoutPanel();
        openSemesterButton = new Button();
        orLabel = new Label();
        newSemesterButton = new Button();
        rootLayout.SuspendLayout();
        contentLayout.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)logoMarkPictureBox).BeginInit();
        buttonLayout.SuspendLayout();
        SuspendLayout();
        // 
        // rootLayout
        // 
        rootLayout.BackColor = Color.Transparent;
        rootLayout.ColumnCount = 3;
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        rootLayout.ColumnStyles.Add(new ColumnStyle());
        rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        rootLayout.Controls.Add(contentLayout, 1, 1);
        rootLayout.Dock = DockStyle.Fill;
        rootLayout.Location = new Point(0, 0);
        rootLayout.Name = "rootLayout";
        rootLayout.RowCount = 3;
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        rootLayout.RowStyles.Add(new RowStyle());
        rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        rootLayout.Size = new Size(800, 600);
        rootLayout.TabIndex = 0;
        // 
        // contentLayout
        // 
        contentLayout.AutoSize = true;
        contentLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        contentLayout.BackColor = Color.Transparent;
        contentLayout.ColumnCount = 1;
        contentLayout.ColumnStyles.Add(new ColumnStyle());
        contentLayout.Controls.Add(logoMarkPictureBox, 0, 0);
        contentLayout.Controls.Add(appNameLabel, 0, 1);
        contentLayout.Controls.Add(taglineLabel, 0, 2);
        contentLayout.Controls.Add(buttonLayout, 0, 3);
        contentLayout.Location = new Point(170, 149);
        contentLayout.Name = "contentLayout";
        contentLayout.RowCount = 4;
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.RowStyles.Add(new RowStyle());
        contentLayout.Size = new Size(460, 302);
        contentLayout.TabIndex = 0;
        // 
        // logoMarkPictureBox
        // 
        logoMarkPictureBox.Anchor = AnchorStyles.None;
        logoMarkPictureBox.BackColor = Color.Transparent;
        logoMarkPictureBox.Image = (Image)resources.GetObject("logoMarkPictureBox.Image");
        logoMarkPictureBox.Location = new Point(166, 0);
        logoMarkPictureBox.Margin = new Padding(0, 0, 0, 16);
        logoMarkPictureBox.Name = "logoMarkPictureBox";
        logoMarkPictureBox.Size = new Size(128, 128);
        logoMarkPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        logoMarkPictureBox.TabIndex = 0;
        logoMarkPictureBox.TabStop = false;
        // 
        // appNameLabel
        // 
        appNameLabel.Anchor = AnchorStyles.None;
        appNameLabel.AutoSize = true;
        appNameLabel.BackColor = Color.Transparent;
        appNameLabel.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        appNameLabel.ForeColor = Color.White;
        appNameLabel.Location = new Point(131, 144);
        appNameLabel.Margin = new Padding(0, 0, 0, 8);
        appNameLabel.Name = "appNameLabel";
        appNameLabel.Size = new Size(197, 51);
        appNameLabel.TabIndex = 1;
        appNameLabel.Text = "IskolRepo";
        appNameLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // taglineLabel
        // 
        taglineLabel.Anchor = AnchorStyles.None;
        taglineLabel.AutoSize = true;
        taglineLabel.BackColor = Color.Transparent;
        taglineLabel.Font = new Font("Segoe UI", 14F);
        taglineLabel.ForeColor = Color.FromArgb(180, 190, 205);
        taglineLabel.Location = new Point(104, 203);
        taglineLabel.Margin = new Padding(0, 0, 0, 24);
        taglineLabel.Name = "taglineLabel";
        taglineLabel.Size = new Size(252, 25);
        taglineLabel.TabIndex = 2;
        taglineLabel.Text = "Your academic file organizer";
        taglineLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // buttonLayout
        // 
        buttonLayout.Anchor = AnchorStyles.None;
        buttonLayout.AutoSize = true;
        buttonLayout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        buttonLayout.BackColor = Color.Transparent;
        buttonLayout.Controls.Add(openSemesterButton);
        buttonLayout.Controls.Add(orLabel);
        buttonLayout.Controls.Add(newSemesterButton);
        buttonLayout.Location = new Point(0, 252);
        buttonLayout.Margin = new Padding(0);
        buttonLayout.Name = "buttonLayout";
        buttonLayout.Size = new Size(460, 50);
        buttonLayout.TabIndex = 3;
        buttonLayout.WrapContents = false;
        // 
        // openSemesterButton
        // 
        openSemesterButton.BackColor = Color.FromArgb(24, 47, 83);
        openSemesterButton.Cursor = Cursors.Hand;
        openSemesterButton.FlatAppearance.BorderSize = 0;
        openSemesterButton.FlatStyle = FlatStyle.Flat;
        openSemesterButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        openSemesterButton.ForeColor = Color.White;
        openSemesterButton.Location = new Point(0, 0);
        openSemesterButton.Margin = new Padding(0);
        openSemesterButton.Name = "openSemesterButton";
        openSemesterButton.Size = new Size(200, 50);
        openSemesterButton.TabIndex = 3;
        openSemesterButton.Text = "Open Semester";
        openSemesterButton.UseVisualStyleBackColor = false;
        openSemesterButton.Click += openSemesterButton_Click;
        // 
        // orLabel
        // 
        orLabel.BackColor = Color.Transparent;
        orLabel.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        orLabel.ForeColor = Color.FromArgb(180, 190, 205);
        orLabel.Location = new Point(210, 0);
        orLabel.Margin = new Padding(10, 0, 10, 0);
        orLabel.Name = "orLabel";
        orLabel.Size = new Size(40, 50);
        orLabel.TabIndex = 4;
        orLabel.Text = "OR";
        orLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // newSemesterButton
        // 
        newSemesterButton.BackColor = Color.FromArgb(24, 47, 83);
        newSemesterButton.Cursor = Cursors.Hand;
        newSemesterButton.FlatAppearance.BorderSize = 0;
        newSemesterButton.FlatStyle = FlatStyle.Flat;
        newSemesterButton.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        newSemesterButton.ForeColor = Color.White;
        newSemesterButton.Location = new Point(260, 0);
        newSemesterButton.Margin = new Padding(0);
        newSemesterButton.Name = "newSemesterButton";
        newSemesterButton.Size = new Size(200, 50);
        newSemesterButton.TabIndex = 5;
        newSemesterButton.Text = "New Semester";
        newSemesterButton.UseVisualStyleBackColor = false;
        newSemesterButton.Click += newSemesterButton_Click;
        // 
        // StartupView
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(12, 14, 24);
        Controls.Add(rootLayout);
        Name = "StartupView";
        Size = new Size(800, 600);
        rootLayout.ResumeLayout(false);
        rootLayout.PerformLayout();
        contentLayout.ResumeLayout(false);
        contentLayout.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)logoMarkPictureBox).EndInit();
        buttonLayout.ResumeLayout(false);
        ResumeLayout(false);
    }
}
