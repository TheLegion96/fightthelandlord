﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PathFinderSpace;
using Microsoft.Win32;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MapEditer
{
    /// <summary>
    /// MapEditer.xaml 的交互逻辑
    /// </summary>
    public partial class MapEditerForm : Window
    {
        public int GridSize = 20;

        private int MaxColumn;

        private int MaxRow;

        public byte[,] Matrix;

        private List<Point> changedNotes = new List<Point>();

        private bool isMouseLeftButtonDown;

        private Point StartPoint;

        private Point EndPoint;

        private byte MouseState;  //0表示障碍物,1表示开始点,2表示结束点

        /// <summary>
        /// 是否可以走斜线
        /// </summary>
        private bool diagonals;

        /// <summary>
        /// 当前编辑地图
        /// </summary>
        private Map _map;

        private Map Map
        {
            get
            {
                return _map;
            }
            set
            {
                this._map = value;
                this.LayoutRoot.Width = value.Width;
                this.LayoutRoot.Height = value.Height;
                Init();
                Canvas.SetZIndex(value.MapImage, -1);
                this.LayoutRoot.Children.Add(value.MapImage);
                if (value.Matrix != null)
                    this.Matrix = value.Matrix;
            }
        }

        public MapEditerForm()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            this.LayoutRoot.Children.Clear();
            MaxColumn = (int)this.LayoutRoot.Width / GridSize;
            MaxRow = (int)this.LayoutRoot.Height / GridSize;
            Matrix = new byte[MaxColumn, MaxRow];
        }
        private void RePaintLine()
        {
            RemoveAllLine();
            for (int i = MaxColumn; i >= 0; i--)
            {
                var line = new Line() { X1 = i * GridSize, Y1 = 0, X2 = i * GridSize, Y2 = MaxRow * GridSize };
                line.Stroke = new SolidColorBrush(Colors.Black);
                LayoutRoot.Children.Add(line);
            }
            for (int j = MaxRow; j >= 0; j--)
            {
                var line = new Line() { X1 = 0, Y1 = j * GridSize, X2 = MaxColumn * GridSize, Y2 = j * GridSize };
                line.Stroke = new SolidColorBrush(Colors.Black);
                LayoutRoot.Children.Add(line);
            }
        }

        private void AddRectangle(Color color, int column, int row)
        {
            var rect = new Rectangle() { Width = GridSize, Height = GridSize, Opacity = 50.0, Fill = new SolidColorBrush(color) };
            Canvas.SetLeft(rect, column * GridSize);
            Canvas.SetTop(rect, row * GridSize);
            this.LayoutRoot.Children.Add(rect);
        }

        private void RemoveAllLine()
        {
            var uies = new List<UIElement>();
            foreach (var uie in this.LayoutRoot.Children)
            {
                if (uie.GetType() == typeof(Line))
                {
                    uies.Add((UIElement)uie);
                }
            }
            foreach (var uie in uies)
            {
                this.LayoutRoot.Children.Remove(uie);
            }
        }

        private void RemoveAllRectangle()
        {
            var uies = new List<UIElement>();
            foreach (var uie in this.LayoutRoot.Children)
            {
                if (uie.GetType() == typeof(Rectangle))
                {
                    uies.Add((UIElement)uie);
                }
            }
            foreach (var uie in uies)
            {
                this.LayoutRoot.Children.Remove(uie);
            }
        }

        private void RemoveRectangleByColor(Color color)
        {
            var rects = new List<Rectangle>();
            foreach (var uie in this.LayoutRoot.Children)
            {
                if (uie.GetType() == typeof(Rectangle))
                {
                    var startRect = uie as Rectangle;
                    var scb = (SolidColorBrush)startRect.Fill;
                    if (scb.Color.Equals(color))
                    {
                        rects.Add(startRect);
                    }
                }
            }
            foreach (var rect in rects)
            {
                this.LayoutRoot.Children.Remove(rect);
            }
        }

        private Point GetPointByUIElement(UIElement uie)
        {
            int column = (int)Canvas.GetLeft(uie) / GridSize;
            int row = (int)Canvas.GetTop(uie) / GridSize;
            return new Point(column, row);
        }

        private Point GetPointByPoint(Point p)
        {
            var newPoint = new Point();
            newPoint.X = (int)p.X / GridSize;
            newPoint.Y = (int)p.Y / GridSize;
            return newPoint;
        }

        private void SetStartPoint(int column, int row)
        {
            RemoveRectangleByColor(Colors.Blue);
            AddRectangle(Colors.Blue, column, row);
            this.StartPoint = new Point(column, row);
        }

        private void SetEndPoint(int column, int row)
        {
            RemoveRectangleByColor(Colors.Red);
            AddRectangle(Colors.Red, column, row);
            this.EndPoint = new Point(column, row);
        }

        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.LayoutRoot.CaptureMouse();
            var truePoint = e.GetPosition(this.LayoutRoot);
            var mousePoint = GetPointByPoint(truePoint);
            if (truePoint.X <= this.LayoutRoot.Width && truePoint.Y <= this.LayoutRoot.Height)
            {
                this.isMouseLeftButtonDown = true;
                if (MouseState == 1)
                {
                    SetStartPoint((int)mousePoint.X, (int)mousePoint.Y);
                    return;
                }
                if (MouseState == 2)
                {
                    SetEndPoint((int)mousePoint.X, (int)mousePoint.Y);
                    return;
                }
                ChangeNote((int)mousePoint.X, (int)mousePoint.Y);
                changedNotes.Add(mousePoint);
            }
        }

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.LayoutRoot.ReleaseMouseCapture();
            var truePoint = e.GetPosition(this.LayoutRoot);
            if (truePoint.X <= this.LayoutRoot.Width && truePoint.Y <= this.LayoutRoot.Height)
            {
                this.isMouseLeftButtonDown = false;
                this.changedNotes.Clear();
            }
        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            var truePoint = e.GetPosition(this.LayoutRoot);
            var mousePoint = GetPointByPoint(truePoint);
            if (truePoint.X <= this.LayoutRoot.Width && truePoint.Y <= this.LayoutRoot.Height)
            {
                if (isMouseLeftButtonDown)
                {
                    if (MouseState == 1)
                    {
                        SetStartPoint((int)mousePoint.X, (int)mousePoint.Y);
                        return;
                    }
                    if (MouseState == 2)
                    {
                        SetEndPoint((int)mousePoint.X, (int)mousePoint.Y);
                        return;
                    }
                    bool isChanged = false;
                    int column = (int)mousePoint.X;
                    int row = (int)mousePoint.Y;
                    foreach (var changedNote in changedNotes)
                    {
                        if (changedNote.X == column && changedNote.Y == row)
                        {
                            isChanged = true;
                        }
                        else
                        {
                            isChanged = false;
                        }
                    }
                    if (!isChanged)
                    {
                        ChangeNote(column, row);
                        changedNotes.Add(new Point(column, row));
                    }
                }
            }
        }

        private void ChangeNote(int column, int row)
        {
            try
            {
                if (Matrix[column, row] == 0)
                {
                    Matrix[column, row] = 1;
                    AddRectangle(Colors.Green, column, row);
                }
                else
                {
                    Matrix[column, row] = 0;
                    List<Rectangle> removeRect = new List<Rectangle>();
                    foreach (var uielement in this.LayoutRoot.Children)
                    {
                        if (uielement.GetType() == typeof(System.Windows.Shapes.Rectangle))
                        {
                            var rect = uielement as Rectangle;
                            var point = GetPointByUIElement(rect);
                            if (point.X == column && point.Y == row)
                            {
                                removeRect.Add(rect);
                            }
                        }
                    }
                    foreach (var rect in removeRect)
                    {
                        this.LayoutRoot.Children.Remove(rect);
                    }
                }
            }
            catch (IndexOutOfRangeException iofre)
            {
            }
        }

        private void btnStartPoint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MouseState = 1;
        }

        private void btnEndPoint_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MouseState = 2;
        }

        private void btnImpediment_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.MouseState = 0;
        }

        private void btnUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (tbGridSize.Text.Trim() != "")
            {
                try
                {
                    this.GridSize = int.Parse(tbGridSize.Text.Trim());
                }
                catch
                {
                }
            }
            Clear();
            Init();
        }

        private void btnStartFindPath_Click(object sender, RoutedEventArgs e)
        {
            RemoveRectangleByColor(Colors.Yellow);
            var pathFinder = new PathFinder(Matrix, StartPoint, EndPoint, diagonals);
            var time1 = DateTime.Now;
            var pathPoints = pathFinder.StartFindPath();
            var time2 = DateTime.Now;
            if (pathPoints == null)
            {
                MessageBox.Show("找不到路径");
                return;
            }
            foreach (var p in pathPoints)
            {
                AddRectangle(Colors.Yellow, (int)p.X, (int)p.Y);
            }
            tbTim.Text = (time2 - time1).ToString();
        }

        private void btnClear_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Clear();
            Init();
        }

        private void Clear()
        {
            var uies = new List<UIElement>();
            foreach (var uie in this.LayoutRoot.Children)
            {
                if (uie.GetType() == typeof(Line) || uie.GetType() == typeof(Rectangle))
                {
                    uies.Add((UIElement)uie);
                }
            }
            foreach (var uie in uies)
            {
                this.LayoutRoot.Children.Remove(uie);
            }
        }

        private void cbDiagonals_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.diagonals = true;
        }

        private void cbDiagonals_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            this.diagonals = false;
        }

        private void OpenMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenMap();
            RefreshMatrix();
        }

        private void NewMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void SaveMenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.Map != null)
            {
                var binaryFormatter = new BinaryFormatter();
                var fs = new FileStream(Map.Path + ".map", FileMode.Create, FileAccess.Write);
                binaryFormatter.Serialize(fs, this.Map);
                fs.Close();
            }
        }

        private void btnOpenImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OpenImage();
        }

        private void OpenImage()
        {
            var openFile = new OpenFileDialog();
            if ((bool)openFile.ShowDialog())
            {
                var bitmapImage = new BitmapImage(new Uri(openFile.FileName));
                var imageMap = new Image();
                imageMap.Source = bitmapImage;
                this.Map = new Map(imageMap, bitmapImage.Width, bitmapImage.Height, openFile.FileName);
                this.Map.Matrix = this.Matrix;
                RePaintLine();
            }
        }

        private Image GetImageByPath(string imagePath)
        {
            var bitmapImage = new BitmapImage(new Uri(imagePath));
            var image = new Image();
            image.Source = bitmapImage;
            return image;
        }

        private void OpenMap()
        {
            var openFile = new OpenFileDialog();
            if ((bool)openFile.ShowDialog())
            {
                var fileStream = new FileStream(openFile.FileName, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                var readedMap = (Map)bf.Deserialize(fileStream);
                readedMap.MapImage = GetImageByPath(readedMap.Path);
                this.Map = readedMap;
                fileStream.Close();
                RePaintLine();
            }
        }

        private void RefreshMatrix()
        {
            for (int x = 0; x < MaxColumn; x++)
            {
                for (int y = 0; y < MaxRow; y++)
                {
                    if (this.Matrix[x, y] == 1)
                    {
                        AddRectangle(Colors.Green, x, y);
                    }
                }
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)e.AddedItems[0];
            if ((string)selectedItem.Content == "100%")
            {
                SetScaleTransform(1F);
            }
            if ((string)selectedItem.Content == "75%")
            {
                SetScaleTransform(0.75F);
            }
            if ((string)selectedItem.Content == "50%")
            {
                SetScaleTransform(0.5F);
            }
            if ((string)selectedItem.Content == "25%")
            {
                SetScaleTransform(0.25F);
            }
            RePaintLine();
            RefreshMatrix();
        }

        private void SetScaleTransform(float i)
        {
            if (i <= 1F)
            {
                GridSize = (int)(20.0F * i);
                //Map.MapImage.RenderTransform = new ScaleTransform();
                //Map.MapImage.RenderTransform.SetValue(ScaleTransform.ScaleXProperty, (double)i);
                //Map.MapImage.RenderTransform.SetValue(ScaleTransform.ScaleYProperty, (double)i);
                Map.MapImage.Width = Map.Width * i;
                Map.MapImage.Height = Map.Height * i;
                UpDateRectangle();
            }
        }

        private void UpDateRectangle()
        {
            RemoveAllRectangle();
            RefreshMatrix();
        }
    }
}