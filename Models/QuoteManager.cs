using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuoteGeneratorAPI.Models {

    public class QuoteManager {
         // database connectivity variables
        private MySqlConnection dbConnection; 
        private MySqlCommand dbCommand;
        private MySqlDataReader dbReader;

        // property variables
        private int _count = 0;
        private List<Quote> _quotes;

        public QuoteManager() {
            // initialization
            _count = 0;
            _quotes = new List<Quote>();            
            dbConnection = new MySqlConnection(Connection.CONNECTION_STRING);
            dbCommand = new MySqlCommand("", dbConnection);

        }  

        // ------------------------------------------------- gets/sets
        public int count {
            get {
                return _count;
            }
        }
        public List<Quote> quotes {
            get {
                return _quotes;
            }
        }

        


        
        // ------------------------------------------------- private methods        

        //Get Reviiews
        public void getQuotes(int limit) {
            try {
                // open connection
                dbConnection.Open();

                dbCommand.CommandText = "SELECT * FROM tbl_quotes LIMIT "+limit;
                dbReader = dbCommand.ExecuteReader();

                while(dbReader.Read()){
                    Quote item = new Quote();
                    item.id = Convert.ToInt32(dbReader["id"]);
                    item.author = Convert.ToString(dbReader["author"]);
                    item.quote = Convert.ToString(dbReader["quote"]);
                    item.permalink = Convert.ToString(dbReader["permalink"]);
                    item.image = Convert.ToString(dbReader["image"]);
                    _quotes.Add(item);
                }
                dbReader.Close();
            } catch (Exception e) {
                Console.WriteLine(">>> error occured");
                Console.WriteLine(">>> " + e.Message);
            }finally {
                dbConnection.Close();
            }            
        }

        // Add Quotes
        public string addQuote(string author, string quote, string permalink, string image){
            /*****Condition checks for author, quote and image validations****/
            // if((author == "" && quote == "") || (permalink == null && review_lname == null)){                 
            //     review_fname = "Anonymous";
            // }
            // if(review_comment == "" || review_comment == null){
            //     return;
            // }else{
                try {
                dbConnection.Open();

                dbCommand.Parameters.Clear();
                dbCommand.CommandText = "INSERT INTO tblQuotes (author,quote,permalink,image) VALUES (?author,?quote,?permalink,?image)";

                dbCommand.Parameters.AddWithValue("?author", author);

                dbCommand.Parameters.AddWithValue("?quote", quote);

                dbCommand.Parameters.AddWithValue("?permalink", permalink);

                dbCommand.Parameters.AddWithValue("?image", image);

                 dbCommand.ExecuteNonQuery();
                 return "Data Saved Successfully";  
            } catch (Exception e) {
                Console.WriteLine(">>> An error has occured with add quote");
                Console.WriteLine(">>> " + e.Message);
                 return "An error has occured with add quote";  
            } finally {
                dbConnection.Close();
            }
            // }
            
        }

        //  public void getQuotesList() {
        //     try {
        //         dbConnection.Open();
        //         Console.WriteLine("Here1");
        //         dbCommand.CommandText = "SELECT * FROM tblQuotes";
        //         Console.WriteLine("Here2>>>"+ dbCommand.CommandText);
        //         dbReader = dbCommand.ExecuteReader();
        //         Console.WriteLine("Here3>>>>"+ dbReader);

        //         while(dbReader.Read()){
        //             Console.WriteLine("Here4>>>>"+dbReader.Read());
        //             SelectListItem item = new SelectListItem();
        //             item.Text = Convert.ToString(dbReader["quote"]);
        //             item.Value = Convert.ToString(dbReader["id"]);
        //             _quotesList.Add(item);
        //         }
        //         dbReader.Close();
        //     } catch (Exception e) {
        //         Console.WriteLine(">>> error occured");
        //         Console.WriteLine(">>> " + e.Message);
        //     }finally {
        //         dbConnection.Close();
        //     }  
        // }

        // public string getQuoteImageById(int id) {
        //     try {
        //         dbConnection.Open();
        //         dbCommand.CommandText = "SELECT * FROM tblQuotes where id="+id;
        //         dbReader = dbCommand.ExecuteReader();
        //         while(dbReader.Read()){                    
        //              selectedImage = Convert.ToString(dbReader["image"]);
        //         }
        //         dbReader.Close();
        //         return (selectedImage != null)? selectedImage : "";
        //     } catch (Exception e) {
        //         Console.WriteLine(">>> error occured");
        //         Console.WriteLine(">>> " + e.Message);
        //         return "";
        //     }finally {
        //         dbConnection.Close();
        //     }  
        // }

        // public string deleteQuote(int id){
        //    try {
        //         dbConnection.Open();                           
        //         dbCommand.Parameters.Clear();
        //         dbCommand.CommandText = "DELETE FROM tblQuotes WHERE id= ?id";
        //         dbCommand.Parameters.AddWithValue("?id", id);
        //         dbCommand.ExecuteNonQuery();
        //         return "Data Deleted Successfully";             
        //     } catch (Exception e) {
        //         Console.WriteLine(">>> An error has occured with delete quote");
        //         Console.WriteLine(">>> " + e.Message);
        //          return "An error has occured with delete quote";  
        //     } finally {
        //         dbConnection.Close();
        //     }
        // }
    }
}