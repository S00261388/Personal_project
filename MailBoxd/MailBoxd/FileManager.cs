using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MailBoxd
{
    public class FileManager
    {
        private string filePath;

        public FileManager(string path)
        {
            filePath = path;
        }

        public bool CanWriteToFile()
        {
            try
            {
                using (var fileStream = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fileStream.Close();
                }
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}
