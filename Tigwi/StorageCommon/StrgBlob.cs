using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace StorageCommon
{
    public class StrgBlob<T>
    {
        BinaryFormatter formatter;
        CloudBlob blob;

        public StrgBlob(CloudBlobContainer container, string blobName)
        {
            formatter = new BinaryFormatter();
            blob = container.GetBlobReference(blobName);
        }

        public T GetIfExists(Exception e)
        {
            // TODO : better error handling
            // TODO : replace the exception by an error code
            // TODO : stream.close est-il important ?
            try
            {
                BlobStream stream = blob.OpenRead();
                return (T)formatter.Deserialize(stream);
            }
            catch (Exception ee)
            {
                throw e;
            }
        }
    }
}
