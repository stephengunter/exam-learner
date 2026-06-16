using Infrastructure.Consts;

namespace Infrastructure.Helpers;
public static class FilesHelpers
{
    public static byte[] ReadFile(string fileName)
    {
        FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        int size = (int)f.Length;
        byte[] data = new byte[size];
        size = f.Read(data, 0, size);
        f.Close();
        return data;
    }
    public static string GetUniqueFileName(string folderPath, string fileName)
   {
      string extension = Path.GetExtension(fileName);
      string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
      string filePath = Path.Combine(folderPath, fileName);
      int count = 1;
      while (File.Exists(filePath))
      {
         fileName = $"{fileNameWithoutExtension}_({count}){extension}";
         filePath = Path.Combine(folderPath, fileName);
         count++;
      }
      return fileName;

   }

   public static FileTypes GetFileType(this string ext)
   {
      return ext.ToLower() switch
      {
         ".jpg" or ".jpeg" or ".png" or ".gif" => FileTypes.Image,
         ".pdf" => FileTypes.Pdf,
         ".doc" or ".docx" => FileTypes.Word,
         ".xls" or ".xlsx" => FileTypes.Excel,
         _ => FileTypes.UnKnown
      };
   }
}
