using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace QuoteGeneratorAPI.Models {

    public class ImageUploader {

        // class constants for different errors while uploading
        public const string ERROR_NO_FILE =  "Please select an image to upload";
        public const string ERROR_TYPE = "Please select only GIF, JPG or PNG files";
        public const string ERROR_SIZE = "Please select image less than 4 MB";
        public const string ERROR_NAME_LENGTH = "Please enter filename less than 100";
        public const string ERROR_SAVE =  "Sorry!! Could not save the file";
        public const string SUCCESS = "File saved successfully!";

        // this is the file size limit in bytes that IFormFile approach can handle
        // do have the option to stream larger files - but is more complicated
        private const int UPLOADLIMIT = 4194304;

        // needed for getting path to web app's location
        private string targetFolder;
        // path to the upload folder
        private string fullPath;

        public ImageUploader(IWebHostEnvironment env, string myTargetFolder) {
            // initialization
            targetFolder = myTargetFolder;         
            Console.WriteLine("----here-------"+targetFolder);
            Console.WriteLine("----env-------"+env);
            Console.WriteLine("----webrootpath-------"+env.WebRootPath);
            // check to see if web app's root folder has an "uploads" folder - if not create it
            fullPath = env.WebRootPath + "/" + targetFolder + "/";
            Console.WriteLine("----fullPAth-------"+fullPath);
            if (!Directory.Exists(fullPath)) {
                Directory.CreateDirectory(fullPath);
            }
        }

        // --------------------------------------------------- public methods
        public string upload(IFormFile file, string filenameToBeUsed){
            if(file != null){
                string contentType = file.ContentType;
                Console.WriteLine(contentType);
                if((contentType == "image/png") || (contentType == "image/jpeg") || (contentType == "image/gif")){
                   long size = file.Length;
                   if(size > 0 && size < UPLOADLIMIT){
                        string filename = filenameToBeUsed;//Path.GetFileName(file.FileName);
                        if(filename.Length < 100){
                            FileStream stream = new FileStream((fullPath+filename), FileMode.Create);
                            try{
                                file.CopyTo(stream);
                                stream.Close();
                                return ImageUploader.SUCCESS;
                            }catch{
                                 stream.Close();
                                return ImageUploader.ERROR_SAVE;
                            }
                        }else{
                            return ImageUploader.ERROR_NAME_LENGTH;
                        }
                   }else{
                    return ImageUploader.ERROR_SIZE;
                   }
                }else{
                    return ImageUploader.ERROR_TYPE;
                }
            }else{
                return ImageUploader.ERROR_NO_FILE;
            }
        }

        public void delete(string filename){
            if(filename != null){
                string existingFile = Path.Combine(fullPath+filename);
                System.IO.File.Delete(existingFile);
            }                
        }

        public bool fileCheck(string filename){
            return File.Exists(fullPath+filename);
        }

    }

}