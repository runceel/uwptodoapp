using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Models;

namespace TodoApp.ViewModels
{
    public class DoneItemPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TodoAppContext TodoAppContext { get; } = TodoAppContext.Current;

        public ObservableCollection<TodoItem> DoneItems => this.TodoAppContext.DoneItems;

        private static readonly PropertyChangedEventArgs SelectedTodoItemPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(SelectedTodoItem));

        private TodoItem selectedTodoItem;

        public TodoItem SelectedTodoItem
        {
            get { return this.selectedTodoItem; }
            set
            {
                if (this.selectedTodoItem == value) { return; }
                this.selectedTodoItem = value;
                this.PropertyChanged?.Invoke(this, SelectedTodoItemPropertyChangedEventArgs);

                this.IsSelectedItem = value != null;
            }
        }

        private static readonly PropertyChangedEventArgs IsSelectedItemPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(IsSelectedItem));

        private bool isSelectedItem;

        public bool IsSelectedItem
        {
            get { return this.isSelectedItem; }
            private set
            {
                if (this.isSelectedItem == value) { return; }
                this.isSelectedItem = value;
                this.PropertyChanged?.Invoke(this, IsSelectedItemPropertyChangedEventArgs);
            }
        }

        public Task RemoveAsync()=>  this.TodoAppContext.RemoveAsync(this.SelectedTodoItem);

        public void RestoreTodoItem() => this.TodoAppContext.RestoreTodoItem(this.SelectedTodoItem);

    }
}
