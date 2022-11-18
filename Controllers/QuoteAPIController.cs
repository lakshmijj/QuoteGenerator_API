using System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using QuoteGeneratorAPI.Models;

namespace QuoteGeneratorAPI.Controllers {
    // attribute is required for Web APIs
    [ApiController]  
    // disabling CORs for requests / responses of public Web API - eliminates CORs errors if this web API is used with a client side web app
    [DisableCors]

    public class QuoteAPIController : ControllerBase {

        // set to get instead of HttpPost
        [HttpGet]
        // the URL routing - Web APIs must have one
        [Route("quotes/{limit}")]
        public ActionResult<List<string>> Get(string limit) {
            // for this action method to return JSON, you only need to have it return a List of data
            // this test List is an example only!
            QuoteManager quoteManager = new QuoteManager();
            quoteManager.getQuotes(Convert.ToInt32(limit));
            List<string> test = new List<string>() { "hello","world","with","json" };
            
            // test this out by hitting https://localhost:5001/data
            return test;
        }        
    }
    
}
