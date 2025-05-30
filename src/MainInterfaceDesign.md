# 主界面UI布局设计方案

## 1. 整体布局结构

### 1.1 主窗口布局
```
+--------------------------------------------------------------+
| 菜单栏                                                       |
+--------------------------------------------------------------+
| 工具栏                                                       |
+--------------------------------------------------------------+
| |                    |                                      | |
| |                    |                                      | |
| |                    |                                      | |
| |                    |                                      | |
| |   导航面板         |           工作区（多标签页）         | |
| |   (连接树/对象树)  |                                      | |
| |                    |                                      | |
| |                    |                                      | |
| |                    |                                      | |
| |                    |                                      | |
+--------------------------------------------------------------+
| 状态栏                                                       |
+--------------------------------------------------------------+
```

### 1.2 布局比例
- 导航面板宽度：占总宽度的20-25%，可调整
- 工作区宽度：占总宽度的75-80%，可调整
- 状态栏高度：固定，约30像素
- 菜单栏和工具栏：标准高度

## 2. 导航面板设计

### 2.1 连接管理区域
```
+---------------------------+
| + 新建连接 v              |
+---------------------------+
| 连接                    v |
|  ├─ MySQL               ○ |
|  │   └─ localhost       ● |
|  ├─ PostgreSQL          v |
|  │   ├─ dev-server      ○ |
|  │   └─ prod-server     ○ |
|  ├─ SQL Server          v |
|  │   └─ main-db         ○ |
|  └─ Oracle              v |
|      └─ enterprise-db   ○ |
+---------------------------+
```

### 2.2 对象浏览器区域
```
+---------------------------+
| localhost (MySQL)         |
+---------------------------+
| 数据库                  v |
|  ├─ mysql               v |
|  ├─ information_schema  v |
|  └─ myapp               v |
|      ├─ 表              v |
|      │   ├─ users         |
|      │   ├─ products      |
|      │   └─ orders        |
|      ├─ 视图            v |
|      ├─ 函数            v |
|      ├─ 存储过程        v |
|      └─ 触发器          v |
+---------------------------+
```

## 3. 工作区设计

### 3.1 多标签页布局
```
+--------------------------------------------------------------+
| 查询1 | 表:users | 表:products | 查询2 | +                   |
+--------------------------------------------------------------+
|                                                              |
|                                                              |
|                   标签页内容区域                             |
|                                                              |
|                                                              |
+--------------------------------------------------------------+
```

### 3.2 查询编辑器标签页
```
+--------------------------------------------------------------+
| 工具栏: [执行] [停止] [格式化] [保存] [历史]                 |
+--------------------------------------------------------------+
|                                                              |
|                   SQL编辑区域                                |
|                                                              |
+--------------------------------------------------------------+
| 结果 | 消息 | 执行计划 |                                     |
+--------------------------------------------------------------+
|                                                              |
|                   结果显示区域                               |
|                                                              |
+--------------------------------------------------------------+
| 状态: 查询执行时间: 0.03s  影响行数: 5                      |
+--------------------------------------------------------------+
```

### 3.3 表数据标签页
```
+--------------------------------------------------------------+
| 工具栏: [筛选] [排序] [添加] [删除] [刷新] [导出]            |
+--------------------------------------------------------------+
|                                                              |
|  id | name      | email            | created_at     | 操作   |
| ----+-----------+------------------+---------------+-------- |
|  1  | 用户1     | user1@email.com  | 2025-01-01... | 编辑   |
|  2  | 用户2     | user2@email.com  | 2025-01-02... | 编辑   |
|  3  | 用户3     | user3@email.com  | 2025-01-03... | 编辑   |
|                                                              |
+--------------------------------------------------------------+
| 第1-10行，共50行                       << < 1 2 3 4 5 > >>   |
+--------------------------------------------------------------+
```

## 4. 对话框设计

### 4.1 连接设置对话框
```
+--------------------------------------------------------------+
| 新建连接                                                [×]  |
+--------------------------------------------------------------+
| 常规 | SSL | SSH | 高级                                      |
+--------------------------------------------------------------+
|                                                              |
| 连接名称: [                                            ]     |
|                                                              |
| 主机: [                      ]  端口: [      ]               |
|                                                              |
| 用户名: [                    ]                               |
|                                                              |
| 密码: [                      ]  保存密码 [√]                 |
|                                                              |
| 数据库: [                    ]                               |
|                                                              |
|                                                              |
|                                      [测试连接] [取消] [确定]|
+--------------------------------------------------------------+
```

### 4.2 表设计对话框
```
+--------------------------------------------------------------+
| 表设计: users                                           [×]  |
+--------------------------------------------------------------+
| 字段 | 索引 | 外键 | 触发器                                  |
+--------------------------------------------------------------+
|                                                              |
| 名称     | 类型      | 长度 | 非空 | 主键 | 自增 | 默认值    |
| ---------+-----------+------+------+------+------+---------- |
| id       | INT       |      |  √   |  √   |  √   |           |
| name     | VARCHAR   | 255  |  √   |      |      |           |
| email    | VARCHAR   | 255  |  √   |      |      |           |
| created_at| DATETIME |      |  √   |      |      | CURRENT.. |
|           |           |      |      |      |      |           |
|                                                              |
|                                                              |
|                                           [取消] [保存]      |
+--------------------------------------------------------------+
```

## 5. 响应式设计考虑

### 5.1 窗口尺寸变化适应
- 导航面板可折叠，在窗口较窄时提供更多工作区空间
- 工作区内容自适应调整，确保关键操作按钮始终可见
- 表格视图支持水平滚动，确保所有列都可访问

### 5.2 布局断点
- 小屏幕 (<800px): 导航面板可折叠，默认折叠
- 中等屏幕 (800-1200px): 导航面板默认显示，宽度约20%
- 大屏幕 (>1200px): 导航面板默认显示，宽度约25%

## 6. 主题设计

### 6.1 浅色主题
- 背景色: #F5F5F5 (导航面板), #FFFFFF (工作区)
- 前景色: #333333 (主文本), #666666 (次要文本)
- 强调色: #0078D7 (选中项), #E6F2FF (悬停项)
- 边框色: #DDDDDD

### 6.2 深色主题
- 背景色: #252526 (导航面板), #1E1E1E (工作区)
- 前景色: #CCCCCC (主文本), #999999 (次要文本)
- 强调色: #0078D7 (选中项), #1C3D5C (悬停项)
- 边框色: #444444

## 7. 图标设计

### 7.1 数据库对象图标
- 数据库: 圆柱形数据库图标
- 表: 网格表格图标
- 视图: 带眼睛的表格图标
- 存储过程: 齿轮图标
- 函数: f(x)图标
- 触发器: 闪电图标

### 7.2 操作图标
- 新建: 加号图标
- 执行: 播放按钮图标
- 保存: 磁盘图标
- 刷新: 循环箭头图标
- 导出: 向外箭头图标
- 设置: 齿轮图标

## 8. 交互设计

### 8.1 键盘快捷键
- Ctrl+N: 新建查询
- F5: 执行查询
- Ctrl+S: 保存
- Ctrl+Tab: 切换标签页
- F4: 打开/关闭导航面板
- Ctrl+F: 查找

### 8.2 上下文菜单
- 对象树节点右键菜单: 打开、设计、重命名、删除等操作
- 表格数据右键菜单: 编辑、复制、粘贴、删除行等操作
- 查询编辑器右键菜单: 复制、粘贴、注释、格式化等操作

## 9. 实现优先级

1. 主窗口基本布局框架
2. 导航面板和连接管理
3. 多标签页工作区
4. 查询编辑器和结果显示
5. 表格数据编辑视图
6. 对话框和表单
7. 主题切换和响应式适配
