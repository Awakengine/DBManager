using System;
using System.Collections.ObjectModel;
using Avalonia.Media;

namespace DBManager.App.ViewModels
{
    // 数据库视图模型
    public class DatabaseViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _owner = string.Empty;
        private long _size;
        private DateTime _createdDate;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        public string Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }
        
        public long Size
        {
            get => _size;
            set => SetProperty(ref _size, value);
        }
        
        public DateTime CreatedDate
        {
            get => _createdDate;
            set => SetProperty(ref _createdDate, value);
        }
        
        public ObservableCollection<DatabaseObjectGroupViewModel> ObjectGroups { get; } = new();
    }
    
    // 数据库对象组视图模型（如表、视图、存储过程等分组）
    public class DatabaseObjectGroupViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _icon = string.Empty;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        public string Icon
        {
            get => _icon;
            set => SetProperty(ref _icon, value);
        }
        
        public ObservableCollection<DatabaseObjectViewModel> Objects { get; } = new();
    }
    
    // 数据库对象视图模型（如具体的表、视图、存储过程等）
    public class DatabaseObjectViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _type = string.Empty;
        private string _schema = string.Empty;
        private DateTime _modifiedDate;
        
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }
        
        public string Schema
        {
            get => _schema;
            set => SetProperty(ref _schema, value);
        }
        
        public DateTime ModifiedDate
        {
            get => _modifiedDate;
            set => SetProperty(ref _modifiedDate, value);
        }
    }
}
