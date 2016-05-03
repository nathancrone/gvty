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
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Linq;

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
                        
            //get the listing
            //CloudBlockBlob blobList = blobContainer.GetBlockBlobReference();
            
            return View();
        }

        public ActionResult Kml(string path)
        {
            string prefix = ConfigurationManager.AppSettings["RootPrefix"];

            // Create the account
            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["AzureStorage"].ConnectionString);

            // Create a blob client.
            CloudBlobClient blobClient = account.CreateCloudBlobClient();

            // Get a reference to a blob container.
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("iocb");

            //get the listing
            CloudBlockBlob blobList = blobContainer.GetBlockBlobReference(string.Format("{0}{1}", Server.UrlDecode(path), "/coordinates"));

            string coords = blobList.DownloadText();

            XNamespace ns = "http://earth.google.com/kml/2.2";

            XDocument xml = new XDocument(new XElement(ns + "kml",
                 new XElement("Document", 
                 new XElement("name", ""), 
                 new XElement("description", ""), 
                 new XElement("Style", new XAttribute("id", "style1"), 
                 new XElement("LineStyle", 
                 new XElement("color", "1A000000"), 
                 new XElement("width", "4")), 
                 new XElement("PolyStyle", 
                 new XElement("color", "809AC2FC"), 
                 new XElement("fill", "1"), 
                 new XElement("outline", "1"))), new XElement("Placemark", new XElement("name", ""), new XElement("styleUrl", "#style1"), new XElement("Polygon", new XElement("outerBoundaryIs", new XElement("LinearRing", new XElement("tessellate", "1"), new XElement("coordinates", coords))))))));
            
            return Content(xml.ToString(), "text/xml");
        }

    }
}