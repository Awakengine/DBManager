using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Avalonia.Media;

namespace DBManager.App.ViewModels
{
    public class ConnectionViewModel : ViewModelBase
    {
        private string _name = string.Empty;
        private string _host = string.Empty;
        private int _port;
        private string _username = string.Empty;
        private string _password = string.Empty;
        private bool _isConnected;
        private ObservableCollection<DatabaseViewModel> _databases = new();
        private IBrush _statusColor = Brushes.Gray;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Host
        {
            get => _host;
            set => SetProperty(ref _host, value);
        }

        public int Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (SetProperty(ref _isConnected, value))
                {
                    StatusColor = value ? Brushes.Green : Brushes.Gray;
                }
            }
        }

        public ObservableCollection<DatabaseViewModel> Databases
        {
            get => _databases;
            set => SetProperty(ref _databases, value);
        }

        public IBrush StatusColor
        {
            get => _statusColor;
            set => SetProperty(ref _statusColor, value);
        }
    }
}
