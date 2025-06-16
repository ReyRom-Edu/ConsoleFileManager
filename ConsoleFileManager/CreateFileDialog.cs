using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace ConsoleFileManager
{
    public static class CreateFileDialog
    {
        public static void ShowDialog(string directory)
        {
            var dialog = new Dialog("Create file", 40, 8);


            var fileNameLabel = new Label("Enter filename")
            {
                X = 2,
                Y = 1,
            };


            var fileNameField = new TextField("")
            {
                X = 2,
                Y = 2,
                Width = Dim.Fill() - 2
            }; 
            

            var createButton = new Button("Create")
            {
                X = 2,
                Y = 5
            };
            var cancelButton = new Button("Cancel")
            {
                X = 12,
                Y = 5
            };

            createButton.Clicked += () =>
            {
                var filename = fileNameField.Text.ToString();
                if (String.IsNullOrEmpty(filename))
                {
                    MessageBox.ErrorQuery("Error", "FileName must be not empty", "Ok");
                    return;
                }

                try
                {
                    var path = Path.Combine(directory, filename);
                    if (File.Exists(path))
                    {
                        MessageBox.ErrorQuery("Error", "FileName already exists", "Ok");
                        return;
                    }

                    File.Create(path).Close();


                    Application.RequestStop();
                }
                catch (Exception ex) 
                {
                    MessageBox.ErrorQuery("Error", ex.Message, "Ok");
                }
            };

            cancelButton.Clicked += () => Application.RequestStop();

            dialog.Add(fileNameLabel);
            dialog.Add(fileNameField);
            dialog.Add(createButton);
            dialog.Add(cancelButton);
            

            Application.Run(dialog);
        }
    }
}
