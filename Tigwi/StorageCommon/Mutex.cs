using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Threading;
using System.Diagnostics;

namespace StorageCommon
{
    public class Mutex : IDisposable
    {
        CloudBlob blob;

        // The mutex must already exists
        public Mutex(CloudBlobContainer container, string mutexName, Exception e)
        {
            blob = container.GetBlobReference(mutexName);

            byte[] b1 = { 1 };
            BlobRequestOptions requestOpt = new BlobRequestOptions();
            bool keepGoing = true;
            string oldEtag = "";
            int lastChange = 0;
            do
            {
                byte[] b;
                string eTag;
                try
                {
                    eTag = blob.Attributes.Properties.ETag;
                    if (eTag != oldEtag)
                    {
                        lastChange = Environment.TickCount;
                        oldEtag = eTag;
                    }
                    b = blob.DownloadByteArray();
                }
                catch (Exception)
                {
                    throw e;
                }

                requestOpt.AccessCondition = AccessCondition.IfMatch(eTag);
                if (b[0] == 0 || Environment.TickCount - lastChange > 3000) // on ne peut garder un lock plus de 3 s
                {
                    try
                    {
                        blob.UploadByteArray(b1, requestOpt);
                        keepGoing = false;
                    }
                    catch (StorageClientException ex)
                    {
                        if (ex.ErrorCode != StorageErrorCode.ConditionFailed)
                            throw;
                    }
                }
                else
                    Thread.Sleep(50);   // constante arbitraire
            } while (keepGoing);
        }

        public void Renew()
        {
            byte[] b1 = { 1 };
            blob.UploadByteArray(b1);
        }

        public void  Dispose()
        {
            byte[] b0 = { 0 };
            blob.UploadByteArray(b0);
        }

        static public void Init(CloudBlobContainer container, string mutexName)
        {
            byte[] b0 = { 0 };
            CloudBlob blob = container.GetBlobReference(mutexName);
            blob.UploadByteArray(b0);
        }

        static public void Delete(CloudBlobContainer container, string mutexName)
        {
            CloudBlob blob = container.GetBlobReference(mutexName);
            blob.DeleteIfExists();
        }
    }
}
