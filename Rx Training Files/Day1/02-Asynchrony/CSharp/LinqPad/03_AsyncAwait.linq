<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async void Main()
{
	var sourceDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\resources\DummyData");
	var targetDir = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\resources\DummyData2");
	
	await DirectoryCopy(sourceDir, targetDir);
	await FileCopy(sourceDir, "Index.htm", "Index_copy.htm");
}

// Define other methods and classes here

//Original code from http://msdn.microsoft.com/en-us/library/kztecsys(v=vs.110).aspx (Asynchronous File I/O)
private async Task DirectoryCopy(string startDirectory, string endDirectory)
{
	string.Format("DirectoryCopy({0}, {1})", startDirectory, endDirectory).Dump();
	if(!Directory.Exists(endDirectory))
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
	string.Format("FileCopy({0},{1},{2})", sourceDirectory, sourceFile, targetFile).Dump();
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
    char[] buffer = new char[0x1000]; 
    int numRead; 
    while ((numRead = await source.ReadAsync(buffer, 0, buffer.Length)) != 0) 
    {
        await destination.WriteAsync(buffer, 0, numRead);
    } 
}