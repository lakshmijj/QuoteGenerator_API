using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuoteGeneratorAPI.Models {

    public class Quotes {
        
        private MySqlConnection dbConnection;
        private MySqlCommand dbCommand;
        private MySqlDataReader dbReader;

       // ------------------------------------------------------- get/set methods
        // [Key] means it's unique
        [Key]
        public int id {get;set;}

        [Required(ErrorMessage="Author cannot be empty")]
        [MaxLength(100, ErrorMessage ="Maximum 100 characters are allowed")]
        [Display(Name="Author")]
        public string author {get;set;}

        [Required(ErrorMessage="Quote cannot be empty")]
        [MaxLength(250, ErrorMessage ="Maximum 250 characters are allowed")]
        [Display(Name="Quote")]        
        public string quote {get; set;}        
        
        //[RegularExpression(@"[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)", ErrorMessage="Please enter a valid URL")]
        [Url(ErrorMessage="Please enter a valid Url")]
        public string? permalink {get; set;}
        
        [Required]
        [Display(Name="Image")]
        public IFormFile selectedFile {get; set;}

                
        public string selectedImage = "";
        public string image = "";
        public string filepath = "";

        private List<SelectListItem> _quotesList;

        public List<SelectListItem> quotesList {
            get {
                return _quotesList;
            }
        }


        //constructor
        public Quotes(){
            dbConnection = new MySqlConnection(Connection.CONNECTION_STRING);
            dbCommand = new MySqlCommand("", dbConnection);
            _quotesList = new List<SelectListItem>();
            selectedImage = "";
            author = "";
            quote = "";
            permalink = "";
            image = "";
            filepath = "";
        }

        //-------------------------public methods

        // Add Quotes
        public string addQuote(){
                try {
                // open connection                
                dbConnection.Open();

                dbCommand.Parameters.Clear();
                dbCommand.CommandText = "INSERT INTO tblQuotes (author,quote,permalink,image, filepath) VALUES (?author,?quote,?permalink,?image, ?filepath)";

                dbCommand.Parameters.AddWithValue("?author", author);

                dbCommand.Parameters.AddWithValue("?quote", quote.Trim());

                dbCommand.Parameters.AddWithValue("?permalink", permalink);

                dbCommand.Parameters.AddWithValue("?image", image);

                dbCommand.Parameters.AddWithValue("?filepath", filepath);

                 dbCommand.ExecuteNonQuery();
                 return "Data Saved Successfully";  
            } catch (Exception e) {
                Console.WriteLine(">>> An error has occured with add quote");
                Console.WriteLine(">>> " + e.Message);
                 return "An error has occured with add quote";  
            } finally {
                dbConnection.Close();
            }
            
        }
        
        //Delete Quote
        public string deleteQuote(int id){
           try {
                dbConnection.Open();                           
                dbCommand.Parameters.Clear();
                dbCommand.CommandText = "DELETE FROM tblQuotes WHERE id= ?id";
                dbCommand.Parameters.AddWithValue("?id", id);
                dbCommand.ExecuteNonQuery();
                return "Data Deleted Successfully";             
            } catch (Exception e) {
                Console.WriteLine(">>> An error has occured with delete quote");
                Console.WriteLine(">>> " + e.Message);
                 return "An error has occured with delete quote";  
            } finally {
                dbConnection.Close();
            }
        }

        public void getQuotesList() {
            try {
                dbConnection.Open();
                Console.WriteLine("Here1");
                dbCommand.CommandText = "SELECT * FROM tblQuotes";
                Console.WriteLine("Here2>>>"+ dbCommand.CommandText);
                dbReader = dbCommand.ExecuteReader();
                Console.WriteLine("Here3>>>>"+ dbReader);

                while(dbReader.Read()){
                    SelectListItem item = new SelectListItem();
                    item.Text = Convert.ToString(dbReader["quote"]);
                    item.Value = Convert.ToString(dbReader["id"]);
                    _quotesList.Add(item);
                }
                Console.WriteLine("Count >>>"+_quotesList.Count());
                dbReader.Close();
            } catch (Exception e) {
                Console.WriteLine(">>> error occured");
                Console.WriteLine(">>> " + e.Message);
            }finally {
                dbConnection.Close();
            }  
        }

        public string getQuoteImageById(int id) {
            try {
                dbConnection.Open();
                dbCommand.CommandText = "SELECT * FROM tblQuotes where id="+id;
                dbReader = dbCommand.ExecuteReader();
                while(dbReader.Read()){                    
                     selectedImage = Convert.ToString(dbReader["image"]);
                }
                dbReader.Close();
                return (selectedImage != null)? selectedImage : "";
            } catch (Exception e) {
                Console.WriteLine(">>> error occured");
                Console.WriteLine(">>> " + e.Message);
                return "";
            }finally {
                dbConnection.Close();
            }  
        }
 
    }
}