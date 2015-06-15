using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace TodoApp.Models
{
    class TodoItemRepository
    {
        internal async Task<TodoItem[]> LoadAsync()
        {
            Debug.WriteLine("LoadAsync");
            var file = await this.GetDataFileAsync();
            using (var s = await file.OpenStreamForReadAsync())
            using (var r = new StreamReader(s))
            {
                var data = await r.ReadToEndAsync();
                if (string.IsNullOrWhiteSpace(data)) { return Enumerable.Empty<TodoItem>().ToArray(); }
                return JsonConvert.DeserializeObject<TodoItem[]>(data);
            }
        }

        internal async Task SaveAsync(IEnumerable<TodoItem> todoItems)
        {
            Debug.WriteLine("SaveAsync");
            var data = JsonConvert.SerializeObject(todoItems);

            var file = await this.GetDataFileAsync();
            using (var s = await file.OpenStreamForWriteAsync())
            using (var w = new StreamWriter(s))
            {
                s.SetLength(0);
                await w.WriteLineAsync(data);
            }
        }

        private async Task<StorageFile> GetDataFileAsync()
        {
            var item = (StorageFile)await ApplicationData.Current.LocalFolder.TryGetItemAsync("data.json");
            if (item == null)
            {
                item = await ApplicationData.Current.LocalFolder.CreateFileAsync("data.json");
            }
            return item;
        }
    }
}
