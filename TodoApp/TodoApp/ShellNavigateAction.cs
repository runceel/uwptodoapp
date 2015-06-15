using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Views;
using Windows.UI.Xaml;

namespace TodoApp
{
    public class ShellNavigateAction : DependencyObject, IAction
    {
        public string NavigatePageType
        {
            get { return (string)GetValue(NavigatePageTypeProperty); }
            set { SetValue(NavigatePageTypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NavigatePageType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NavigatePageTypeProperty =
            DependencyProperty.Register("NavigatePageType", typeof(string), typeof(ShellNavigateAction), new PropertyMetadata(null));

        public object Execute(object sender, object parameter)
        {
            var shell = Window.Current.Content as Shell;
            shell.RootFrame.Navigate(Type.GetType(this.NavigatePageType));
            return null;
        }
    }
}
