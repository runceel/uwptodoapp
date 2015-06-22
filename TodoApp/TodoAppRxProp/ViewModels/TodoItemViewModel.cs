using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoAppRxProp.Models;
using System.Reactive.Linq;
using Reactive.Bindings.Extensions;

namespace TodoAppRxProp.ViewModels
{
    public class TodoItemViewModel
    {
        public TodoItem Model { get; }

        public ReadOnlyReactiveProperty<string> Description { get; private set; }
        public ReactiveProperty<bool> Done { get; private set; }

        public TodoItemViewModel(TodoItem model)
        {
            this.Model = model;

            this.Description = model
                .ObserveProperty(x => x.Description)
                .ToReadOnlyReactiveProperty();

            this.Done = model
                .ToReactivePropertyAsSynchronized(x => x.Done);
        }
    }
}
