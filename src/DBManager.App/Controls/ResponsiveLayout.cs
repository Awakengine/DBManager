using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;

namespace DBManager.App.Controls
{
    public class ResponsiveLayout : Panel
    {
        // 定义断点
        private const double SmallScreenWidth = 800;
        private const double MediumScreenWidth = 1200;
        
        // 定义导航面板默认宽度比例
        private const double SmallScreenNavRatio = 0.0; // 小屏幕默认隐藏
        private const double MediumScreenNavRatio = 0.2; // 中等屏幕20%
        private const double LargeScreenNavRatio = 0.25; // 大屏幕25%
        
        // 当前导航面板是否可见
        public static readonly StyledProperty<bool> IsNavigationPaneVisibleProperty =
            AvaloniaProperty.Register<ResponsiveLayout, bool>(
                nameof(IsNavigationPaneVisible), true);
        
        public bool IsNavigationPaneVisible
        {
            get => GetValue(IsNavigationPaneVisibleProperty);
            set => SetValue(IsNavigationPaneVisibleProperty, value);
        }
        
        // 导航面板控件
        public static readonly StyledProperty<Control> NavigationPaneProperty =
            AvaloniaProperty.Register<ResponsiveLayout, Control>(
                nameof(NavigationPane));
        
        public Control NavigationPane
        {
            get => GetValue(NavigationPaneProperty);
            set => SetValue(NavigationPaneProperty, value);
        }
        
        // 内容区控件
        public static readonly StyledProperty<Control> ContentAreaProperty =
            AvaloniaProperty.Register<ResponsiveLayout, Control>(
                nameof(ContentArea));
        
        public Control ContentArea
        {
            get => GetValue(ContentAreaProperty);
            set => SetValue(ContentAreaProperty, value);
        }
        
        // 分隔条宽度
        public static readonly StyledProperty<double> SplitterWidthProperty =
            AvaloniaProperty.Register<ResponsiveLayout, double>(
                nameof(SplitterWidth), 5.0);
        
        public double SplitterWidth
        {
            get => GetValue(SplitterWidthProperty);
            set => SetValue(SplitterWidthProperty, value);
        }
        
        // 分隔条颜色
        public static readonly StyledProperty<IBrush> SplitterBrushProperty =
            AvaloniaProperty.Register<ResponsiveLayout, IBrush>(
                nameof(SplitterBrush), new SolidColorBrush(Colors.LightGray));
        
        public IBrush SplitterBrush
        {
            get => GetValue(SplitterBrushProperty);
            set => SetValue(SplitterBrushProperty, value);
        }
        
        // 导航面板宽度
        private double _navigationPaneWidth;
        
        // 构造函数
        public ResponsiveLayout()
        {
            // 完全重构事件订阅方式，使用属性变更事件而不是GetObservable
            PropertyChanged += OnPropertyChanged;
            
            // 监听布局更新
            this.LayoutUpdated += OnLayoutUpdated;
            
            // 初始化布局
            UpdateLayoutInternal();
        }
        
        private void OnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == IsNavigationPaneVisibleProperty ||
                e.Property == NavigationPaneProperty ||
                e.Property == ContentAreaProperty ||
                e.Property == SplitterWidthProperty)
            {
                InvalidateMeasure();
            }
            else if (e.Property == BoundsProperty)
            {
                UpdateLayoutInternal();
            }
        }
        
        private void OnLayoutUpdated(object? sender, EventArgs e)
        {
            DrawSplitter();
        }
        
        // 更新布局 - 重命名以避免与基类方法冲突
        private void UpdateLayoutInternal()
        {
            var width = Bounds.Width;
            
            // 根据屏幕宽度确定导航面板宽度比例
            double ratio;
            if (width < SmallScreenWidth)
            {
                ratio = SmallScreenNavRatio;
            }
            else if (width < MediumScreenWidth)
            {
                ratio = MediumScreenNavRatio;
            }
            else
            {
                ratio = LargeScreenNavRatio;
            }
            
            // 计算导航面板宽度
            _navigationPaneWidth = width * ratio;
            
            // 如果导航面板不可见，则宽度为0
            if (!IsNavigationPaneVisible)
            {
                _navigationPaneWidth = 0;
            }
            
            // 重新测量和排列
            InvalidateMeasure();
        }
        
        protected override Size MeasureOverride(Size availableSize)
        {
            if (NavigationPane == null || ContentArea == null)
            {
                return availableSize;
            }
            
            // 更新导航面板宽度
            UpdateLayoutInternal();
            
            // 测量导航面板
            if (IsNavigationPaneVisible && _navigationPaneWidth > 0)
            {
                NavigationPane.Measure(new Size(_navigationPaneWidth, availableSize.Height));
            }
            
            // 测量内容区
            double contentWidth = availableSize.Width - _navigationPaneWidth;
            if (IsNavigationPaneVisible && _navigationPaneWidth > 0)
            {
                contentWidth -= SplitterWidth;
            }
            
            ContentArea.Measure(new Size(contentWidth, availableSize.Height));
            
            return availableSize;
        }
        
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (NavigationPane == null || ContentArea == null)
            {
                return finalSize;
            }
            
            // 排列导航面板
            if (IsNavigationPaneVisible && _navigationPaneWidth > 0)
            {
                NavigationPane.Arrange(new Rect(0, 0, _navigationPaneWidth, finalSize.Height));
            }
            
            // 排列内容区
            double contentX = _navigationPaneWidth;
            if (IsNavigationPaneVisible && _navigationPaneWidth > 0)
            {
                contentX += SplitterWidth;
            }
            
            double contentWidth = finalSize.Width - contentX;
            ContentArea.Arrange(new Rect(contentX, 0, contentWidth, finalSize.Height));
            
            return finalSize;
        }
        
        // 使用自定义方法绘制分隔条，而不是重写OnRender
        private void DrawSplitter()
        {
            // 在布局更新时通过添加子元素的方式绘制分隔条
            if (IsNavigationPaneVisible && _navigationPaneWidth > 0)
            {
                // 检查是否已有分隔条
                var existingSplitter = this.FindControl<Border>("Splitter");
                
                if (existingSplitter == null)
                {
                    // 创建分隔条
                    var splitter = new Border
                    {
                        Name = "Splitter",
                        Background = SplitterBrush,
                        Width = SplitterWidth,
                        Height = Bounds.Height,
                        [Canvas.LeftProperty] = _navigationPaneWidth
                    };
                    
                    // 添加到子元素
                    if (!this.Children.Contains(splitter))
                    {
                        this.Children.Add(splitter);
                    }
                }
                else
                {
                    // 更新分隔条位置和大小
                    existingSplitter.Width = SplitterWidth;
                    existingSplitter.Height = Bounds.Height;
                    existingSplitter.SetValue(Canvas.LeftProperty, _navigationPaneWidth);
                }
            }
            else
            {
                // 移除分隔条
                var existingSplitter = this.FindControl<Border>("Splitter");
                if (existingSplitter != null)
                {
                    this.Children.Remove(existingSplitter);
                }
            }
        }
    }
}
