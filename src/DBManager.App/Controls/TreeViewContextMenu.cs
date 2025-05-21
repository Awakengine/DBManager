using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace DBManager.App.Controls
{
    public class TreeViewContextMenu : TreeView
    {
        private Dictionary<string, ContextMenu> _contextMenus;

        public TreeViewContextMenu()
        {
            _contextMenus = new Dictionary<string, ContextMenu>();
            
            // 初始化默认右键菜单
            InitializeContextMenus();
            
            // 注册右键事件
            PointerPressed += OnTreeViewPointerPressed;
        }

        private void InitializeContextMenus()
        {
            // 连接节点右键菜单
            var connectionMenu = new ContextMenu();
            connectionMenu.Items.Add(new MenuItem { Header = "打开连接" });
            connectionMenu.Items.Add(new MenuItem { Header = "关闭连接" });
            connectionMenu.Items.Add(new MenuItem { Header = "编辑连接" });
            connectionMenu.Items.Add(new MenuItem { Header = "复制连接" });
            connectionMenu.Items.Add(new MenuItem { Header = "删除连接" });
            _contextMenus["Connection"] = connectionMenu;
            
            // 数据库节点右键菜单
            var databaseMenu = new ContextMenu();
            databaseMenu.Items.Add(new MenuItem { Header = "新建查询" });
            databaseMenu.Items.Add(new MenuItem { Header = "刷新" });
            databaseMenu.Items.Add(new MenuItem { Header = "备份数据库" });
            databaseMenu.Items.Add(new MenuItem { Header = "属性" });
            _contextMenus["Database"] = databaseMenu;
            
            // 表节点右键菜单
            var tableMenu = new ContextMenu();
            tableMenu.Items.Add(new MenuItem { Header = "打开表" });
            tableMenu.Items.Add(new MenuItem { Header = "设计表" });
            tableMenu.Items.Add(new MenuItem { Header = "新建表" });
            tableMenu.Items.Add(new MenuItem { Header = "删除表" });
            tableMenu.Items.Add(new MenuItem { Header = "截断表" });
            tableMenu.Items.Add(new MenuItem { Header = "导出数据" });
            tableMenu.Items.Add(new MenuItem { Header = "导入数据" });
            _contextMenus["Table"] = tableMenu;
            
            // 视图节点右键菜单
            var viewMenu = new ContextMenu();
            viewMenu.Items.Add(new MenuItem { Header = "打开视图" });
            viewMenu.Items.Add(new MenuItem { Header = "设计视图" });
            viewMenu.Items.Add(new MenuItem { Header = "新建视图" });
            viewMenu.Items.Add(new MenuItem { Header = "删除视图" });
            _contextMenus["View"] = viewMenu;
            
            // 存储过程节点右键菜单
            var procedureMenu = new ContextMenu();
            procedureMenu.Items.Add(new MenuItem { Header = "执行存储过程" });
            procedureMenu.Items.Add(new MenuItem { Header = "编辑存储过程" });
            procedureMenu.Items.Add(new MenuItem { Header = "新建存储过程" });
            procedureMenu.Items.Add(new MenuItem { Header = "删除存储过程" });
            _contextMenus["Procedure"] = procedureMenu;
        }

        private void OnTreeViewPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            // 检查是否右键点击
            if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                // 获取点击位置下的TreeViewItem
                var point = e.GetPosition(this);
                var result = this.InputHitTest(point);
                
                if (result is Control control)
                {
                    // 查找TreeViewItem
                    var treeViewItem = FindTreeViewItem(control);
                    
                    if (treeViewItem != null)
                    {
                        // 选中该项
                        treeViewItem.IsSelected = true;
                        
                        // 根据节点类型显示对应的右键菜单
                        var nodeType = GetNodeType(treeViewItem.DataContext);
                        
                        if (_contextMenus.TryGetValue(nodeType, out var contextMenu))
                        {
                            // 修正ContextMenu.Open方法调用
                            contextMenu.PlacementTarget = treeViewItem;
                            contextMenu.Open();
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        private TreeViewItem? FindTreeViewItem(Control control)
        {
            while (control != null)
            {
                if (control is TreeViewItem treeViewItem)
                {
                    return treeViewItem;
                }
                
                control = control.Parent as Control;
            }
            
            return null;
        }

        private string GetNodeType(object? dataContext)
        {
            // 根据数据上下文判断节点类型
            // 这里需要根据实际的ViewModel类型进行判断
            
            if (dataContext is ViewModels.ConnectionViewModel)
                return "Connection";
            
            if (dataContext is ViewModels.DatabaseViewModel)
                return "Database";
            
            // 检查数据库对象组类型
            if (dataContext is ViewModels.DatabaseObjectGroupViewModel group)
            {
                if (group.Name == "表")
                    return "Table";
                
                if (group.Name == "视图")
                    return "View";
                
                if (group.Name == "存储过程")
                    return "Procedure";
            }
            
            // 检查数据库对象类型
            if (dataContext is ViewModels.DatabaseObjectViewModel obj)
            {
                // 这里需要根据父节点类型判断
                var parent = FindParentTreeViewItem(obj);
                if (parent?.DataContext is ViewModels.DatabaseObjectGroupViewModel parentGroup)
                {
                    if (parentGroup.Name == "表")
                        return "Table";
                    
                    if (parentGroup.Name == "视图")
                        return "View";
                    
                    if (parentGroup.Name == "存储过程")
                        return "Procedure";
                }
            }
            
            // 默认返回空
            return "";
        }

        private TreeViewItem? FindParentTreeViewItem(object dataContext)
        {
            // 使用VisualTreeHelper替代GetVisualDescendants
            foreach (var item in this.GetVisualChildren())
            {
                if (item is TreeViewItem treeViewItem)
                {
                    if (treeViewItem.DataContext == dataContext)
                    {
                        // 找到后查找其父TreeViewItem
                        var parent = treeViewItem.Parent;
                        while (parent != null)
                        {
                            if (parent is TreeViewItem parentItem)
                            {
                                return parentItem;
                            }
                            
                            parent = parent.Parent;
                        }
                    }
                    
                    // 递归查找子项
                    var childResult = FindChildTreeViewItem(treeViewItem, dataContext);
                    if (childResult != null)
                    {
                        return childResult;
                    }
                }
            }
            
            return null;
        }
        
        private TreeViewItem? FindChildTreeViewItem(TreeViewItem parent, object dataContext)
        {
            foreach (var child in parent.GetVisualChildren())
            {
                if (child is TreeViewItem childItem)
                {
                    if (childItem.DataContext == dataContext)
                    {
                        return parent; // 返回父项
                    }
                    
                    // 递归查找
                    var result = FindChildTreeViewItem(childItem, dataContext);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }
            
            return null;
        }
    }
}
