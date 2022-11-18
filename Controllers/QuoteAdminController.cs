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
            Quote quote = new Quote();
            quote.getQuotesList();    
            return View(quote);
        }
        

        [HttpPost]
        public IActionResult AddQuote(Quote quotes, string author, string quote, string permalink, IFormFile selectedFile){
            if (!ModelState.IsValid) {
                return View("Index", quotes);
            };
            string image = Path.GetFileName(selectedFile.FileName);
            ImageUploader imageUploader = new ImageUploader(environment, "uploads");
            int result = imageUploader.upload(selectedFile);
            string feedback = "";
            if(result == 5){                
                feedback = quotes.addQuote(author, quote, permalink, image);
            }
            TempData["addResponse"] = feedback;
            TempData["deleteFeedBack"] = "";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteQuote(Quote quote, int id){
            //fetch the image name stored in DB
            quote.selectedImage = quote.getQuoteImageById(id);
            //if image, delete it from server location
            if(quote.selectedImage != ""){
                ImageUploader imageUploader = new ImageUploader(environment, "uploads");
                //Deleted the image
                imageUploader.delete(quote.selectedImage);
                //Delete Quote from DB
                string feedback = quote.deleteQuote(Convert.ToInt32(id));
                TempData["addResponse"] = "";
                TempData["deleteFeedBack"] = "Quote with image "+quote.selectedImage+" has been deleted";
            }
            return RedirectToAction("Index");            
        }

    }
}
