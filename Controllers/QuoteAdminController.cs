using System;
using Microsoft.AspNetCore.Mvc;
using QuoteGeneratorAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;



namespace QuoteGeneratorAPI.Controllers {

    public class QuoteAdminController : Controller {
        
        private IWebHostEnvironment environment; 
        public QuoteAdminController(IWebHostEnvironment env){
            environment = env;
        }
        public IActionResult Index() {
            QuoteManager quoteManager = new QuoteManager();
            quoteManager.getQuotesList();
            Console.WriteLine("Quotes List>>>>>>>>>>>>>>");
            Console.WriteLine(quoteManager.quotesList);
            return View(quoteManager);
        }
        

        [HttpPost]
        public IActionResult AddQuote(QuoteManager quoteManager, string author, string quote, string permalink, IFormFile selectedFile){
            Console.WriteLine("----------------------------");
            string image = Path.GetFileName(selectedFile.FileName);
            Console.WriteLine("------------selected image: "+quoteManager.selectedImage);
            ImageUploader imageUploader = new ImageUploader(environment, "uploads");
            Console.WriteLine(">>>>>>>>>>>>>>>>>>selected file:  "+selectedFile);
            int result = imageUploader.upload(selectedFile);
            Console.WriteLine(">>>>>>>>>>>>>>>>>>result of upload: "+result);
            string feedback = "";
            if(result == 5){                
                feedback = quoteManager.addQuote(author, quote, permalink, image);
            }
            // Console.WriteLine("Upload Result"+ result);
            // imageManager.buildImagesList(environment, "uploads");
            ViewData["feedback"] = feedback;
            quoteManager = new QuoteManager();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteQuote(QuoteManager quoteManager, int id){
            //fetch the image name stored in DB
            quoteManager.selectedImage = quoteManager.getQuoteById(id);
            //if image, delete it from server location
            if(quoteManager.selectedImage != ""){
                ImageUploader imageUploader = new ImageUploader(environment, "uploads");
                imageUploader.delete(quoteManager.selectedImage);
                string feedback = quoteManager.deleteQuote(Convert.ToInt32(id));
                //ViewData["deleteFeedBack"] = "Quote with image 'imageName' has been deleted";   
                 quoteManager.getQuotesList();             
            }
            return RedirectToAction("Index");            
        }

        [HttpPost]
        public IActionResult GetQuoteById(QuoteManager quoteManager, int id){
            //fetch the image name stored in DB
            quoteManager.selectedImage = quoteManager.getQuoteById(id);
            return RedirectToAction("Index");            
        }

    }
}
