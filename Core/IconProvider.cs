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
    public const string ImageIconKey = "image";
    public const string VideoIconKey = "video";

    public ImageList CreateImageList()
    {
        var imageList = new ImageList
        {
            ColorDepth = ColorDepth.Depth32Bit,
            ImageSize = new Size(16, 16)
        };

        imageList.Images.Add(FolderIconKey, CreateFolderIcon());
        imageList.Images.Add(ImageIconKey, CreateImageIcon());
        imageList.Images.Add(VideoIconKey, CreateVideoIcon());
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

        imageList.Images.Add(FolderIconKey, CreateFolderIcon());
        imageList.Images.Add(ImageIconKey, CreateImageIcon());
        imageList.Images.Add(VideoIconKey, CreateVideoIcon());
        imageList.Images.Add(FileIconKey, CreateFileIcon());

        return imageList;
    }

    public string GetIconKey(string filePath)
    {
        if (Directory.Exists(filePath))
        {
            return FolderIconKey;
        }

        var ext = Path.GetExtension(filePath).ToLowerInvariant();

        return ext switch
        {
            ".png" or ".jpg" or ".jpeg" or ".gif" or ".bmp" => ImageIconKey,
            ".mp4" or ".avi" or ".mkv" or ".mov" => VideoIconKey,
            _ => FileIconKey
        };
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

    private static Bitmap CreateImageIcon()
    {
        var bitmap = new Bitmap(16, 16);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Transparent);

        using var frameBrush = new SolidBrush(Color.FromArgb(180, 220, 255));
        using var framePen = new Pen(Color.FromArgb(70, 130, 200));
        using var skyBrush = new SolidBrush(Color.FromArgb(135, 195, 240));
        using var groundBrush = new SolidBrush(Color.FromArgb(100, 180, 100));
        using var sunBrush = new SolidBrush(Color.FromArgb(255, 220, 50));
        using var mountainBrush = new SolidBrush(Color.FromArgb(80, 140, 80));

        graphics.FillRectangle(frameBrush, 2, 2, 12, 12);
        graphics.FillRectangle(skyBrush, 3, 3, 10, 6);
        graphics.FillRectangle(groundBrush, 3, 9, 10, 4);
        graphics.FillEllipse(sunBrush, 4, 4, 3, 3);
        graphics.FillPolygon(mountainBrush, new[] { new Point(3, 13), new Point(8, 7), new Point(13, 13) });
        graphics.DrawRectangle(framePen, 2, 2, 12, 12);

        return bitmap;
    }

    private static Bitmap CreateVideoIcon()
    {
        var bitmap = new Bitmap(16, 16);
        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Transparent);

        using var bodyBrush = new SolidBrush(Color.FromArgb(60, 60, 60));
        using var bodyPen = new Pen(Color.FromArgb(30, 30, 30));
        using var stripBrush = new SolidBrush(Color.FromArgb(220, 220, 220));
        using var playBrush = new SolidBrush(Color.FromArgb(80, 210, 120));

        graphics.FillRectangle(bodyBrush, 1, 3, 14, 10);
        graphics.DrawRectangle(bodyPen, 1, 3, 14, 10);

        for (int x = 2; x <= 12; x += 3)
        {
            graphics.FillRectangle(stripBrush, x, 3, 2, 2);
            graphics.FillRectangle(stripBrush, x, 11, 2, 2);
        }

        graphics.FillPolygon(playBrush, new[] { new Point(6, 6), new Point(6, 10), new Point(11, 8) });

        return bitmap;
    }



}
