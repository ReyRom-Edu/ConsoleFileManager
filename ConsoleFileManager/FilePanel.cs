using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace ConsoleFileManager
{
    public class FilePanel : FrameView
    {
        public string CurrentPath { get; private set; } = "\\";
        public bool IsSelected => listView.HasFocus;


        private ListView listView;

        public FilePanel(Pos x, Pos y, Dim height, Dim width)
        {
            X = x;
            Y = y;
            Height = height;
            Width = width;
            
            Border.BorderStyle = BorderStyle.Single;

            listView = new ListView()
            {
                Width = Dim.Fill(),
                Height = Dim.Fill(),
            };

            listView.OpenSelectedItem += ListView_OpenSelectedItem;
            listView.Enter += (e) =>
            {
                Border.BorderBrush = Color.White;
            };
            listView.Leave += (e) =>
            {
                Border.BorderBrush = Color.Green;
            };

            Add(listView);
            Refresh();
        }

        private void ListView_OpenSelectedItem(ListViewItemEventArgs obj)
        {
            string selected = obj.Value?.ToString();
            Navigate(selected);
            Refresh();
        }

        private void Navigate(string target)
        {
            if (target == "..")
            {
                CurrentPath = Directory.GetParent(CurrentPath)?.FullName ?? CurrentPath;
            }
            else
            {
                string path = Path.Combine(CurrentPath, target.TrimEnd('/'));
                if (Directory.Exists(path))
                    CurrentPath = path;
            }
        }

        public void Refresh()
        {
            try
            {
                Title = CurrentPath;
                var entries = Directory.GetFileSystemEntries(CurrentPath)
                    .OrderBy(e => e)
                    .Select(e => Path.GetFileName(e) + (Directory.Exists(e) ? "/" : ""))
                    .Prepend("..")
                    .ToList();
                listView.SetSource(entries);
            }
            catch (Exception ex)
            {
                listView.SetSource(new[] {"..", "<Error>" });
            }
        }

        public string GetSelectedFullName()
        {
            return Path.Combine(CurrentPath, listView.SelectedItem >= 0
                ? (string)listView.Source.ToList()[listView.SelectedItem]!
                : "");
        }
    }
}
