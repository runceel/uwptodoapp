using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Models
{
    public class TodoItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Id { get; } = Guid.NewGuid().ToString();

        private static readonly PropertyChangedEventArgs DescriptionPropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Description));

        private string description;

        public string Description
        {
            get { return this.description; }
            set
            {
                if (this.description == value) { return; }
                this.description = value;
                this.PropertyChanged?.Invoke(this, DescriptionPropertyChangedEventArgs);
            }
        }


        private static readonly PropertyChangedEventArgs DonePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Done));

        private bool done;

        public bool Done
        {
            get { return this.done; }
            set
            {
                if (this.done == value) { return; }
                this.done = value;
                this.PropertyChanged?.Invoke(this, DonePropertyChangedEventArgs);
            }
        }


    }
}
