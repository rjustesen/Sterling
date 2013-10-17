using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;

/// <summary>
/// Summary description for FileUtils
/// </summary>
public class FileUtils
{
    private static string[] docFiles = {".PDF",".TXT",".RTF"};
    private static string[] imageFiles = {".JPG", ".TIF" };
    private static string[] mediaFiles = { ".WAV", ".WMA", ".MP4",".MPG",".ACC",".MP3"};

    public static bool IsValidFile(string file)
    {
        if (IsDocFile(file))
            return true;
        if (IsImageFile(file))
            return true;
        if (IsMediaFile(file))
            return true;
        return false;
    }

    public static bool IsDocFile(string file)
    {
        return docFiles.FirstOrDefault(x => x.Equals(Path.GetExtension(file).ToUpper())) != null;
    }

    public static bool IsImageFile(string file)
    {
        return imageFiles.FirstOrDefault(x => x.Equals(Path.GetExtension(file).ToUpper())) != null;
    }

    public static bool IsMediaFile(string file)
    {
        return mediaFiles.FirstOrDefault(x => x.Equals(Path.GetExtension(file).ToUpper())) != null;
    }

    public static string SupportedFileFormats()
    {
        StringBuilder sb = new StringBuilder();
        foreach (string s in docFiles)
        {
            sb.Append(s);
            sb.Append(",");
        }
        foreach (string s in imageFiles)
        {
            sb.Append(s);
            sb.Append(",");
        }
        foreach (string s in mediaFiles)
        {
            sb.Append(s);
            sb.Append(",");
        }
        return sb.ToString();
    }

    public static byte[] LoadFile(string filePath)
    {
        FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        BinaryReader reader = new BinaryReader(stream);
        byte[] file = reader.ReadBytes((int)stream.Length);
        reader.Close();
        stream.Close();
        return file;
    }
}
