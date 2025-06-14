using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFileManager
{
    public class FileManagerController
    {
        public void Copy(FilePanel source, FilePanel target)
        {
            var srcFile = source.GetSelectedFullName();

            if (File.Exists(srcFile))
            {
                var dstFile = Path.Combine(target.CurrentPath, Path.GetFileName(srcFile));
                File.Copy(srcFile, dstFile, true);
                target.Refresh();
            }
        }
        public void Move(FilePanel source, FilePanel target)
        {
            var srcFile = source.GetSelectedFullName();

            if (File.Exists(srcFile))
            {
                var dstFile = Path.Combine(target.CurrentPath, Path.GetFileName(srcFile));
                File.Move(srcFile, dstFile, true);
            }
            source.Refresh();
            target.Refresh();
        }

        public void Delete(FilePanel source)
        {
            var srcFile = source.GetSelectedFullName();
            if (File.Exists(srcFile))
            { 
                File.Delete(srcFile);
            }
            else if (Directory.Exists(srcFile))
            {
                Directory.Delete(srcFile, true);
            }
            source.Refresh();
        }
    }
}
