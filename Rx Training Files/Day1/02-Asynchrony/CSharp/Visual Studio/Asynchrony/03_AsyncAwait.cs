using System;
using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Asynchrony
{
    [TestFixture]
    public class AsyncAwait
    {
        [Test]
        public async void FileOperationsWith_Async_await()
        {
            var sourceDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\resources\DummyData");
            var targetDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\..\resources\DummyData2");

            await DirectoryCopy(sourceDir, targetDir);
            await FileCopy(sourceDir, "Index.htm", "Index_copy.htm");
            Console.WriteLine("Done");
        }

        private async Task DirectoryCopy(string startDirectory, string endDirectory)
        {
            Console.WriteLine("DirectoryCopy({0}, {1})", startDirectory, endDirectory);
            if (!Directory.Exists(endDirectory))
            {
                Directory.CreateDirectory(endDirectory);
            }
            foreach (string filename in Directory.EnumerateFiles(startDirectory))
            {
                using (FileStream sourceStream = File.Open(filename, FileMode.Open))
                {
                    using (FileStream destinationStream = File.Create(endDirectory + filename.Substring(filename.LastIndexOf('\\'))))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }
                }
            }
        }

        private async Task FileCopy(string sourceDirectory, string sourceFile, string targetFile)
        {
            Console.WriteLine("FileCopy({0},{1},{2})", sourceDirectory, sourceFile, targetFile);
            using (StreamReader sourceReader = File.OpenText(Path.Combine(sourceDirectory, sourceFile)))
            {
                using (StreamWriter destinationWriter = File.CreateText(Path.Combine(sourceDirectory, targetFile)))
                {
                    await CopyFilesAsync(sourceReader, destinationWriter);
                }
            }
        }

        public async Task CopyFilesAsync(StreamReader source, StreamWriter destination)
        {
            var buffer = new char[0x1000];
            int numRead;
            while ((numRead = await source.ReadAsync(buffer, 0, buffer.Length)) != 0)
            {
                await destination.WriteAsync(buffer, 0, numRead);
            }
        }
    }
}
