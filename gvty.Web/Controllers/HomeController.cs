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
            string prefix = ConfigurationManager.AppSettings["RootPrefix"];

            // Create the account
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);

            // Create a blob client.
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Get a reference to a blob container.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("iocb");

            //get the listing
            CloudBlockBlob blobList = blobContainer.GetBlockBlobReference(string.Format("{0}{1}", prefix, "/_list"));

            string list = blobList.DownloadText();

            List<Territory> Territories = (List<Territory>)JsonConvert.DeserializeObject(list, typeof(List<Territory>));

            return View(Territories);
        }


        public ActionResult Map(string path)
        {
            // Create the account
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);

            // Create a blob client.
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Get a reference to a blob container.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("iocb");

            IEnumerable<IListBlobItem> blobItems = blobContainer.ListBlobs(Server.UrlDecode(path), true);

            //Response.Write(blobItems.Count());



            //get the listing
            //CloudBlockBlob blobList = blobContainer.GetBlockBlobReference();


            return View();
        }

    }
}