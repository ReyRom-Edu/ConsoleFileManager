using System;
using System.Collections.Generic;
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
            var dialog = new Dialog("Create file", 60, 10);

            var fileNameField = new TextField("")
            {
                X = 2,
                Y = 1,
                Width = Dim.Fill() - 2
            }; 
            

            var createButton = new Button("Create")
            {
                X = Pos.Left(dialog) + 3,
                Y = Pos.Bottom(dialog) - 3
            };
            var cancelButton = new Button("Cancel")
            {
                X = Pos.Right(dialog) -3,
                Y = Pos.Bottom(dialog) - 3
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


            dialog.Add(fileNameField);
            dialog.Add(cancelButton);
            dialog.Add(createButton);

            Application.Run(dialog);
        }
    }
}
