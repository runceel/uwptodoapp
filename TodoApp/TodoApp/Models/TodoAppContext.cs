using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    class TodoAppContext : INotifyPropertyChanged
    {
        internal static TodoAppContext Current { get; } = new TodoAppContext(); 

        public event PropertyChangedEventHandler PropertyChanged;

        private TodoItemRepository TodoItemRepository { get; } = new TodoItemRepository();

        public ObservableCollection<TodoItem> TodoItems { get; } = new ObservableCollection<TodoItem>();

        public ObservableCollection<TodoItem> DoneItems { get; } = new ObservableCollection<TodoItem>();

        public ObservableCollection<TodoItem> AllItems { get; } = new ObservableCollection<TodoItem>();

        private static readonly PropertyChangedEventArgs AddItemDescriptionPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(AddItemDescription));

        private string addItemDescription;

        public string AddItemDescription
        {
            get { return this.addItemDescription; }
            set
            {
                if (this.addItemDescription == value) { return; }
                this.addItemDescription = value;
                this.PropertyChanged?.Invoke(this, AddItemDescriptionPropertyChangedEventArgs);
            }
        }

        public TodoAppContext()
        {
            this.AllItems.CollectionChanged += this.AllItems_CollectionChanged;
        }

        internal async Task AddTodoItemAsync()
        {
            var todoItem = new TodoItem { Description = this.AddItemDescription };
            this.AllItems.Add(todoItem);
            this.AddItemDescription = "";
            await this.TodoItemRepository.SaveAsync(this.AllItems);
        }

        internal async Task LoadAsync()
        {
            this.AllItems.Clear();
            var items = await this.TodoItemRepository.LoadAsync();
            foreach (var item in items)
            {
                this.AllItems.Add(item);
            }
        }

        internal Task SaveAsync() => this.TodoItemRepository.SaveAsync(this.AllItems);

        internal async Task RemoveAsync(TodoItem item)
        {
            this.AllItems.Remove(item);
            await this.SaveAsync();
        }

        internal void RestoreTodoItem(TodoItem item) => item.Done = false;

        private async void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var item = (TodoItem)sender;
            if (e.PropertyName == nameof(TodoItem.Done))
            {
                if (item.Done)
                {
                    this.TodoItems.Remove(item);
                    this.DoneItems.Add(item);
                }
                else
                {
                    this.TodoItems.Add(item);
                    this.DoneItems.Remove(item);
                }
            }

            await this.SaveAsync();
        }

        private void AllItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        var item = e.NewItems.Cast<TodoItem>().Single();
                        item.PropertyChanged += this.Item_PropertyChanged;
                        if (item.Done)
                        {
                            this.DoneItems.Add(item);
                        }
                        else
                        {
                            this.TodoItems.Add(item);
                        }
                        break;
                    }
                case NotifyCollectionChangedAction.Move:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Remove:
                    {
                        var item = e.OldItems.Cast<TodoItem>().Single();
                        this.DoneItems.Remove(item);
                        this.TodoItems.Remove(item);
                        item.PropertyChanged -= this.Item_PropertyChanged;
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                    throw new NotSupportedException();
                case NotifyCollectionChangedAction.Reset:
                    {
                        this.DoneItems.Clear();
                        this.TodoItems.Clear();
                        break;
                    }
            }
        }

    }
}
