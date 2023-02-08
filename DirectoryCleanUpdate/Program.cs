using System;
using System.IO;

namespace DirectoryCleanUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите путь до папки: ");
            string path = Console.ReadLine();
            Console.WriteLine();
            int fileCount = 0;
            int folderCount = 0;

            try
            {
                if (Directory.Exists(path))
                {
                    var sizeBeforeDeleting = GetSize(path, default);

                    string[] files = Directory.GetFiles(path);

                    foreach (var item in files)
                    {
                        FileInfo fileInfo = new FileInfo(item);
                        if (DateTime.Now - TimeSpan.FromMinutes(30) > fileInfo.LastWriteTime)
                        {
                            fileInfo.Delete();
                            fileCount++;
                        }
                    }

                    string[] dirs = Directory.GetDirectories(path);

                    foreach (var item in dirs)
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(item);
                        if (DateTime.Now - TimeSpan.FromMinutes(30) > directoryInfo.LastWriteTime)
                        {
                            directoryInfo.Delete(true);
                            folderCount++;
                        }
                    }

                    Console.WriteLine("Произведено удаление");
                    Console.WriteLine();

                    var sizeAfterDeleting = GetSize(path, default);

                    Console.WriteLine($"Исходный размер папки составлял: {sizeBeforeDeleting} байт");
                    Console.WriteLine($"Освобождено:  {sizeBeforeDeleting - sizeAfterDeleting} байт");
                    Console.WriteLine($"Текущий размер папки составляет: {sizeAfterDeleting} байт");
                    Console.WriteLine($"Было удалено файлов: {fileCount}, папок с вложенными файлами: {folderCount}");

                }
                else
                {
                    Console.WriteLine("Ошибка: директория не найдена");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine("Ошибка: " + exception);
            }
        }

        static double GetSize(string path, double size)
        {
            string[] dirs = Directory.GetDirectories(path);

            if (dirs.Length != 0)
            {
                foreach (var item in dirs)
                {
                    GetSize(item, size);
                }
            }

            var files = Directory.GetFiles(path);

            Console.WriteLine($"Директория: {path} содержит {files.Length} файлов");

            foreach (var file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                size += fileInfo.Length;
                Console.WriteLine($"Размер файла {fileInfo.Name} составляет {fileInfo.Length} байт");
            }

            Console.WriteLine();

            return size;
        }
    }
}
