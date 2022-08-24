using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator
{
    public static class Util
    {
        public static void CopyFilesRecursively(this string sourcePath, string targetPath)
        {
            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
            }

            //Copy all the files & Replaces any files with the same name
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public static int CountFirstSpaces(this string t)
        {
            int nbSpaces = 0;
            while (nbSpaces< t.Length && t[nbSpaces] == ' ')
            {
                nbSpaces++;
            }
            return nbSpaces == t.Length? 0 : nbSpaces;
        }

        public static T[] Split<T>(this T[] array, int index, out T[] last)
        {
            last = array.Skip(index).ToArray();
            return array.Take(index).ToArray();
        }

        public static T[] Split<T>(this T[] array, int startIndex, int endIndex, out T[] first, out T[] last)
        {
            first = array.Take(startIndex).ToArray();
            last = array.Skip(endIndex).ToArray();
            return array.Skip(startIndex).Take(endIndex - startIndex).ToArray();
        }

        public static void WriteFile(this StringBuilder lines, string filePath)
        {
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(lines);
                    writer.Flush();
                }
            }
        }
        public static void WriteFile(this IEnumerable<string> lines, string filePath)
        {
            using (FileStream fileStream = File.OpenWrite(filePath))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }
                    writer.Flush();
                }
            }
        }
    }
}
