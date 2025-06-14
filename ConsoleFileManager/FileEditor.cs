using Terminal.Gui;

namespace ConsoleFileManager
{
    public static class FileEditor
    {
        public static void Open(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.ErrorQuery("Error", "File not found", "Ok");
                return;
            }

            string originalText = File.ReadAllText(filePath);

            ColorScheme colorScheme = new ColorScheme()
            {
                Normal = Application.Driver.MakeAttribute(Color.Green, Color.Black),
                Focus = Application.Driver.MakeAttribute(Color.White, Color.Black),
            };

            var editor = new Window($"Editor: {Path.GetFileName(filePath)}")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 1,
                Height = Dim.Fill() - 1,
                ColorScheme = colorScheme,
            };

            var textView = new TextView()
            {
                Text = originalText,
                X = 0,
                Y = 0,
                Width = Dim.Fill(),
                Height = Dim.Fill(),
                WordWrap = true,
            };

            var statusBar = new Label()
            {
                X = 1,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill() - 1,
                ColorScheme = Colors.Menu,
            };

            void UpdateStatus()
            {
                statusBar.Text = $"F2: Save, F4: Revert, Esc: Exit | Chars Count: {textView.Text.Length}";
            }

            textView.KeyPress += (_) => UpdateStatus();

            UpdateStatus();

            editor.Add(textView);

            editor.KeyPress += (args) =>
            {
                switch (args.KeyEvent.Key)
                {
                    case Key.F2:
                        try
                        {
                            originalText = SaveFile(filePath, textView);
                            MessageBox.Query("Saved", "File saved succeessfelly", "Ok");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.ErrorQuery("Error", ex.Message, "Ok");
                        }
                        args.Handled = true;
                        break;

                    case Key.F4:
                        textView.Text = originalText;
                        args.Handled = true;
                        break;
                    case Key.Esc:
                        if (textView.Text.ToString() != originalText)
                        {
                            var answer = MessageBox.Query("Unsaved", "You have unsaved changes. Save before exiting?", "Yes", "No", "Cancel");

                            if (answer == 0)
                            {
                                try
                                {
                                    SaveFile(filePath, textView);
                                    Application.RequestStop();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.ErrorQuery("Error", ex.Message, "Ok");
                                }
                            }
                            else if (answer == 1)
                            {
                                Application.RequestStop();
                            }
                            // Cancel - do nothing
                        }
                        else
                        {
                            Application.RequestStop();
                        }
                        args.Handled = true;
                        break;
                }
            };


            var editorTop = new Toplevel();
            editorTop.Add(editor);
            editorTop.Add(statusBar);

            Application.Run(editorTop);
        }



        private static string SaveFile(string filePath, TextView textView)
        {
            string originalText = textView.Text.ToString();
            File.WriteAllText(filePath, originalText);
            return originalText;
        }
    }
}
