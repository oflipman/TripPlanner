using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddingBinaryFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "";
            string imagePath = @"";
            int id = 5;

            BinaryFilesManager manager = new BinaryFilesManager();;
            byte[] image = manager.GetBinaryFiles(imagePath);
            manager.InsertDataInBytes(image,connectionString,id);
        }

    }
}
