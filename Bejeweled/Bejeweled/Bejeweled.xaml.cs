﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Bejeweled
{
    public partial class Bejeweled : UserControl
    {
        /// <summary>
        /// 动画是否正在播放
        /// </summary>
        public bool AnimationIsPlaying { get; set; }
        /// <summary>
        /// 鼠标左键单击坐标
        /// </summary>
        private Point mouseUpBeforePoint;

        /// <summary>
        /// 通过列号和行号获取bijou
        /// </summary>
        /// <param name="Column">列号</param>
        /// <param name="Row">行号</param>
        /// <returns>bijou</returns>
        public bijou this[int Column, int Row]
        {
            get
            {
                return (bijou)this.LayoutRoot.Children[Row * 9 + Column];
            }
            set
            {
                this.LayoutRoot.Children[Row * 9 + Column] = (UIElement)value;
            }
        }

        public Bejeweled()
        {
            InitializeComponent();
            InitGame();
        }
        /// <summary>
        /// 初始化游戏
        /// </summary>
        public void InitGame()
        {
            Random r = new Random();
            for (int row = 0; row < 9; row++)
            {
                for (int column = 0; column < 9; column++)
                {
                    int type = r.Next(1,6);
                    this.LayoutRoot.Children.Add(new bijou(string.Format("Images/{0}.png", type), type, column, row));
                }
            }
        }
        /// <summary>
        /// 调换两个bijou的位置
        /// </summary>
        /// <param name="bijou1">第一个bijou</param>
        /// <param name="bijou2">第二个bijou</param>
        public void ExchangeLocation(bijou bijou1, bijou bijou2)
        {
            this[bijou1.Column, bijou1.Row] = bijou2;
            this[bijou2.Column, bijou2.Row] = bijou1;
            int bijouColumn = bijou1.Column;
            int bijouRow = bijou1.Row;
            bijou1.Column = bijou2.Column;
            bijou1.Row = bijou2.Row;
            bijou2.Column = bijouColumn;
            bijou2.Row = bijouRow;
        }

        /// <summary>
        /// 让第一个bijou的Zindex值比第二个大
        /// </summary>
        /// <param name="addedbijou">被加Zindex值的bijou</param>
        /// <param name="?">参考值</param>
        public void AddingZindexBybijou(bijou addedbijou, bijou referencebijou)
        {
            Canvas.SetZIndex(addedbijou, Canvas.GetZIndex(referencebijou) + 1);
        }

        /// <summary>
        /// 将指定的宝石向上移动一格
        /// </summary>
        /// <param name="bijou">要移动的宝石</param>
        public void Up(bijou bijou)
        {
            var near = GetbijouByDirection(bijou, Direction.Up);
            if (near != null)
            {
                this.ExchangeLocation(bijou, near); //调换两个宝石的位置
                AddingZindexBybijou(bijou, near); //让bijou的Zindex比near大
                bijou.RenderTransform = new TranslateTransform();
                bijou.RenderTransform.SetValue(TranslateTransform.YProperty, 64.0); //实际位置已经往上移了一位,所以往下平移64
                near.RenderTransform = new TranslateTransform();
                near.RenderTransform.SetValue(TranslateTransform.YProperty, -64.0); //实际位置已经往下移了一位,所以网上平移64
                EventHandler eh = new EventHandler((object sender1, EventArgs ea1) =>
                {
                    var da = sender1 as DoubleAnimation;
                    sbAll.Children.Remove(da);
                    bijou.RenderTransform.SetValue(TranslateTransform.YProperty, 0.0); //播放完后还原平移变换
                    near.RenderTransform.SetValue(TranslateTransform.YProperty, 0.0);
                });
                this.AddAnimationToStoryboard(bijou.RenderTransform, "Y", 0.0, TimeSpan.FromMilliseconds(300), eh); //添加动画
                this.AddAnimationToStoryboard(near.RenderTransform, "Y", 0.0, TimeSpan.FromMilliseconds(300), eh); //添加动画
                this.BeginAnimation();
            }
            //bijou.Children.Add(new TextBlock() { Text = "UP" });
        }
        /// <summary>
        /// 将指定的宝石向下移动一格
        /// </summary>
        /// <param name="bijou">要移动的宝石</param>
        public void Down(bijou bijou)
        {
            bijou.Children.Add(new TextBlock() { Text = "down" });
        }
        /// <summary>
        /// 将指定的宝石向左移动一格
        /// </summary>
        /// <param name="bijou">要移动的宝石</param>
        public void Left(bijou bijou)
        {
            bijou.Children.Add(new TextBlock() { Text = "left" });
        }
        /// <summary>
        /// 将指定的宝石向右移动一格
        /// </summary>
        /// <param name="bijou">要移动的宝石</param>
        public void Right(bijou bijou)
        {
            bijou.Children.Add(new TextBlock() { Text = "right" });
        }

        /// <summary>
        /// 获取指定bijou的指定方向的相邻bijou
        /// </summary>
        /// <param name="movedbijou">指定的bijou</param>
        /// <param name="direction">方向</param>
        /// <returns>相邻的bijou</returns>
        public bijou GetbijouByDirection(bijou movedbijou, Direction direction)
        {
            bijou returnbijou = null;
            switch (direction)
            {
                case Direction.Up:
                    if (movedbijou.Row != 0)
                    {
                        returnbijou = this[movedbijou.Column, movedbijou.Row - 1];
                    }
                    break;
                case Direction.Down:
                    if (movedbijou.Row != 8)
                    {
                        returnbijou = this[movedbijou.Column, movedbijou.Row + 1];
                    }
                    break;
                case Direction.Left:
                    if (movedbijou.Column != 0)
                    {
                        returnbijou = this[movedbijou.Column - 1, movedbijou.Row];
                    }
                    break;
                case Direction.Right:
                    if (movedbijou.Column != 8)
                    {
                        returnbijou = this[movedbijou.Column + 1, movedbijou.Row];
                    }
                    break;
            }
            return returnbijou;
        }
        
        /// <summary>
        /// 向Storyboard添加动画
        /// </summary>
        /// <param name="dobj">动画目标</param>
        /// <param name="property">被改变的值</param>
        /// <param name="to">动画完毕后的最终值</param>
        /// <param name="Duration">动画持续时间</param>
        /// <param name="eh">动画的完成事件处理程序</param>
        public void AddAnimationToStoryboard(DependencyObject dobj, string property, double to, TimeSpan Duration, EventHandler eh)
        {
            DoubleAnimation da = new DoubleAnimation();
            Storyboard.SetTarget(da, dobj);
            Storyboard.SetTargetProperty(da, new PropertyPath(property));
            da.To = to;
            da.Duration = new Duration(Duration);
            da.Completed += eh;
            sbAll.Children.Add(da);
        }
        /// <summary>
        /// 开始动画
        /// </summary>
        public void BeginAnimation()
        {
            AnimationIsPlaying = true;
            sbAll.Begin();
        }

        
        /// <summary>
        /// 获得可以被消除的宝石列表
        /// </summary>
        /// <returns>可以被消除的宝石列表,返回值为null表示不可消除</returns>
        public List<bijou> GetErasablebijou()
        {
            return null;
        }
        #region 事件处理程序
        private void LayoutRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(bijou))
            {
                this.mouseUpBeforePoint = e.GetPosition(null);
                bijou tempbijou = (bijou)e.OriginalSource;
                tempbijou.CaptureMouse();
            }
        }

        private void LayoutRoot_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource != null && e.OriginalSource.GetType() == typeof(bijou) && !AnimationIsPlaying)
            {
                bijou movedbijou = (bijou)e.OriginalSource;
                if (movedbijou.isMouseLeftButtonDown)
                {
                    Point mousePoint = e.GetPosition(null);
                    double XCha = mousePoint.X - mouseUpBeforePoint.X;
                    double YCha = mousePoint.Y - mouseUpBeforePoint.Y;
                    double absXCha = Math.Abs(XCha);
                    double absYCha = Math.Abs(YCha);
                    #region 判断移动方向
                    if (absXCha > 10 || absYCha > 10)
                    {
                        if (XCha > 10 && YCha < 10)
                        {
                            if (absXCha > absYCha)
                            {
                                this.Right(movedbijou);
                            }
                            else
                            {
                                this.Up(movedbijou);
                            }
                        }
                        else if (XCha > 10 && YCha > 10)
                        {
                            if (absXCha > absYCha)
                            {
                                this.Right(movedbijou);
                            }
                            else
                            {
                                this.Down(movedbijou);
                            }
                        }
                        else if (XCha < 10 && YCha > 10)
                        {
                            if (absXCha > absYCha)
                            {
                                this.Left(movedbijou);
                            }
                            else
                            {
                                this.Down(movedbijou);
                            }
                        }
                        else if (XCha < 10 && YCha < 10)
                        {
                            if (absXCha > absYCha)
                            {
                                this.Left(movedbijou);
                            }
                            else
                            {
                                this.Up(movedbijou);
                            }
                        }
                    }
                    #endregion
                }
            }
        }

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource != null && e.OriginalSource.GetType() == typeof(bijou))
            {
                bijou tempbijou = (bijou)e.OriginalSource;
                tempbijou.ReleaseMouseCapture();
            }
        }
        #endregion

        private void sbAll_Completed(object sender, EventArgs e)
        {
            Storyboard sb = sender as Storyboard;
            sb.Stop();
            AnimationIsPlaying = false;
        }
    }
    public enum Direction
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
}