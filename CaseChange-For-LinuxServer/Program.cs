using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace POC.TOOLS
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> fileInformtion = new List<string>();
            string applicationPath = @"C:\Users\samraj.milton\Desktop\CDNprod";
            fileInformtion.Add(applicationPath + @"\content");
            fileInformtion.Add(applicationPath + @"\scripts");
            try
            {
                foreach (string fileinfo in fileInformtion)
                {
                    var _directoryInfo = new DirectoryInfo(fileinfo);
                    ParseDirectory(_directoryInfo);
                }
            }
            catch
            {
            }

        }
        static void ParseDirectory(DirectoryInfo directoryInfo)
        {
            foreach (var objInfo in directoryInfo.GetDirectories())
            {
                ParseDirectory(RenameDirectoryToLower(objInfo));
            }
            // Files
            foreach (var objFileInfo in directoryInfo.GetFiles())
            {
                RenameFileToLower(objFileInfo);
            }
        }
        static DirectoryInfo RenameDirectoryToLower(DirectoryInfo dirInfo)
        {
            string strLower = dirInfo.FullName.ToLower();
            string strTmpName = dirInfo.FullName + "_temp";
            dirInfo.MoveTo(strTmpName);
            dirInfo = new DirectoryInfo(strTmpName);
            dirInfo.MoveTo(strLower);
            return new DirectoryInfo(strLower);
        }
        static FileInfo RenameFileToLower(FileInfo fileInfo)
        {
            string strLower = fileInfo.FullName.ToLower();
            string strTmpName = fileInfo.FullName + "_temp";
            fileInfo.MoveTo(strTmpName);
            fileInfo = new FileInfo(strTmpName);
            fileInfo.MoveTo(strLower);
            return new FileInfo(strLower);
        }
    }
}
