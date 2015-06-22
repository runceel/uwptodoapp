using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Linq;
using TodoAppRxProp.Models;

namespace TodoAppRxProp.ViewModels
{
    public class DoneItemPageViewModel
    {
        private TodoAppContext TodoAppContext { get; } = TodoAppContext.Current;

        public ReadOnlyReactiveCollection<TodoItemViewModel> DoneItems { get; private set; }

        public ReactiveProperty<TodoItemViewModel> SelectedTodoItem { get; private set; }

        public ReactiveCommand RestoreCommand { get; private set; }

        public ReactiveCommand RemoveCommand { get; private set; }

        public DoneItemPageViewModel()
        {
            this.DoneItems = this.TodoAppContext
                .DoneItems
                .ToReadOnlyReactiveCollection(
                    this.TodoAppContext.DoneItems.ToCollectionChanged<TodoItem>(),
                    x => new TodoItemViewModel(x));

            this.SelectedTodoItem = new ReactiveProperty<TodoItemViewModel>();

            this.RestoreCommand = this.SelectedTodoItem
                .Select(x => x != null)
                .ToReactiveCommand();
            this.RestoreCommand.Subscribe(_ => this.TodoAppContext.RestoreTodoItem(this.SelectedTodoItem.Value.Model));

            this.RemoveCommand = this.SelectedTodoItem
                .Select(x => x != null)
                .ToReactiveCommand();
            this.RemoveCommand.Subscribe(async _ => await this.TodoAppContext.RemoveAsync(this.SelectedTodoItem.Value.Model));

        }
    }
}
