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
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private TodoAppContext TodoAppContext { get; } = TodoAppContext.Current;

        private static readonly PropertyChangedEventArgs AddItemDescriptionPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(AddItemDescription));

        private string addItemDescription;

        /// <summary>
        /// 追加するTodoの概要
        /// </summary>
        public string AddItemDescription
        {
            get { return this.addItemDescription; }
            set
            {
                if (this.addItemDescription == value) { return; }
                this.addItemDescription = value;
                this.PropertyChanged?.Invoke(this, AddItemDescriptionPropertyChangedEventArgs);

                this.CanAdd = !string.IsNullOrWhiteSpace(value);
            }
        }

        private static readonly PropertyChangedEventArgs CanAddPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(CanAdd));

        private bool canAdd;

        /// <summary>
        /// Todoが追加できる状態かどうか
        /// </summary>
        public bool CanAdd
        {
            get { return this.canAdd; }
            private set
            {
                if (this.canAdd == value) { return; }
                this.canAdd = value;
                this.PropertyChanged?.Invoke(this, CanAddPropertyChangedEventArgs);
            }
        }

        public ObservableCollection<TodoItem> TodoItems => this.TodoAppContext.TodoItems;



        public Task LoadAsync()=>  this.TodoAppContext.LoadAsync();

        public async Task AddAsync()
        {
            this.TodoAppContext.AddItemDescription = this.AddItemDescription;
            await this.TodoAppContext.AddTodoItemAsync();
            this.AddItemDescription = "";
        }
    }
}
