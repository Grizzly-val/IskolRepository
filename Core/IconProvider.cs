using System.Drawing;
using System.Windows.Forms;

namespace IskolRepository.Core;

/// <summary>
/// Provides reusable ListView icons for repository browsing.
/// </summary>
public sealed class IconProvider
{
    public const string FolderIconKey = "folder";
    public const string FileIconKey = "file";
    public const string SemesterIconKey = "semester";
    public const string SubjectIconKey = "subject";
    public const string RepositoryIconKey = "repository";
    public const string SubRepositoryIconKey = "subrepo";

    public ImageList CreateImageList()
    {
        var imageList = new ImageList
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(16, 16)
        };

        imageList.Images.Add(FolderIconKey, CreateFolderIcon());
        imageList.Images.Add(FileIconKey, CreateFileIcon());

        return imageList;
    }

    public ImageList CreateTreeViewImageList()
    {
        var imageList = new ImageList
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(16, 16)
        };

        imageList.Images.Add(SemesterIconKey, CreateFolderIcon());
        imageList.Images.Add(SubjectIconKey, CreateFolderIcon());
        imageList.Images.Add(RepositoryIconKey, CreateFolderIcon());
        imageList.Images.Add(SubRepositoryIconKey, CreateFolderIcon());
        imageList.Images.Add(FileIconKey, CreateFileIcon());

        return imageList;
    }

    public string GetIconKey(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            return FolderIconKey;
        }

        return FileIconKey;
    }

    private static Bitmap CreateFolderIcon()
    {
        var bitmap = new Bitmap(16, 16);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Transparent);

        using var tabBrush = new SolidBrush(Color.FromArgb(245, 204, 96));
        using var bodyBrush = new SolidBrush(Color.FromArgb(235, 187, 70));
        using var outlinePen = new Pen(Color.FromArgb(176, 133, 32));

        graphics.FillRectangle(tabBrush, 2, 3, 6, 3);
        graphics.FillRectangle(bodyBrush, 1, 5, 14, 9);
        graphics.DrawRectangle(outlinePen, 1, 5, 13, 8);
        graphics.DrawRectangle(outlinePen, 2, 3, 5, 2);

        return bitmap;
    }

    private static Bitmap CreateFileIcon()
    {
        var bitmap = new Bitmap(16, 16);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Transparent);

        var points = new[]
        {
            new Point(4, 1),
            new Point(10, 1),
            new Point(13, 4),
            new Point(13, 14),
            new Point(4, 14)
        };

        using var pageBrush = new SolidBrush(Color.White);
        using var foldBrush = new SolidBrush(Color.FromArgb(228, 232, 240));
        using var outlinePen = new Pen(Color.FromArgb(120, 130, 150));
        using var linePen = new Pen(Color.FromArgb(110, 160, 220), 1);

        graphics.FillPolygon(pageBrush, points);
        graphics.FillPolygon(foldBrush, new[] { new Point(10, 1), new Point(13, 4), new Point(10, 4) });
        graphics.DrawPolygon(outlinePen, points);
        graphics.DrawLine(outlinePen, 10, 1, 10, 4);
        graphics.DrawLine(outlinePen, 10, 4, 13, 4);
        graphics.DrawLine(linePen, 6, 7, 11, 7);
        graphics.DrawLine(linePen, 6, 9, 11, 9);
        graphics.DrawLine(linePen, 6, 11, 10, 11);

        return bitmap;
    }
}
