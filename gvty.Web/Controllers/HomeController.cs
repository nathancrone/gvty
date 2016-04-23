using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Table;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using gvty.Web.Models;


namespace gvty.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            string prefix = "us752/en/CNT/{0}";

            // Create the account
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);

            // Create a blob client.
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Get a reference to a blob container.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("iocb");

            //get the listing
            CloudBlockBlob blobList = blobContainer.GetBlockBlobReference(string.Format(prefix, "_list"));

            string list = blobList.DownloadText();

            List<Territory> Territories = (List<Territory>)JsonConvert.DeserializeObject(list, typeof(List<Territory>));

            return View(Territories);
        }
    }
}