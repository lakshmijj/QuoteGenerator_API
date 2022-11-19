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
            Quotes quote = new Quotes();
            quote.getQuotesList();       
            return View(quote);
        }
        

        [HttpPost]
        public IActionResult AddQuote(Quotes quotes){  
            TempData["author"] = quotes.author;
            TempData["quote"] = quotes.quote;
            TempData["permalink"] = quotes.permalink;
            if (!ModelState.IsValid) {
                return View("Index", quotes);                
            };
            quotes.image = Path.GetFileName(quotes.selectedFile.FileName);            
            quotes.filepath = quotes.image;
            ImageUploader imageUploader = new ImageUploader(environment, "uploads");
            if(imageUploader.fileCheck(quotes.image)){
                quotes.filepath = quotes.filepath+"_"+new Guid();
            }            
            string result = imageUploader.upload(quotes.selectedFile, quotes.filepath);
            string feedback = "";
            if(result == "File saved successfully!" ){                
                feedback = quotes.addQuote();
            }else{
                feedback = result;
                TempData["addResponse"] = feedback;
                return View("Index", quotes); 
            }
            TempData["addResponse"] = feedback;
            TempData["deleteFeedBack"] = "";
            TempData["author"] = "";
            TempData["quote"] = "";
            TempData["permalink"] = "";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteQuote(Quotes quote, int id){
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
