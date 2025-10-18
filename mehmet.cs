using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;


class Program
{

    static void Main(string[] args)
    {
        string datapath = @"C:\Users\acarm\Downloads\ilmesafe.xlsx";
        DataTable dt = readDT(datapath);
        Console.WriteLine("DataTable loaded with {0} rows and {1} columns.", dt.Rows.Count, dt.Columns.Count);
        string[] cities = new string[81];
        for (int i = 0; i < 1; i++)
        {
            for (int j = 2; j < dt.Columns.Count; j++)
            {
                cities[j - 2] = dt.Rows[i][j].ToString();
            }

        }
        for (int i = 0; i < 81; i++)
        {
            Console.WriteLine(cities[i]);
        }

        selectRandomCities(cities);
    }

    static void selectRandomCities(string[] cities)
    {
        string[] temp = cities;
        int[] randomNums = new int[10];
        Random random = new Random();
        Dictionary<string, int> selectedCitises = new Dictionary<string, int>();
        
        for (int i = 0; i < 10; i++)
        {


            int index = random.Next(0, 81);
            if (selectedCitises.ContainsKey(temp[index]))
            {
                i--;
                continue; // If already selected, skip this iteration
            }
            selectedCitises.Add(temp[index], index)


            Console.WriteLine("Selected City: {0} at index {1}", cities[index], index);



        }
    }



    static DataTable readDT(String datapath)
    {
        string filePath = datapath;

            // Excel dosyasını aç
        using (var workbook = new XLWorkbook(filePath))
        {
            var worksheet = workbook.Worksheet(1); // 1. sayfa
            var range = worksheet.RangeUsed();     // Dolu hücre aralığı

                // Verileri okumak için DataTable oluştur
            DataTable dt = new DataTable();

            bool firstRow = true;
            foreach (var row in range.Rows())
            {
                if (firstRow)
                {
                        // İlk satır başlık
                    foreach (var cell in row.Cells())
                        dt.Columns.Add(cell.Value.ToString());
                        firstRow = false;
                }
                else
                {
                        // Diğer satırlar veri
                    dt.Rows.Add();
                    int i = 0;
                    foreach (var cell in row.Cells())
                    {
                        dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                        i++;
                    }
                }
            }
            return dt;

        }

    }
}

