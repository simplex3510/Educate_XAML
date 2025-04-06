using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Exercise1_ToDoList
{
    public class MainViewModel /*: INotifyCollectionChanged*/
    {
        public ObservableCollection<string> ToDoList { get; set; } = new ObservableCollection<string>();

        private string _newToDo;
        public string NewToDo
        {
            get => _newToDo;
            set
            {
                _newToDo = value;
                OnPropertyChanged(nameof(NewToDo));
            }
        }

        public ICommand AddToDoCommand { get; }
        public ICommand RemoveToDoCommand { get; }

        public MainViewModel()
        {
            AddToDoCommand = new Command(AddToDo);
            RemoveToDoCommand = new Command<string>(RemoveToDo);
        }

        private void AddToDo()
        {
            if (!string.IsNullOrWhiteSpace(NewToDo))
            {
                ToDoList.Add(NewToDo);
                NewToDo = string.Empty;
            }
        }

        private void RemoveToDo(string item)
        {
            if (ToDoList.Contains(item))
            {
                ToDoList.Remove(item);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
