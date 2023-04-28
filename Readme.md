
# PaintView.MAUI

A View control for paint using touch controls.

## PaintView

A multiplatform control for paint with the finders, mouse or pointers. The view has properties for use it as a clear drawing area or use his control buttons:


![View with control buttons](https://github.com/hjam40/PaintView.MAUI/blob/master/PaintView.MAUI.Test/Screenshots/paintview.png)

![View without control buttons](https://github.com/hjam40/PaintView.MAUI/blob/master/PaintView.MAUI.Test/Screenshots/paintview2.png)

Properties:

 ```csharp
/// Binding property for use this control in MVVM.
public PaintView Self
/// Drawing area backgroud color
public new Color BackgroundColor
/// Show/Hide color selector button.
public bool ShowColorSelector
/// Show/Hide pointer size selector button.
public bool ShowPointerSelector
/// Show/Hide undo and redo buttons.
public bool ShowUndoRedoButtons
/// Show/Hide figures draw toolbar.
public bool ShowFigureButtons
/// Sets the size for control buttons.
public float ButtonsSize
/// Sets the border color for control buttons.
public Color ButtonsBorderColor
/// Sets the background color for control buttons.
public Color ButtonsBackgroundColor
/// Sets the background color for control buttons selected in toolbar.
public Color SelectedButtonBackgroundColor
/// Sets the drawing color.
public Color SelectedColor
/// Sets the drawing pointer size.
public float SelectedPointer
/// If true, control buttons are hidden while user draws.
public bool HideButtonsOnDrawing
/// Minimun movement distante for start to draw.
public float MinDistanceBetweenDrawingPoints
/// Colors list to show in the palette.
public List<Color> Palette
/// Pointers sizes list to show in the pointer selector.
public List<float> Pointers
 ```

Events:
 ```csharp
 /// Initialize the Paint view.
 public void Reset(bool clearNoUnDoDraws = true)
 /// Gets a bytes[] representation from drawing area snapshot.
 public async Task<byte[]> GetSnapshotBytesAsync(SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
 /// Gets an image representation from drawing area snapshot.
 public async Task<ImageSource> GetSnapshotAsync(SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
 /// Undo the drawing steps indicated.
 public void UnDo(int steps = 1);
 /// Redo the drawing steps indicated.
 public void ReDo(int steps = 1)
 /// Draws a line in the indicates coordinates.
 public void DrawLine(Point start, Point end, Color color, float pointerSize, bool canBeUnDo = true)
 /// Draws an oval in the indicates coordinates.
 public void DrawOval(Point start, Point end, Color strokeColor, Color fillColor, float pointerSize, bool canBeUnDo = true)
 /// Draws a rectangle in the indicates coordinates.
public void DrawRect(Point start, Point end, Color strokeColor, Color fillColor, float pointerSize, bool canBeUnDo = true)
 ```

### Install and configure PaintView

1. Download and Install [PaintView.MAUI](https://www.nuget.org/packages/PaintView.MAUI) NuGet package on your application.

1. Initialize the plugin in your `MauiProgram.cs`:

    ```csharp
    // Add the using to the top
    using PaintView.MAUI;
    
    public static MauiApp CreateMauiApp()
    {
    	var builder = MauiApp.CreateBuilder();
    
    	builder
    		.UseMauiApp<App>()
    		.UsePaintView(); // Add the use of the plugging
    
    	return builder.Build();
    }
    ```

### Using PaintView

In XAML, make sure to add the right XML namespace:

`xmlns:pv="clr-namespace:PaintView.MAUI;assembly=PaintView.MAUI"`

Use the control:
```xaml
    <Grid>
        <pv:PaintView Grid.Row="0" HorizontalOptions="Fill" VerticalOptions="Fill" BackgroundColor="White"
                      ShowColorSelector="True" ShowPointerSelector="True" ShowUndoRedoButtons="True"
                      ButtonsSize="40"
                      BindingContext="{x:Reference mpage}" Self="{Binding PaintView}"/>
    </Grid>
```

Use the events:
```csharp
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
    private void Button_Clicked_4(object sender, EventArgs e)
    {
        PaintView.DrawRect(new(200, 100), new(300, 200), Colors.Black, Colors.Cyan, 2);
        PaintView.DrawOval(new(200, 100), new(300, 200), Colors.Black, Colors.Cyan, 2, false);
        PaintView.DrawLine(new(200, 100), new(300, 200), Colors.Black, 2);
    }
```
