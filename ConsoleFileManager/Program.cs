using Terminal.Gui;

namespace ConsoleFileManager
{
    class Program
    {


        static void Main()
        {
            Application.Init();

            FilePanel leftPanel;
            FilePanel rightPanel;

            var top = Application.Top;

            int data = 0;

            ColorScheme colorScheme = new ColorScheme()
            {
                Normal = Application.Driver.MakeAttribute(Color.Green, Color.Black),
                Focus = Application.Driver.MakeAttribute(Color.White, Color.Black),
            };

            var window = new Window("Console File Manager")
            {
                X = 1,
                Y = 1,
                Width = Dim.Fill() - 1,
                Height = Dim.Fill() - 1,
                ColorScheme = colorScheme,
            };

            var helpBar = new Label("F3: Open file, F4: Create file, F5: Copy, F6: Move, F8: Delete")
            {
                X = 1,
                Y = Pos.AnchorEnd(1),
                Width = Dim.Fill() - 1,
            };

            leftPanel = new FilePanel(0, 0, Dim.Fill(), Dim.Percent(50));
            rightPanel = new FilePanel(Pos.Percent(50), 0, Dim.Fill(), Dim.Percent(50));

            window.Add(leftPanel);
            window.Add(rightPanel);
            top.Add(helpBar);
            top.Add(window);

            var controller = new FileManagerController();


            top.KeyPress += (args) =>
            {
                FilePanel src, dst;
                switch (args.KeyEvent.Key)
                {

                    case Key.Esc:
                        Exit();
                        args.Handled = true;
                        break;
                    case Key.F3:
                        (src, _) = GetPanels(leftPanel, rightPanel);
                        FileEditor.Open(src.GetSelectedFullName());
                        args.Handled = true;
                        break;
                    case Key.F4:
                        (src, _) = GetPanels(leftPanel, rightPanel);
                        CreateFileDialog.ShowDialog(src.CurrentPath);
                        src.Refresh();
                        args.Handled = true;
                        break;
                    case Key.F5:
                        (src, dst) = GetPanels(leftPanel, rightPanel);
                        controller.Copy(src, dst);
                        args.Handled = true;
                        break;
                    case Key.F6:
                        (src, dst) = GetPanels(leftPanel, rightPanel);
                        controller.Move(src, dst);
                        args.Handled = true;
                        break;
                    case Key.F8:
                        (src, _) = GetPanels(leftPanel, rightPanel);
                        controller.Delete(src);
                        args.Handled = true;
                        break;
                }
            };



            Application.Run();
        }

        private static (FilePanel source, FilePanel target) GetPanels(FilePanel leftPanel, FilePanel rightPanel)
        {
            var src = leftPanel.IsSelected ? leftPanel : rightPanel;
            var dst = leftPanel.IsSelected ? rightPanel : leftPanel;

            return (src, dst);
        }

        static void Exit()
        {
            Application.RequestStop();
            Console.Clear();
        }
    }
}