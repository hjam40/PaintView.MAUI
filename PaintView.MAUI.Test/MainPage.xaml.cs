namespace PaintView.MAUI.Test;

public partial class MainPage : ContentPage
{
    public PaintView PaintView { get; set; }
    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        PaintView.Reset();
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
}