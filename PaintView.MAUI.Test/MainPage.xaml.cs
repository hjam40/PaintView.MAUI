namespace PaintView.MAUI.Test;

public partial class MainPage : ContentPage
{
    private PaintView paintView;
    public PaintView PaintView 
    { 
        get => paintView;
        set
        {
            paintView = value;
            if (paintView != null)
            {
                float x = (float)(paintView.Width - 300) / 2f;
                float y = (float)(paintView.Height - 170) / 2f;
                paintView.DrawText(new(x, y), "Sign here:", Colors.Black, Colors.Black, 0f, 20f, 1, false);
                paintView.DrawRect(new(x, y + 20), new(x + 300, y + 170), Colors.Black, Colors.Transparent, 1f, false);
                paintView.DrawBounds = new Rect(x + 1, y + 1 + 20, 299, 149);
            }
        }
    }
    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        PaintView.Reset(false);
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        image.Source = await PaintView.GetSnapshotAsync();
    }

    private void Button_Clicked_2(object sender, EventArgs e)
    {
        PaintView.UnDo();
    }

    private void Button_Clicked_3(object sender, EventArgs e)
    {
        PaintView.ReDo();
    }
    private void Button_Clicked_4(object sender, EventArgs e)
    {
        PaintView.DrawRect(new(200, 100), new(300, 200), Colors.Black, Colors.Cyan, 2);
        PaintView.DrawOval(new(200, 100), new(300, 200), Colors.Black, Colors.Cyan, 2, false);
        PaintView.DrawLine(new(200, 100), new(300, 200), Colors.Black, 2);
    }
    private void Button_Clicked_5(object sender, EventArgs e)
    {
        PaintView.DrawText(new(200, 200), eText.Text, Colors.Black, Colors.Blue, 1, 25, 1.2f);
    }
}