using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAppRxProp.Models;

namespace TodoAppRxProp.ViewModels
{
    public class MainPageViewModel : IDisposable
    {
        private readonly CompositeDisposable disposable = new CompositeDisposable();

        private TodoAppContext TodoAppContext { get; } = TodoAppContext.Current;
        
        public ReadOnlyReactiveCollection<TodoItemViewModel> TodoItems { get; private set; }

        public ReactiveProperty<string> Description { get; private set; }

        public ReactiveCommand AddTodoItemCommand { get; private set; }

        public MainPageViewModel()
        {
            this.TodoItems = this.TodoAppContext
                .TodoItems
                .ToReadOnlyReactiveCollection(
                    this.TodoAppContext.TodoItems.ToCollectionChanged<TodoItem>(),
                    x => new TodoItemViewModel(x))
                .AddTo(this.disposable);

            this.Description = this.TodoAppContext
                .ToReactivePropertyAsSynchronized(x => x.AddItemDescription, ignoreValidationErrorValue: true)
                .SetValidateNotifyError(x => string.IsNullOrWhiteSpace(x) ? "Error" : null)
                .AddTo(this.disposable);

            this.AddTodoItemCommand = this.Description
                .ObserveHasErrors
                .Select(x => !x)
                .ToReactiveCommand();
            this.AddTodoItemCommand.Subscribe(async _ => await this.TodoAppContext.AddTodoItemAsync())
                .AddTo(this.disposable);
        }

        public Task LoadAsync() => this.TodoAppContext.LoadAsync();

        public void Dispose()
        {
            this.disposable.Dispose();
        }
    }
}
