using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;


class Program
{   
    
    class CityCodeAndNamePair
    {
        string cityName;
        string code;

        public CityCodeAndNamePair(string cityName, string code)
        {
            this.cityName = cityName;
            this.code = code;
        }
        public CityCodeAndNamePair(string cityNameAndCode)
        {
            string[] City = cityNameAndCode.Split(' ');
            this.cityName = City[1];
            this.code = City[0];
        }

    }
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
        Dictionary<string,int> selectedCities = new Dictionary<string,int>();
        int[] city_codes = new int[10];
        (selectedCities,city_codes)= selectRandomCities(cities);
        int m = 0;
        int total_distance = 0;
        foreach (var city in selectedCities)
        {
            if (m >= 10) break;
            if (m == 0)
                {
                    Console.WriteLine($"{m+1}. şehrimiz {city.Key} ve plakası {city.Value}");
                    m++;
                    continue;
                }
            total_distance += Convert.ToInt32(dt.Rows[city_codes[m - 1]][city_codes[m] + 1]);
            Console.WriteLine($"{m + 1}. şehrimiz {city.Key} ve plakası {city.Value} önceki şehirden gelinen mesafe {dt.Rows[city_codes[m-1]][city_codes[m]+1]} , total distance {total_distance} , ");
            m++;
        }
        Dictionary<CityCodeAndNamePair, CityCodeAndNamePair[]>[] neighboor_set = ReadNeighboorCities(@"C:\Users\acarm\Downloads\NeighboringCities_with_all_plates (1).txt");

    }

    static (Dictionary<string,int>,int[]) selectRandomCities(string[] cities)
    {
        Random r = new Random();
        int[] city_codes = new int[10];
        Dictionary<string, int> selectedCities = new Dictionary<string, int>();
        int j = 0;
        while (j < 10)
        {
            int index = r.Next(cities.Length);
            string city = cities[index];
            if (selectedCities.ContainsKey(city))
            {
                j--;
                continue;
            }
            selectedCities.Add(city, index + 1);
            city_codes[j] = index+1;
            j++;

        }

        return (selectedCities,city_codes);
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

    static Dictionary<CityCodeAndNamePair,CityCodeAndNamePair[]>[] ReadNeighboorCities(String datapath)
    {
        Dictionary<CityCodeAndNamePair, CityCodeAndNamePair[]>[] neighboor_set = new Dictionary<CityCodeAndNamePair, CityCodeAndNamePair[]>[81];
        string[] lines = File.ReadAllLines(datapath);
        int z = 0;
        foreach (string line in lines)
        {
            if (z == 81) { break; }
            string[] city_names_inline = line.Split(',');
            CityCodeAndNamePair currentCity = new CityCodeAndNamePair(city_names_inline[0]);
            CityCodeAndNamePair[] neighboor_cities = new CityCodeAndNamePair[city_names_inline.Length - 1];
            int b = 0;
            foreach (var city in city_names_inline)
            {
                if (b == 0)
                {
                    b++;
                    continue;
                }
                Console.WriteLine(city);
                neighboor_cities[b] = new CityCodeAndNamePair(city_names_inline[b]);
            }
            
            neighboor_set[z].Add(currentCity, neighboor_cities);
            z++;
        }
        return neighboor_set;
    }
}

