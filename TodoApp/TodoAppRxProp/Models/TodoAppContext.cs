using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Reactive.Bindings.Helpers;
using Reactive.Bindings.Extensions;

namespace TodoAppRxProp.Models
{
    class TodoAppContext : INotifyPropertyChanged
    {
        internal static TodoAppContext Current { get; } = new TodoAppContext(); 

        public event PropertyChangedEventHandler PropertyChanged;

        private TodoItemRepository TodoItemRepository { get; } = new TodoItemRepository();

        private readonly IFilteredReadOnlyObservableCollection<TodoItem> todoItems;

        public IFilteredReadOnlyObservableCollection<TodoItem> TodoItems { get { return this.todoItems; } }

        private readonly IFilteredReadOnlyObservableCollection<TodoItem> doneItems;

        public IFilteredReadOnlyObservableCollection<TodoItem> DoneItems { get { return this.doneItems; } }

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
            this.doneItems = this.AllItems
                .ToFilteredReadOnlyObservableCollection(x => x.Done);

            this.todoItems = this.AllItems
                .ToFilteredReadOnlyObservableCollection(x => !x.Done);

            this.AllItems
                .ObserveElementProperty(x => x.Done, isPushCurrentValueAtFirst: false)
                .Subscribe(async _ => await this.SaveAsync());
        }

        internal async Task AddTodoItemAsync()
        {
            var todoItem = new TodoItem { Description = this.AddItemDescription };
            this.AllItems.Add(todoItem);
            this.AddItemDescription = "";
            await this.SaveAsync();
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

        internal Task SaveAsync() => this.TodoItemRepository.SaveAsync(this.AllItems.ToArray());

        internal async Task RemoveAsync(TodoItem item)
        {
            this.AllItems.Remove(item);
            await this.SaveAsync();
        }

        internal void RestoreTodoItem(TodoItem item) => item.Done = false;

    }
}
