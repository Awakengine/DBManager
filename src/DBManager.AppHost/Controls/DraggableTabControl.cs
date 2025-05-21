using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace DBManager.App.Controls
{
    public class DraggableTabControl : TabControl
    {
        private TabItem? _draggedItem;
        private int _draggedIndex;
        private bool _isDragging;
        private Point _dragStartPoint;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            
            // 获取点击的TabItem
            var point = e.GetPosition(this);
            var tabItem = GetTabItemFromPoint(point);
            
            if (tabItem != null)
            {
                _draggedItem = tabItem;
                _draggedIndex = Items.IndexOf(_draggedItem.DataContext);
                _dragStartPoint = point;
                _isDragging = true;
                
                // 捕获鼠标
                e.Pointer.Capture(this);
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            
            if (_isDragging)
            {
                var point = e.GetPosition(this);
                
                // 计算拖动距离
                var distance = point.X - _dragStartPoint.X;
                
                // 如果拖动距离足够大，尝试交换位置
                if (Math.Abs(distance) > 10)
                {
                    var tabItem = GetTabItemFromPoint(point);
                    
                    if (tabItem != null && tabItem != _draggedItem)
                    {
                        var newIndex = Items.IndexOf(tabItem.DataContext);
                        
                        // 交换位置
                        if (newIndex != _draggedIndex)
                        {
                            SwapTabItems(_draggedIndex, newIndex);
                            _draggedIndex = newIndex;
                            _dragStartPoint = point;
                        }
                    }
                }
            }
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            
            if (_isDragging)
            {
                _isDragging = false;
                _draggedItem = null;
                
                // 释放鼠标捕获
                e.Pointer.Capture(null);
            }
        }

        private TabItem? GetTabItemFromPoint(Point point)
        {
            // 查找点击位置下的TabItem
            var result = this.InputHitTest(point);
            
            if (result != null)
            {
                // 向上查找TabItem
                // 修正：确保result是Visual类型，然后使用FindAncestorOfType
                if (result is Visual visual)
                {
                    return visual.FindAncestorOfType<TabItem>();
                }
            }
            
            return null;
        }

        private void SwapTabItems(int oldIndex, int newIndex)
        {
            // 获取集合
            var items = Items as System.Collections.IList;
            
            if (items != null)
            {
                // 获取要移动的项
                var item = items[oldIndex];
                
                // 移除并在新位置插入
                items.RemoveAt(oldIndex);
                items.Insert(newIndex, item);
                
                // 选中移动后的项
                SelectedIndex = newIndex;
            }
        }
    }
}
