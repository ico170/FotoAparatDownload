using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//for xml config file
using System.Configuration;
using System.Collections.Specialized;
//using System.Threading.Tasks;

    
/*
 app for my mom because she never could copy images from her camera
*/

namespace FotoAparatDownload
{
    class Program
    {
        // declaration of variables
        List<string> pathSource = new List<string>();
        string downloadLocation = ConfigurationManager.AppSettings.Get("downloadLocation");
        List<string> drives = new List<string>();
        string folderName = ConfigurationManager.AppSettings.Get("folderName");

        //gets all drives that are marked removable when testing camera see if it needs to be removable or something else.


        public List<string> GetPathSource()
        {
            return pathSource;
        }


        public List<string> GetDrives()
        {
            return drives;
        }


        public string GetDrive(int i)
        {
            return drives.ElementAt(i);
        }


        public void SetPathSource(string path)
        {
            pathSource.Add(path);
        }


        public List<string> InitDrive()
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady)
                {
                    if (drive.DriveType.ToString() == "Removable")
                    {
                        drives.Add(drive.ToString());
                    }
                }
            }
            if (drives.Count() == 0)
            {
                Console.WriteLine("No removable drives");
            }
            return drives;
        }

        /// <summary>
        /// summary test
        /// </summary>
        /// <param name="root"></param>
        /// <param name="searchWord">search Test</param>
        /// <returns>bool</returns>
        public bool GetSubFolder(string root, string searchWord)
        {

            var foldersFound = Directory.GetDirectories(root, searchWord, SearchOption.AllDirectories);
            if (foldersFound.Length == 0)
            {
                return false;
            }
            foreach (var folder in foldersFound)
            {
                SetPathSource(folder);
                //Console.WriteLine(folder);
            }
            return true;
        }

        /// <summary>
        /// Prints out available drives and location
        /// </summary>
        public void Print()
        { // Need modular print for each thing  enum
            Console.WriteLine("available drives: ");
            foreach (var drive in drives)
            {
                Console.WriteLine(drive);
            }
            Console.WriteLine("sourcePath");
            foreach (var path in pathSource)
            {
                Console.WriteLine(path);
            }
        }


        static void Main(string[] args)
        {
           // string testing= ConfigurationManager.AppSettings.Get("Key0");
          

            Program test = new Program();
            test.InitDrive();
            //Console.WriteLine("Count:{0}", test.GetDrives().Count());
            test.SetPathSources();
            test.CopyFiles();
           
            test.Print();
            Console.WriteLine("DONE Press any key...");
            Console.ReadKey();
       
            /*
            jedna funkcija koja zove sve ostale da u mainu ne bude komadića koji se i onako samo jedni na druge vežu. MAINMAIN   MAIN---------------------------------------MAIN
            */
        }

        private void CopyFiles()
        {
            Console.WriteLine("<-----------COPY STARTED------------------------>");
            foreach (var path in pathSource)
            {
                string[] files = Directory.GetFiles(path);
                for (var i = 0; i < files.Length; i++)
                {
                    //cuts out the name of the file out of link
                    string input = files[i];
                    int index = input.LastIndexOf("\\");
                    if (index > 0)
                    {
                        input = input.Substring(index);
                    }
                   
                    //if file is same name make a new  one with added (1) NOT DONE
                    try
                    {
                        File.Copy(files[i], downloadLocation + input, false);
                    }

                    catch (IOException copyError)
                    {
                        Console.WriteLine(copyError.Message);
                    }

                }
               
            }
            Console.WriteLine("<-----------COPY DONE------------------------>");
        }

        private void SetPathSources()
        {
            for (int i = 0; i < GetDrives().Count(); i++)
            {

                //remove hardcoded word DCIM not done
                if (GetSubFolder(GetDrive(i), folderName) == false)
                {
                    Console.WriteLine("No such file");
                }
            }
        }
    }

}


