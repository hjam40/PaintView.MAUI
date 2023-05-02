using GestureRecognizerView.MAUI;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace PaintView.MAUI;

public partial class PaintView : ContentView
{
    public static readonly BindableProperty SelfProperty = BindableProperty.Create(nameof(Self), typeof(PaintView), typeof(PaintView), null, BindingMode.OneWayToSource);
    public static readonly new BindableProperty BackgroundColorProperty = BindableProperty.Create(nameof(BackgroundColor), typeof(Color), typeof(PaintView), Colors.White, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ShowColorSelectorProperty = BindableProperty.Create(nameof(ShowColorSelector), typeof(bool), typeof(PaintView), true, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ShowPointerSelectorProperty = BindableProperty.Create(nameof(ShowPointerSelector), typeof(bool), typeof(PaintView), true, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ShowUndoRedoButtonsProperty = BindableProperty.Create(nameof(ShowUndoRedoButtons), typeof(bool), typeof(PaintView), true, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ShowFigureButtonsProperty = BindableProperty.Create(nameof(ShowFigureButtons), typeof(bool), typeof(PaintView), true, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ButtonsSizeProperty = BindableProperty.Create(nameof(ButtonsSize), typeof(float), typeof(PaintView), 40f, propertyChanged: ForceRefresh);
    public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create(nameof(SelectedColor), typeof(Color), typeof(PaintView), Colors.Black, propertyChanged: RefreshPaint);
    public static readonly BindableProperty SelectedPointerProperty = BindableProperty.Create(nameof(SelectedPointer), typeof(float), typeof(PaintView), 1f, propertyChanged: RefreshPaint);
    public static readonly BindableProperty HideButtonsOnDrawingProperty = BindableProperty.Create(nameof(HideButtonsOnDrawing), typeof(bool), typeof(PaintView), true, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ButtonsBorderColorProperty = BindableProperty.Create(nameof(ButtonsBorderColor), typeof(Color), typeof(PaintView), Colors.LightGrey, propertyChanged: ForceRefresh);
    public static readonly BindableProperty ButtonsBackgroundColorProperty = BindableProperty.Create(nameof(ButtonsBackgroundColor), typeof(Color), typeof(PaintView), Colors.White, propertyChanged: ForceRefresh);
    public static readonly BindableProperty SelectedButtonBackgroundColorProperty = BindableProperty.Create(nameof(SelectedButtonBackgroundColor), typeof(Color), typeof(PaintView), Colors.Grey, propertyChanged: ForceRefresh);
    
    /// <summary>
    /// Binding property for use this control in MVVM.
    /// </summary>
    public PaintView Self
    {
        get { return (PaintView)GetValue(SelfProperty); }
        set { SetValue(SelfProperty, value); }
    }
    /// <summary>
    /// Drawing area backgroud color.
    /// </summary>
    public new Color BackgroundColor
    {
        get { return (Color)GetValue(BackgroundColorProperty); }
        set { SetValue(BackgroundColorProperty, value); }
    }
    /// <summary>
    /// Show/Hide color selector button.
    /// </summary>
    public bool ShowColorSelector
    {
        get { return (bool)GetValue(ShowColorSelectorProperty); }
        set { SetValue(ShowColorSelectorProperty, value); }
    }
    /// <summary>
    /// Show/Hide pointer size selector button.
    /// </summary>
    public bool ShowPointerSelector
    {
        get { return (bool)GetValue(ShowPointerSelectorProperty); }
        set { SetValue(ShowPointerSelectorProperty, value); }
    }
    /// <summary>
    /// Show/Hide undo and redo buttons.
    /// </summary>

    public bool ShowUndoRedoButtons
    {
        get { return (bool)GetValue(ShowUndoRedoButtonsProperty); }
        set { SetValue(ShowUndoRedoButtonsProperty, value); }
    }
    /// <summary>
    /// Show/Hide figures draw toolbar.
    /// </summary>

    public bool ShowFigureButtons
    {
        get { return (bool)GetValue(ShowFigureButtonsProperty); }
        set { SetValue(ShowFigureButtonsProperty, value); }
    }
    /// <summary>
    /// Sets the size for control buttons.
    /// </summary>
    public float ButtonsSize
    {
        get { return (float)GetValue(ButtonsSizeProperty); }
        set { SetValue(ButtonsSizeProperty, value); }
    }
    /// <summary>
    /// Sets the border color for control buttons.
    /// </summary>
    public Color ButtonsBorderColor
    {
        get { return (Color)GetValue(ButtonsBorderColorProperty); }
        set { SetValue(ButtonsBorderColorProperty, value); }
    }
    /// <summary>
    /// Sets the background color for control buttons.
    /// </summary>
    public Color ButtonsBackgroundColor
    {
        get { return (Color)GetValue(ButtonsBackgroundColorProperty); }
        set { SetValue(ButtonsBackgroundColorProperty, value); }
    }
    /// <summary>
    /// Sets the background color for control buttons selected in toolbar.
    /// </summary>
    public Color SelectedButtonBackgroundColor
    {
        get { return (Color)GetValue(SelectedButtonBackgroundColorProperty); }
        set { SetValue(SelectedButtonBackgroundColorProperty, value); }
    }
    /// <summary>
    /// Sets the drawing color.
    /// </summary>
    public Color SelectedColor
    {
        get { return (Color)GetValue(SelectedColorProperty); }
        set { SetValue(SelectedColorProperty, value); }
    }
    /// <summary>
    /// Sets the drawing pointer size.
    /// </summary>
    public float SelectedPointer
    {
        get { return (float)GetValue(SelectedPointerProperty); }
        set { SetValue(SelectedPointerProperty, value); }
    }
    /// <summary>
    /// If true, control buttons are hidden while user draws.
    /// </summary>
    public bool HideButtonsOnDrawing
    {
        get { return (bool)GetValue(HideButtonsOnDrawingProperty); }
        set { SetValue(HideButtonsOnDrawingProperty, value); }
    }
    /// <summary>
    /// Minimun movement distante for start to draw.
    /// </summary>
    public float MinDistanceBetweenDrawingPoints { get; set; } = 1f;
    /// <summary>
    /// Colors list to show in the palette.
    /// </summary>
    public List<Color> Palette { get; set; } = new List<Color> { Colors.Black, Colors.White, Colors.Red, Colors.Orange, Colors.Brown, Colors.Blue, Colors.LightBlue, Colors.Cyan, Colors.DarkGreen, Colors.Green, Colors.LightGreen, Colors.YellowGreen, Colors.Yellow, Colors.DarkGray, Colors.Gray, Colors.LightGrey };
    /// <summary>
    /// Pointers sizes list to show in the pointer selector.
    /// </summary>
    public List<float> Pointers { get; set; } = new List<float> { 1, 2, 3, 4, 5, 6, 8, 9, 10, 12, 15, 18, 20 };
    private Rect bounds = Rect.Zero;
    /// <summary>
    /// Sets the rectangle area where user can draw.
    /// </summary>
    public Rect DrawBounds { 
        get => bounds;
        set => bounds = value;
    }

    private uint activePointerId = uint.MaxValue;
    private bool drawing = false;
    private int drawingPoints = 0;
    private Point lastPoint;
    private SKPath currentPath;
    private SKPaint paint;
    private readonly List<Tuple<SKPath, SKPaint>> undopaths = new();
    private readonly List<Tuple<SKPath, SKPaint>> redopaths = new();
    private readonly List<Tuple<SKPath, SKPaint>> paintpaths = new();
    private bool forceRepaint = false;
    private bool showingPalette = false;
    private bool showingPointers = false;
    private float paletteOffset = 0f, pointersOffset = 0f;
    private bool panningPalette = false, panningPointerse = false;
    private SKPath undoPath, redoPath;
    private SKImage imageData = null;
    private bool capturing = false;
    private bool painting = false;
    private bool paintingSelected = false;
    private int figure = -1;
    private readonly int numFigureButtons = 5;
    private Point paintingStartPoint;
    private float scale = 0;
    private bool onlyBounds = false;
    public PaintView()
	{
		InitializeComponent();
        skCanvas.PaintSurface += SkCanvas_PaintSurface;
        grv.PointerGestureListener += Grv_PointerGestureListener;
        grv.TapGestureListener += Grv_TapGestureListener;
        grv.PanGestureListener += Grv_PanGestureListener;
        SizeChanged += PaintView_SizeChanged;
        paint = new() { Style = SKPaintStyle.Stroke, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round, StrokeWidth = SelectedPointer, Color = SelectedColor.ToSKColor() };
    }
    private void PaintView_SizeChanged(object sender, EventArgs e)
    {
        forceRepaint = true;
        skCanvas.InvalidateSurface();
    }
    private void Grv_PanGestureListener(object sender, PanGestureEventArgs args)
    {
        switch(args.Status)
        {
            case GestureRecognizerStatus.Started:
                if (showingPalette && args.Pointers[0].EndPoint.X < ButtonsSize)
                {
                    panningPalette = true;
                    paletteOffset += (float)args.IncY;
                    paletteOffset = Math.Clamp(paletteOffset, Math.Min(0f, (float)(Height - (Palette.Count * ButtonsSize + ButtonsSize))), 0f);
                    skCanvas.InvalidateSurface();
                }
                else if (showingPointers && args.Pointers[0].EndPoint.X < (ShowColorSelector ? ButtonsSize * 2 : ButtonsSize) && args.Pointers[0].EndPoint.X >= (ShowColorSelector ? ButtonsSize : 0))
                {
                    panningPointerse = true;
                    pointersOffset += (float)args.IncY;
                    pointersOffset = Math.Clamp(pointersOffset, Math.Min(0f, (float)(Height - (Pointers.Count * ButtonsSize + ButtonsSize))), 0f);
                    skCanvas.InvalidateSurface();
                }
                break;
            case GestureRecognizerStatus.Running:
                if (panningPalette)
                {
                    paletteOffset += (float)args.IncY;
                    paletteOffset = Math.Clamp(paletteOffset, Math.Min(0f, (float)(Height - (Palette.Count * ButtonsSize + ButtonsSize))), 0f);
                    skCanvas.InvalidateSurface();
                }
                else if (panningPointerse)
                {
                    pointersOffset += (float)args.IncY;
                    pointersOffset = Math.Clamp(pointersOffset, Math.Min(0f, (float)(Height - (Pointers.Count * ButtonsSize + ButtonsSize))), 0f);
                    skCanvas.InvalidateSurface();
                }
                break;
            case GestureRecognizerStatus.Complete:
            case GestureRecognizerStatus.Cancel:
                panningPointerse = panningPalette = false;
                break;
        }
    }

    private void Grv_TapGestureListener(object sender, TapGestureEventArgs args)
    {
        if (args.Status == GestureRecognizerStatus.Complete)
        {
            float y;
            if (ShowUndoRedoButtons)
                y = Math.Max(undoPath.Bounds.Height / scale * 6, undoPath.Bounds.Height / scale * 6 + ((float)Height - undoPath.Bounds.Height / scale * 6 - numFigureButtons * ButtonsSize) / 2);
            else
                y = ((float)Height - numFigureButtons * ButtonsSize) / 2;
            if (ShowFigureButtons && args.X >= (float)Width - ButtonsSize - 2 && args.Y >= y && args.Y <= y + ButtonsSize * numFigureButtons)
            {
                var newFigure = (int)Math.Truncate(((float)args.Y - y) / ButtonsSize);
                if (figure == newFigure)
                {
                    paintingSelected = false;
                    figure = -1;
                }
                else
                {
                    paintingSelected = true;
                    figure = newFigure;
                }
                skCanvas.InvalidateSurface();
            }
            else if (ShowColorSelector && args.X <= ButtonsSize && args.Y <= ButtonsSize)
            {
                paletteOffset = 0f;
                showingPalette = !showingPalette;
                forceRepaint = !showingPalette;
                skCanvas.InvalidateSurface();
            }
            else if (ShowPointerSelector && args.X >= (ShowColorSelector ? ButtonsSize : 0) && args.X <= (ShowColorSelector ? ButtonsSize * 2 : ButtonsSize) && args.Y <= ButtonsSize)
            {
                pointersOffset = 0f;
                showingPointers = !showingPointers;
                forceRepaint = !showingPointers;
                skCanvas.InvalidateSurface();
            }
            else if (ShowUndoRedoButtons && args.X >= ((float)Width - undoPath.Bounds.Width / scale * 4) && args.Y <= undoPath.Bounds.Height / scale * 6)
            {
                if (undopaths.Count > 0 && args.X >= ((float)Width - undoPath.Bounds.Width / scale * 4) && args.X <= ((float)Width - undoPath.Bounds.Width / scale * 2) && args.Y <= undoPath.Bounds.Height / scale * 6)
                    UnDo();
                else if (redopaths.Count > 0 && args.X >= ((float)Width - undoPath.Bounds.Width / scale * 2) && args.Y <= undoPath.Bounds.Height / scale * 6)
                    ReDo();
            }
            else if (showingPalette && args.X <= ButtonsSize)
            {
                if (args.Y <= ButtonsSize * Palette.Count + ButtonsSize)
                {
                    int idx = (int)(((args.Y - ButtonsSize) / ButtonsSize) + Math.Abs(paletteOffset / ButtonsSize));
                    SelectedColor = Palette[idx];
                    showingPalette = false;
                    forceRepaint = true;
                    skCanvas.InvalidateSurface();
                }
            }
            else if (showingPointers && args.X >= (ShowColorSelector ? ButtonsSize : 0) && args.X <= (ShowColorSelector ? ButtonsSize * 2 : ButtonsSize))
            {
                if (args.Y <= ButtonsSize * Pointers.Count + ButtonsSize)
                {
                    int idx = (int)(((args.Y - ButtonsSize) / ButtonsSize) + Math.Abs(pointersOffset / ButtonsSize));
                    SelectedPointer = Pointers[idx];
                    showingPointers = false;
                    forceRepaint = true;
                    skCanvas.InvalidateSurface();
                }
            }
        }
    }
    private void Grv_PointerGestureListener(object sender, PointerGestureEventArgs args)
    {
        switch (args.Status)
        {
            case PointerRecognizerStatus.Press:
                if (!drawing)
                {
                    float y;
                    if (ShowUndoRedoButtons)
                        y = Math.Max(undoPath.Bounds.Height / scale * 6, undoPath.Bounds.Height / scale * 6 + ((float)Height - undoPath.Bounds.Height / scale * 6 - numFigureButtons * ButtonsSize) / 2);
                    else
                        y = ((float)Height - numFigureButtons * ButtonsSize) / 2;
                    if (!(ShowFigureButtons && args.X >= (float)Width - ButtonsSize - 2 && args.Y >= y && args.Y <= y + ButtonsSize * numFigureButtons) &&
                        !(ShowColorSelector && args.X <= ButtonsSize && args.Y <= ButtonsSize) &&
                        !(ShowPointerSelector && args.X >= (ShowColorSelector ? ButtonsSize : 0) && args.X <= (ShowColorSelector ? ButtonsSize * 2 : ButtonsSize) && args.Y <= ButtonsSize) &&
                        !(ShowUndoRedoButtons && args.X >= ((float)Width - undoPath.Bounds.Width / scale * 4.5) && args.Y <= undoPath.Bounds.Height / scale * 6.5) &&
                        !(showingPointers && args.X >= (ShowColorSelector ? ButtonsSize : 0) && args.X <= (ShowColorSelector ? ButtonsSize * 2 : ButtonsSize)) &&
                        !(showingPalette && args.X <= ButtonsSize) &&
                        (bounds == Rect.Zero || (args.X >= bounds.X * scale && args.X <= (bounds.X + bounds.Width) * scale && args.Y >= bounds.Y * scale && args.Y <= (bounds.Y + bounds.Height) * scale)))
                    {
                        currentPath = new();
                        drawingPoints = 0;
                        activePointerId = args.Pointer.PointerId;
                        lastPoint = new Point(args.Pointer.EndPoint.X*scale, args.Pointer.EndPoint.Y * scale);
                        currentPath.MoveTo((float)lastPoint.X, (float)lastPoint.Y);
                        if (paintingSelected)
                        {
                            paintingStartPoint = lastPoint;
                            painting = true;
                        }
                        else
                            drawing = true;
                    }
                }
                break;
            case PointerRecognizerStatus.Move:
                if (drawing && activePointerId == args.Pointer.PointerId)
                {
                    if (lastPoint.Distance(args.Pointer.EndPoint) >= MinDistanceBetweenDrawingPoints)
                    {
                        if (bounds == Rect.Zero || (args.X >= bounds.X * scale && args.X <= (bounds.X + bounds.Width) * scale && args.Y >= bounds.Y * scale && args.Y <= (bounds.Y + bounds.Height) * scale))
                        {
                            lastPoint = new Point(args.Pointer.EndPoint.X * scale, args.Pointer.EndPoint.Y * scale);
                            currentPath.LineTo((float)lastPoint.X, (float)lastPoint.Y);
                            forceRepaint = HideButtonsOnDrawing && drawingPoints == 0;
                            drawingPoints++;
                        }
                        else
                        {
                            if (drawingPoints > 0)
                            {
                                undopaths.Add(new(currentPath, paint));
                                redopaths.Clear();
                                currentPath = new();
                            }
                            drawing = false;
                            activePointerId = uint.MaxValue;
                        }
                        skCanvas.InvalidateSurface();
                    }
                }
                else if (painting && activePointerId == args.Pointer.PointerId)
                {
                    if (lastPoint.Distance(args.Pointer.EndPoint) >= MinDistanceBetweenDrawingPoints)
                    {
                        if (bounds == Rect.Zero || (args.X >= bounds.X * scale && args.X <= (bounds.X + bounds.Width) * scale && args.Y >= bounds.Y * scale && args.Y <= (bounds.Y + bounds.Height) * scale))
                        {
                            lastPoint = new Point(args.Pointer.EndPoint.X * scale, args.Pointer.EndPoint.Y * scale);
                            currentPath = new();
                            var xdis = (float)(lastPoint.X - paintingStartPoint.X);
                            var ydis = (float)(lastPoint.Y - paintingStartPoint.Y);
                            switch (figure)
                            {
                                case 0:
                                    currentPath.MoveTo((float)paintingStartPoint.X, (float)paintingStartPoint.Y);
                                    currentPath.LineTo((float)lastPoint.X, (float)lastPoint.Y);
                                    break;
                                case 1:
                                    currentPath.AddRect(new((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)paintingStartPoint.X + Math.Min(xdis, ydis), (float)paintingStartPoint.Y + Math.Min(xdis, ydis)));
                                    break;
                                case 2:
                                    currentPath.AddCircle((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)paintingStartPoint.Distance(lastPoint));
                                    break;
                                case 3:
                                    currentPath.AddRect(new((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)lastPoint.X, (float)lastPoint.Y));
                                    break;
                                case 4:
                                    currentPath.AddOval(new((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)lastPoint.X, (float)lastPoint.Y));
                                    break;
                            }
                            forceRepaint = true;
                            drawingPoints++;
                        }
                        else
                        {
                            if (drawingPoints > 0)
                            {
                                undopaths.Add(new(currentPath, paint));
                                redopaths.Clear();
                                currentPath = new();
                            }
                            painting = false;
                            activePointerId = uint.MaxValue;
                        }
                        skCanvas.InvalidateSurface();
                    }
                }
                break;
            case PointerRecognizerStatus.Release:
                if (drawing && activePointerId == args.Pointer.PointerId)
                {
                    if (lastPoint.Distance(args.Pointer.EndPoint) >= MinDistanceBetweenDrawingPoints)
                    {
                        lastPoint = new Point(args.Pointer.EndPoint.X * scale, args.Pointer.EndPoint.Y * scale);
                        currentPath.LineTo((float)lastPoint.X, (float)lastPoint.Y);
                        drawingPoints++;
                    }
                    if (drawingPoints > 0)
                    {
                        undopaths.Add(new(currentPath, paint));
                        redopaths.Clear();
                        currentPath = new();
                    }
                    if (drawingPoints > 0 || HideButtonsOnDrawing)
                        skCanvas.InvalidateSurface();
                    drawing = false;
                    activePointerId = uint.MaxValue;
                }
                else if (painting && activePointerId == args.Pointer.PointerId)
                {
                    if (lastPoint.Distance(args.Pointer.EndPoint) >= MinDistanceBetweenDrawingPoints)
                    {
                        lastPoint = new Point(args.Pointer.EndPoint.X * scale, args.Pointer.EndPoint.Y * scale);
                        currentPath = new();
                        var xdis = (float)(lastPoint.X - paintingStartPoint.X);
                        var ydis = (float)(lastPoint.Y - paintingStartPoint.Y);
                        switch (figure)
                        {
                            case 0:
                                currentPath.MoveTo((float)paintingStartPoint.X, (float)paintingStartPoint.Y);
                                currentPath.LineTo((float)lastPoint.X, (float)lastPoint.Y);
                                break;
                            case 1:
                                currentPath.AddRect(new((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)paintingStartPoint.X + Math.Min(xdis, ydis), (float)paintingStartPoint.Y + Math.Min(xdis, ydis)));
                                break;
                            case 2:
                                currentPath.AddCircle((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)paintingStartPoint.Distance(lastPoint));
                                break;
                            case 3:
                                currentPath.AddRect(new((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)lastPoint.X, (float)lastPoint.Y));
                                break;
                            case 4:
                                currentPath.AddOval(new((float)paintingStartPoint.X, (float)paintingStartPoint.Y, (float)lastPoint.X, (float)lastPoint.Y));
                                break;
                        }
                        drawingPoints++;
                    }
                    if (drawingPoints > 0)
                    {
                        undopaths.Add(new(currentPath, paint));
                        redopaths.Clear();
                        currentPath = new();
                        forceRepaint = true;
                    }
                    painting = false;
                    if (drawingPoints > 0 || HideButtonsOnDrawing)
                        skCanvas.InvalidateSurface();
                }
                break;
            case PointerRecognizerStatus.Exit:
            case PointerRecognizerStatus.Cancel:
                if ((drawing || painting) && activePointerId == args.Pointer.PointerId)
                {
                    if (drawingPoints > 0)
                    {
                        undopaths.Add(new(currentPath, paint));
                        redopaths.Clear();
                        currentPath = new();
                    }
                    drawing = false;
                    painting = false;
                    activePointerId = uint.MaxValue;
                    if (HideButtonsOnDrawing)
                        skCanvas.InvalidateSurface();
                }
                break;
        }
    }
    /// <summary>
    /// Undo the drawing steps indicated.
    /// </summary>
    /// <param name="steps">Number of steps to undo</param>
    public void UnDo(int steps = 1)
    {
        while (steps > 0 && undopaths.Count > 0)
        {
            var p = undopaths.Last();
            undopaths.Remove(p);
            redopaths.Add(p);
            if (undopaths.Count > 0 && undopaths.Last().Item2.Style == SKPaintStyle.Fill)
            {
                p = undopaths.Last();
                undopaths.Remove(p);
                redopaths.Add(p);
            }
            steps--;
        }
        forceRepaint = true;
        skCanvas.InvalidateSurface();
    }
    /// <summary>
    /// Redo the drawing steps indicated.
    /// </summary>
    /// <param name="steps">Number of steps to redo</param>
    public void ReDo(int steps = 1)
    {
        while (steps > 0 && redopaths.Count > 0)
        {
            var p = redopaths.Last();
            redopaths.Remove(p);
            undopaths.Add(p);
            if (redopaths.Count > 0 && p.Item2.Style == SKPaintStyle.Fill)
            {
                p = redopaths.Last();
                redopaths.Remove(p);
                undopaths.Add(p);
            }
            steps--;
        }
        forceRepaint = true;
        skCanvas.InvalidateSurface();
    }
    /// <summary>
    /// Draws a line in the indicates coordinates.
    /// </summary>
    /// <param name="start">Line start point</param>
    /// <param name="end">Line end point</param>
    /// <param name="color">Stroke color for the line</param>
    /// <param name="pointerSize">Stroke width for the line</param>
    /// <param name="canBeUnDo">Indicates if the user can undo this draw with the undo button</param>
    public void DrawLine(Point start, Point end, Color color, float pointerSize, bool canBeUnDo = true)
    {
        if (start.X >= 0 && start.X <= Width && end.X >= 0 && end.X <= Width && start.Y >= 0 && start.Y <= Height && end.Y >= 0 && end.Y <= Height)
        {
            SKPath path = new();
            SKPaint p = new() { Style = SKPaintStyle.Stroke, Color = color.ToSKColor(), StrokeWidth = pointerSize };
            path.MoveTo((float)start.X * scale, (float)start.Y * scale);
            path.LineTo((float)end.X * scale, (float)end.Y * scale);
            if (canBeUnDo)
            {
                undopaths.Add(new(path, p));
                redopaths.Clear();
            }
            else
                paintpaths.Add(new(path, p));
            forceRepaint = true;
            skCanvas.InvalidateSurface();
        }
    }
    /// <summary>
    /// Draws an oval in the indicates coordinates.
    /// </summary>
    /// <param name="start">Oval rect start point</param>
    /// <param name="end">Oval rect end point</param>
    /// <param name="strokeColor">Stroke color for the oval</param>
    /// <param name="fillColor">Stroke color for the oval</param>
    /// <param name="pointerSize">Stroke width for the oval</param>
    /// <param name="canBeUnDo">Indicates if the user can undo this draw with the undo button</param>
    public void DrawOval(Point start, Point end, Color strokeColor, Color fillColor, float pointerSize, bool canBeUnDo = true)
    {
        if (start.X >= 0 && start.X <= Width && end.X >= 0 && end.X <= Width && start.Y >= 0 && start.Y <= Height && end.Y >= 0 && end.Y <= Height)
        {
            SKPath path = new();
            SKPaint ps = new() { Style = SKPaintStyle.Stroke, Color = strokeColor.ToSKColor(), StrokeWidth = pointerSize };
            SKPaint pf = new() { Style = SKPaintStyle.Fill, Color = fillColor.ToSKColor(), StrokeWidth = pointerSize };
            path.AddOval(new((float)start.X * scale, (float)start.Y * scale, (float)end.X * scale, (float)end.Y * scale));
            if (fillColor != Colors.Transparent)
            {
                if (canBeUnDo)
                    undopaths.Add(new(path, pf));
                else
                    paintpaths.Add(new(path, pf));
            }
            if (canBeUnDo)
            {
                undopaths.Add(new(path, ps));
                redopaths.Clear();
            }
            else
                paintpaths.Add(new(path, ps));
            forceRepaint = true;
            skCanvas.InvalidateSurface();
        }
    }
    /// <summary>
    /// Draws a rectangle in the indicates coordinates.
    /// </summary>
    /// <param name="start">Rectangle start point</param>
    /// <param name="end">Rectangle end point</param>
    /// <param name="strokeColor">Stroke color for the rectangle</param>
    /// <param name="fillColor">Stroke color for the rectangle</param>
    /// <param name="pointerSize">Stroke width for the rectangle</param>
    /// <param name="canBeUnDo">Indicates if the user can undo this draw with the undo button</param>
    public void DrawRect(Point start, Point end, Color strokeColor, Color fillColor, float pointerSize, bool canBeUnDo = true)
    {
        if (start.X >= 0 && start.X <= Width && end.X >= 0 && end.X <= Width && start.Y >= 0 && start.Y <= Height && end.Y >= 0 && end.Y <= Height)
        {
            SKPath path = new();
            SKPaint ps = new() { Style = SKPaintStyle.Stroke, Color = strokeColor.ToSKColor(), StrokeWidth = pointerSize };
            SKPaint pf = new() { Style = SKPaintStyle.Fill, Color = fillColor.ToSKColor(), StrokeWidth = pointerSize };
            path.AddRect(new((float)start.X * scale, (float)start.Y * scale, (float)end.X * scale, (float)end.Y * scale));
            if (fillColor != Colors.Transparent)
            {
                if (canBeUnDo)
                    undopaths.Add(new(path, pf));
                else
                    paintpaths.Add(new(path, pf));
            }
            if (canBeUnDo)
            {
                undopaths.Add(new(path, ps));
                redopaths.Clear();
            }
            else
                paintpaths.Add(new(path, ps));
            forceRepaint = true;
            skCanvas.InvalidateSurface();
        }
    }
    /// <summary>
    /// Draws a text in the indicates coordinates.
    /// </summary>
    /// <param name="start">Rectangle start point</param>
    /// <param name="strokeColor">Stroke color for the text</param>
    /// <param name="fillColor">Stroke color for the text</param>
    /// <param name="pointerSize">Stroke width for the text</param>
    /// <param name="fontSize">Font size for the text</param>
    /// <param name="scaleX">Scale X for the text</param>
    /// <param name="canBeUnDo">Indicates if the user can undo this draw with the undo button</param>
    public void DrawText(Point start, string text, Color strokeColor, Color fillColor, float pointerSize, float fontSize, float scaleX = 1, bool canBeUnDo = true)
    {
        if (start.X >= 0 && start.X <= Width && start.Y >= 0 && start.Y <= Height)
        {
            var font = new SKFont(SKTypeface.Default, fontSize * scale, scaleX);
            SKPaint ps = new() { Style = SKPaintStyle.Stroke, Color = strokeColor.ToSKColor(), StrokeWidth = pointerSize };
            SKPaint pf = new() { Style = SKPaintStyle.Fill, Color = fillColor.ToSKColor(), StrokeWidth = pointerSize };
            SKPaint skPaint = new(font);
            var path = skPaint.GetTextPath(text, (float)start.X * scale, (float)start.Y * scale);

            if (fillColor != Colors.Transparent)
            {
                if (canBeUnDo)
                    undopaths.Add(new(path, pf));
                else
                    paintpaths.Add(new(path, pf));
            }
            if (canBeUnDo)
            {
                undopaths.Add(new(path, ps));
                redopaths.Clear();
            }
            else
                paintpaths.Add(new(path, ps));
            forceRepaint = true;
            skCanvas.InvalidateSurface();
        }
    }
    private static void ForceRefresh(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue && bindable is PaintView pv)
        {
            pv.forceRepaint = true;
            pv.skCanvas.InvalidateSurface();
        }
    }
    private static void RefreshPaint(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue && bindable is PaintView pv)
        {
            pv.paint = new() { Style = SKPaintStyle.Stroke, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round, StrokeWidth = pv.SelectedPointer, Color = pv.SelectedColor.ToSKColor() };
        }
    }
    private void DrawFigureButtons(SKCanvas canvas)
    {
        if (ShowFigureButtons)
        {
            float scaleButtonSize = ButtonsSize * scale;
            float y;
            if (ShowUndoRedoButtons)
                y = Math.Max(undoPath.Bounds.Height * 6, undoPath.Bounds.Height * 6 + ((float)Height * scale - undoPath.Bounds.Height * 6 - numFigureButtons * scaleButtonSize) / 2);
            else
                y = ((float)Height*scale - numFigureButtons * scaleButtonSize) / 2;
            float x = (float)Width*scale - scaleButtonSize - 2;
            float fsize = scaleButtonSize * 3f / 4f;
            float xinc = (scaleButtonSize - fsize) / 2;
            float yoffset = 0;
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Fill, Color = (figure == 0 ? SelectedButtonBackgroundColor.ToSKColor() : ButtonsBackgroundColor.ToSKColor()) });
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
            canvas.DrawLine(x + xinc, y + yoffset + xinc, x + xinc + fsize, y + yoffset + xinc + fsize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = SKColors.Black });
            yoffset += scaleButtonSize;
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Fill, Color = (figure == 1 ? SelectedButtonBackgroundColor.ToSKColor() : ButtonsBackgroundColor.ToSKColor()) });
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
            canvas.DrawRect(x + xinc, y + yoffset + xinc, fsize, fsize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = SKColors.Black });
            yoffset += scaleButtonSize;
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Fill, Color = (figure == 2 ? SelectedButtonBackgroundColor.ToSKColor() : ButtonsBackgroundColor.ToSKColor()) });
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
            canvas.DrawCircle(x + scaleButtonSize / 2, y + yoffset + scaleButtonSize / 2, fsize / 2, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = SKColors.Black });
            yoffset += scaleButtonSize;
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Fill, Color = (figure == 3 ? SelectedButtonBackgroundColor.ToSKColor() : ButtonsBackgroundColor.ToSKColor()) });
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
            canvas.DrawRect(x + xinc, y + yoffset + xinc*2, fsize, fsize-xinc*2, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = SKColors.Black });
            yoffset += scaleButtonSize;
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Fill, Color = (figure == 4 ? SelectedButtonBackgroundColor.ToSKColor() : ButtonsBackgroundColor.ToSKColor()) });
            canvas.DrawRect(x, y + yoffset, (float)scaleButtonSize, (float)scaleButtonSize, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
            canvas.DrawOval(x + scaleButtonSize / 2, y + yoffset + scaleButtonSize / 2, fsize/2, (fsize - xinc * 2)/2, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = SKColors.Black });
        }
    }
    private void DrawPointers(SKCanvas canvas)
    {
        if (ShowPointerSelector)
        {
            float x = ShowColorSelector ? (float)ButtonsSize * scale : 0;
            float radio;
            if (showingPointers)
            {
                int i = 0;
                foreach(var diameter in Pointers)
                {
                    radio = Math.Clamp(diameter / 2, 1f, (float)ButtonsSize / 2) * scale;
                    canvas.DrawRect(x, (pointersOffset + (float)ButtonsSize + (float)ButtonsSize * i) * scale, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Fill, Color = ButtonsBackgroundColor.ToSKColor() });
                    canvas.DrawRect(x, (pointersOffset + (float)ButtonsSize + (float)ButtonsSize * i) * scale, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
                    canvas.DrawCircle(x + ((float)ButtonsSize / 2) * scale, (pointersOffset + (float)ButtonsSize + (float)ButtonsSize * i + (float)ButtonsSize / 2) * scale, radio, new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black });
                    i++;
                }
            }
            canvas.DrawRect(x, 0, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.White });
            canvas.DrawRect(x, 0, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
            radio = Math.Max(1, SelectedPointer / 2) * scale;
            canvas.DrawCircle(x + ((float)ButtonsSize / 2) * scale, ((float)ButtonsSize / 2) * scale, radio, new SKPaint { Style = SKPaintStyle.Fill, Color = SKColors.Black });
        }
    }
    private void DrawPalette(SKCanvas canvas)
    {
        if (ShowColorSelector)
        {
            if (showingPalette)
            {
                int i = 0;
                foreach (var color in Palette)
                {
                    canvas.DrawRect(0, (paletteOffset + (float)ButtonsSize + (float)ButtonsSize * i) * scale, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Fill, Color = color.ToSKColor() });
                    i++;
                }
            }
            canvas.DrawRect(0, 0, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Fill, Color = SelectedColor.ToSKColor() });
            canvas.DrawRect(0, 0, (float)ButtonsSize * scale, (float)ButtonsSize * scale, new SKPaint { Style = SKPaintStyle.Stroke, StrokeWidth = 2, Color = ButtonsBorderColor.ToSKColor() });
        }
    }
    private void DrawDoButtons(SKCanvas canvas)
    {
        if (ShowUndoRedoButtons)
        {
            undoPath = SKPath.ParseSvgPathData("M18 13C17.4904 11.9961 16.6247 11.1655 15.5334 10.6333C14.442 10.1011 13.1842 9.89624 11.9494 10.0495C9.93127 10.3 8.52468 11.6116 7 12.8186M7 10V13H10");
            undoPath.Transform(SKMatrix.CreateScale(2f * scale, 2f * scale));
            undoPath.Transform(SKMatrix.CreateTranslation((float)Width * scale - undoPath.Bounds.Width * 4, 0));
            redoPath = SKPath.ParseSvgPathData("M6 13C6.50963 11.9961 7.37532 11.1655 8.46665 10.6333C9.55797 10.1011 10.8158 9.89624 12.0506 10.0495C14.0687 10.3 15.4753 11.6116 17 12.8186M17 10V13H14");
            redoPath.Transform(SKMatrix.CreateScale(2f * scale, 2f * scale));
            redoPath.Transform(SKMatrix.CreateTranslation((float)Width * scale - undoPath.Bounds.Width * 2, 0));
            SKPaint p = new() { Style = SKPaintStyle.Stroke, StrokeCap = SKStrokeCap.Round, StrokeJoin = SKStrokeJoin.Round, StrokeWidth = 2.5f * scale, Color = undopaths.Count > 0 ? SKColors.Black : SKColors.LightGray };
            canvas.DrawPath(undoPath, p);
            p.Color = redopaths.Count > 0 ? SKColors.Black : SKColors.LightGray;
            canvas.DrawPath(redoPath, p);
        }
    }
    /// <summary>
    /// Initialize the Paint view.
    /// </summary>
    /// <param name="clearNoUnDoDraws">If true, figures drawn by drawing methods will not be eraser</param>
    public void Reset(bool clearNoUnDoDraws = true)
    {
        undopaths.Clear();
        redopaths.Clear();
        if (clearNoUnDoDraws) paintpaths.Clear();
        forceRepaint = true;
        showingPalette = false;
        showingPointers = false;
        drawing = false;
        capturing = false;
        painting = false;
        paintingSelected = false;
        figure = -1;
        skCanvas.InvalidateSurface();
    }
    /// <summary>
    /// Gets a bytes[] representation from drawing area snapshot.
    /// </summary>
    /// <param name="onlyDrawBounds">Limit the snapshot to DrawBounds area</param>
    /// <param name="format">Image format</param>
    /// <param name="quality">Image quality</param>
    /// <returns>Snapshot image bytes</returns>
    public async Task<byte[]> GetSnapshotBytesAsync(bool onlyDrawBounds = true, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        capturing = true;
        imageData = null;
        onlyBounds = onlyDrawBounds;
        skCanvas.InvalidateSurface();
        while (imageData == null)
            await Task.Delay(TimeSpan.FromMilliseconds(10));
        capturing = false;
        return imageData.Encode(format, quality).ToArray();
    }
    /// <summary>
    /// Gets an image representation from drawing area snapshot.
    /// </summary>
    /// <param name="onlyDrawBounds">Limit the snapshot to DrawBounds area</param>
    /// <param name="format">Image format</param>
    /// <param name="quality">Image quality</param>
    /// <returns>Snapshot image</returns>
    public async Task<ImageSource> GetSnapshotAsync(bool onlyDrawBounds = true, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        var data = await GetSnapshotBytesAsync(onlyDrawBounds, format, quality);
        MemoryStream ms = new();
        ms.Write(data);
        ms.Position = 0;
        return ImageSource.FromStream(() => ms);
    }

    private void SkCanvas_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        if (scale == 0)
            scale = canvas.LocalClipBounds.Width / Math.Max(1f, (float)Width);
        Self ??= this;

        if (forceRepaint || capturing)
        {
            canvas.Clear(BackgroundColor.ToSKColor());
            foreach (var path in paintpaths)
                canvas.DrawPath(path.Item1, path.Item2);
            foreach (var path in undopaths)
                canvas.DrawPath(path.Item1, path.Item2);
            forceRepaint = false;
        }
        if (currentPath != null)
            canvas.DrawPath(currentPath, paint);
        if (capturing)
        {
            if (onlyBounds)
                imageData = e.Surface.Snapshot(new SKRectI((int)(bounds.X*scale), (int)(bounds.Y*scale), (int)((bounds.X+bounds.Width)*scale), (int)((bounds.Y + bounds.Height) * scale)));
            else
                imageData = e.Surface.Snapshot();
        }
        if (!HideButtonsOnDrawing || (!drawing && !painting))
        {
            DrawPalette(canvas);
            DrawPointers(canvas);
            DrawDoButtons(canvas);
            DrawFigureButtons(canvas);
        }
    }
}