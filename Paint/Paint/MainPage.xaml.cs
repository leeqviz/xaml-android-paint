using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TouchTracking;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Paint
{
    public partial class MainPage : ContentPage
    {
        int mode = 1;
        SKPoint p1, p2;
        SKBitmap bitmap;
        SKCanvas fig;
        SKColor colorBuffer;

        Dictionary<long, SKPath> inProgressPaths = new Dictionary<long, SKPath>();

        SKPaint ppp = new SKPaint
        {
            Style = SKPaintStyle.Fill,
            Color = SKColors.White,
        };

        SKPaint paint = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = SKColors.Black,
            StrokeWidth = 10,
            StrokeCap = SKStrokeCap.Round,
            StrokeJoin = SKStrokeJoin.Round
        };
        public MainPage()
        {
            bitmap = new SKBitmap(1200, 1600);
            fig = new SKCanvas(bitmap);
            fig.DrawRect(0, 0, 1200, 1600, ppp); 
            InitializeComponent();
        }
        void OnTouchEffectAction(object sender, TouchActionEventArgs args)
        {
            switch (args.Type)
            {
                case TouchActionType.Pressed:
                    p1 = ConvertToPixel(args.Location);
                    p2 = ConvertToPixel(args.Location);
                    if (mode == 1)
                    {
                        SKPath path = new SKPath();
                        path.MoveTo(ConvertToPixel(args.Location));
                        inProgressPaths.Add(args.Id, path);
                    }
                    else if (mode == 5) FloodFillImage(bitmap, (int)p1.X, (int)p1.Y, paint.Color);
                    canvasView.InvalidateSurface();
                    break;
                case TouchActionType.Moved:
                    p2 = ConvertToPixel(args.Location);
                    if (mode == 1)
                    {
                        colorBuffer = paint.Color;
                        SKPath path = inProgressPaths[args.Id];
                        path.LineTo(ConvertToPixel(args.Location));
                        paint.Color = colorBuffer;
                    }
                    canvasView.InvalidateSurface();
                    break;
                case TouchActionType.Released:
                    p2 = ConvertToPixel(args.Location);
                    if (mode == 1) inProgressPaths.Clear();
                    else if (mode == 2) fig.DrawRect(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y, paint);
                    else if (mode == 3) fig.DrawOval(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y, paint);
                    else if (mode == 4) fig.DrawLine(p1, p2, paint);
                    canvasView.InvalidateSurface();
                    break;
                case TouchActionType.Cancelled:
                    if (inProgressPaths.ContainsKey(args.Id) && mode == 1) inProgressPaths.Remove(args.Id);
                    canvasView.InvalidateSurface();
                    break;
            }
        }
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKCanvas canvas = args.Surface.Canvas;
            canvas.Clear();
            canvas.DrawBitmap(bitmap, 0, 0);
            if (mode == 1)
            {
                colorBuffer = paint.Color;
                foreach (SKPath path in inProgressPaths.Values)
                    fig.DrawPath(path, paint);
                paint.Color = colorBuffer;
            }
            else if (mode == 2) canvas.DrawRect(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y, paint);
            else if (mode == 3) canvas.DrawOval(p1.X, p1.Y, p2.X - p1.X, p2.Y - p1.Y, paint);
            else if (mode == 4) canvas.DrawLine(p1, p2, paint);
        }
        SKPoint ConvertToPixel(TouchTrackingPoint pt)
        {
            return new SKPoint((float)(canvasView.CanvasSize.Width * pt.X / canvasView.Width),
                                (float)(canvasView.CanvasSize.Height * pt.Y / canvasView.Height));
        }
        private void Brush_Clicked(object sender, EventArgs e)
        {
            ClearButtonsBorder();
            brush.BorderColor = System.Drawing.Color.Gray;
            mode = 1;
        }
        private void Rect_Clicked(object sender, EventArgs e)
        {
            ClearButtonsBorder();
            rect.BorderColor = System.Drawing.Color.Gray;
            mode = 2;
        }
        private void Ellipce_Clicked(object sender, EventArgs e)
        {
            ClearButtonsBorder();
            oval.BorderColor = System.Drawing.Color.Gray;
            mode = 3;
        }
        private void Line_Clicked(object sender, EventArgs e)
        {
            ClearButtonsBorder();
            line.BorderColor = System.Drawing.Color.Gray;
            mode = 4;
        }
        private void Fill_Clicked(object sender, EventArgs e)
        {
            ClearButtonsBorder();
            fill.BorderColor = System.Drawing.Color.Gray;
            mode = 5;
        }
        private void Clear_Clicked(object sender, EventArgs e)
        {
            ClearButtonsBorder();
            brush.BorderColor = System.Drawing.Color.Gray;
            mode = 1;
            canvasView.InvalidateSurface();
            fig.DrawRect(0, 0, 1200, 1600, ppp);
            canvasView.InvalidateSurface();
        }
        private void Red_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            redb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Red;
            slider.ThumbColor = System.Drawing.Color.Red;
        }
        private void White_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            whiteb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.White;
            slider.ThumbColor = System.Drawing.Color.White;
        }
        private void Yellow_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            yellowb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Yellow;
            slider.ThumbColor = System.Drawing.Color.Yellow;
        }
        private void Green_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            greenb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Green;
            slider.ThumbColor = System.Drawing.Color.Green;
        }
        private void Blue_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            blueb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Blue;
            slider.ThumbColor = System.Drawing.Color.Blue;
        }
        private void Black_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            blackb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Black;
            slider.ThumbColor = System.Drawing.Color.Black;
        }
        private void Orange_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            orangeb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Orange;
            slider.ThumbColor = System.Drawing.Color.Orange;
        }
        private void Brown_Clicked(object sender, EventArgs e)
        {
            ClearColorsBorder();
            brownb.BorderColor = System.Drawing.Color.Gray;
            paint.Color = SKColors.Brown;
            slider.ThumbColor = System.Drawing.Color.Brown;
        }
        public static void FloodFillImage(SKBitmap image, int x, int y, SKColor color)
        {
            SKColor srcColor = image.GetPixel(x, y);
            bool[,] hits = new bool[image.Height, image.Width];
            Queue<SKPoint> queue = new Queue<SKPoint>();
            queue.Enqueue(new SKPoint(x, y));
            while (queue.Count != 0)
            {
                SKPoint p = queue.Dequeue();
                if (FloodFillImageDo(image, hits, (int)p.X, (int)p.Y, srcColor, color))
                {
                    queue.Enqueue(new SKPoint(p.X, p.Y - 1));
                    queue.Enqueue(new SKPoint(p.X, p.Y + 1));
                    queue.Enqueue(new SKPoint(p.X - 1, p.Y));
                    queue.Enqueue(new SKPoint(p.X + 1, p.Y));
                }
            }
        }
        private static bool FloodFillImageDo(SKBitmap image, bool[,] hits, int x, int y, SKColor srcColor, SKColor tgtColor)
        {
            if (y < 0) return false;
            if (x < 0) return false;
            if (y > image.Height - 1) return false;
            if (x > image.Width - 1) return false;
            if (hits[y, x]) return false;
            if (image.GetPixel(x, y) != srcColor) return false;
            image.SetPixel(x, y, tgtColor);
            hits[y, x] = true;
            return true;
        }
        private void ClearButtonsBorder()
        {
            brush.BorderColor = System.Drawing.Color.LightGray;
            rect.BorderColor = System.Drawing.Color.LightGray;
            oval.BorderColor = System.Drawing.Color.LightGray;
            line.BorderColor = System.Drawing.Color.LightGray;
            fill.BorderColor = System.Drawing.Color.LightGray;
        }
        private void ClearColorsBorder()
        {
            redb.BorderColor = System.Drawing.Color.Red;
            whiteb.BorderColor = System.Drawing.Color.White;
            yellowb.BorderColor = System.Drawing.Color.Yellow;
            greenb.BorderColor = System.Drawing.Color.Green;
            blueb.BorderColor = System.Drawing.Color.Blue;
            blackb.BorderColor = System.Drawing.Color.Black;
            orangeb.BorderColor = System.Drawing.Color.Orange;
            brownb.BorderColor = System.Drawing.Color.Brown;
        }
        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            double value = e.NewValue;
            paint.StrokeWidth = (int)value;
        }
    }
}
