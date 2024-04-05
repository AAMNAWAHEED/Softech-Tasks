using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Reflection.Metadata;
namespace azureblob
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var connstring = "DefaultEndpointsProtocol=https;AccountName=blobstoragetask123;AccountKey=X1j9LLAqTwr0703FqHpCaOeZdy27x2mvu9yulgFHAmOzdHis+DkVmbufd4HELPtXh8sWczLY9MvG+AStXR3Kpw==;EndpointSuffix=core.windows.net";
            var containername = "chunkfileupload";
            var container = new BlobContainerClient(connstring, containername);


            int i = 0;

            int chunksize = 1 * 1024 * 1024;

            using (var stream = File.OpenRead("file.mp4"))
            {
                
                long fileSize = stream.Length;
                long uploadedBytes = 0;
                while (uploadedBytes < fileSize)
                {
                    var blob = container.GetBlobClient($"file{i}.mp4");  //generating a blob client
                    Console.WriteLine(blob);
                    
                    var remainingBytes = fileSize - uploadedBytes;
                    var bytesToRead = Math.Min(chunksize, remainingBytes);  //1-cunk size  2-chunck size  3-remaining bytes

                    byte[] buffer = new byte[bytesToRead];  //data will be stored at here
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);  //no of bytes read from stream
                    
                    using (var memoryStream = new MemoryStream(buffer, 0, bytesRead))  //(data array,starts from,end till)
                    {
                        await blob.UploadAsync(memoryStream, true);
                       
                        uploadedBytes += bytesRead;
                        Console.WriteLine($"Uploaded {uploadedBytes} bytes of {fileSize} bytes");
                    }
                    i++;
                }
                
                    
                
            }
        }
    }
}
