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

            //Response.Write(blobItems.Count());
            

            //get the listing
            //CloudBlockBlob blobList = blobContainer.GetBlockBlobReference();
            
            return View();
        }

        public ActionResult Kml(string path)
        {
            XNamespace ns = "http://earth.google.com/kml/2.2";

            //XDocument xml = new XDocument(
            //    new XElement(ns + "kml", 
            //    new XElement("Placemark", 
            //    new XElement("name", "My House"), 
            //    new XElement("description", "This is where I live."), 
            //    new XElement("Point", 
            //    new XElement("coordinates", "‐1.1582475900650024,51.89766144330168,0"))))
            //    );

            string coords = "-96.7529,32.82105,0.0 -96.7499,32.82102,0.0 -96.74923,32.82103,0.0 -96.74846,32.82103,0.0 -96.74826,32.82103,0.0 -96.74802,32.82103,0.0 -96.74793,32.82104,0.0 -96.74784,32.82104,0.0 -96.74766,32.82106,0.0 -96.74746,32.8211,0.0 -96.74703,32.82118,0.0 -96.74683,32.82122,0.0 -96.74658,32.82126,0.0 -96.74653,32.82081,0.0 -96.74648,32.8203,0.0 -96.74645,32.81977,0.0 -96.74641,32.81915,0.0 -96.74640000000001,32.81885,0.0 -96.74641,32.8185,0.0 -96.74646,32.81777,0.0 -96.74651,32.81711,0.0 -96.74656,32.81639,0.0 -96.74784,32.81647,0.0 -96.74892,32.81653,0.0 -96.74931,32.81654,0.0 -96.74944,32.81654,0.0 -96.74962,32.81653,0.0 -96.74977,32.81652,0.0 -96.74996,32.81649,0.0 -96.75011,32.81647,0.0 -96.75019,32.81646,0.0 -96.75026,32.81644,0.0 -96.75041,32.8164,0.0 -96.75053,32.81636,0.0 -96.7506,32.81633,0.0 -96.75068,32.8163,0.0 -96.75082,32.81624,0.0 -96.75093,32.81617,0.0 -96.75099,32.81613,0.0 -96.75107000000001,32.81608,0.0 -96.75116000000001,32.81601,0.0 -96.75126,32.8159,0.0 -96.75131,32.81584,0.0 -96.75139,32.81573,0.0 -96.75145,32.81562,0.0 -96.75151000000001,32.81549,0.0 -96.7516,32.81527,0.0 -96.75166,32.81514,0.0 -96.75176000000002,32.81497,0.0 -96.75181,32.81487,0.0 -96.75187,32.81476,0.0 -96.75209,32.81452,0.0 -96.75216,32.81441,0.0 -96.75247,32.81414,0.0 -96.75265,32.81399,0.0 -96.75274,32.81389,0.0 -96.75293,32.81375,0.0 -96.75295,32.81385,0.0 -96.75298000000001,32.81406,0.0 -96.75299,32.81502,0.0 -96.75286,32.81507,0.0 -96.7529,32.81516,0.0 -96.75293,32.81526,0.0 -96.75296,32.81536,0.0 -96.75296,32.81539,0.0 -96.75297,32.81545,0.0 -96.75299,32.81557,0.0 -96.75299,32.81568,0.0 -96.75298000000001,32.81617,0.0 -96.75292,32.8163,0.0 -96.75292,32.81643,0.0 -96.75293,32.81667,0.0 -96.75293,32.81682,0.0 -96.75291,32.81721,0.0 -96.75289,32.81816,0.0 -96.75287,32.81912,0.0 -96.75288,32.81959,0.0 -96.75287,32.82009,0.0 -96.7529,32.82105,0.0";

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