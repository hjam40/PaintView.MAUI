using GestureRecognizerView.MAUI;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace PaintView.MAUI;

public static class AppBuilderExtensions
{
    public static MauiAppBuilder UsePaintView(this MauiAppBuilder builder)
    {
        builder.UseGestureRecognizerView();
        builder.UseSkiaSharp();

        return builder;
    }
}
