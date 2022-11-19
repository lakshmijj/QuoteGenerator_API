using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Json;

namespace QuoteGeneratorAPI.Models {

    public class qm{
         public int id {get;set;}
         public string author {get;set;}
         public string quote {get;set;}
         public string image {get;set;}
         public string permalink {get;set;}
    }

    public class QuoteManager {
         // database connectivity variables
        private MySqlConnection dbConnection; 
        private MySqlCommand dbCommand;
        private MySqlDataReader dbReader;

        // property variables
        private int _count = 0;
        private List<qm> _quotes;

        public QuoteManager() {
            // initialization
            _count = 0;
            _quotes = new List<qm>();            
            dbConnection = new MySqlConnection(Connection.CONNECTION_STRING);
            dbCommand = new MySqlCommand("", dbConnection);

        }  

        // ------------------------------------------------- gets/sets
        public int count {
            get {
                return _count;
            }
        }
        public List<qm> quotes {
            get {
                return _quotes;
            }
        }

        


        
        // ------------------------------------------------- private methods        

        //Get Reviiews
        public string getQuotes(int limit) {
            try {
                // open connection
                dbConnection.Open();

                dbCommand.CommandText = "SELECT * FROM tblQuotes LIMIT "+limit;
                dbReader = dbCommand.ExecuteReader();

                while(dbReader.Read()){
                    qm item = new qm();
                    item.id = Convert.ToInt32(dbReader["id"]);
                    item.author = Convert.ToString(dbReader["author"]);
                    item.quote = Convert.ToString(dbReader["quote"]);
                    item.permalink = Convert.ToString(dbReader["permalink"]);
                    item.image = Convert.ToString(dbReader["image"]);
                    //_quotes.Add(item);
                    _quotes.Add(new qm(){id=item.id, author=item.author, quote=item.quote, permalink=item.permalink, image=item.image});
                    Console.WriteLine(JsonSerializer.Serialize(_quotes));
                    //var serializer = new JavaScriptSerializer();
                   //var serializedResult = serializer.Serialize(RegisteredUsers);
                }
                dbReader.Close();
                return JsonSerializer.Serialize(_quotes);
            } catch (Exception e) {                
                Console.WriteLine(">>> error occured");
                Console.WriteLine(">>> " + e.Message);
                return "[]";
            }finally {
                dbConnection.Close();
            }            
        }
    }
}