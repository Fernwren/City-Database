using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;
using System.IO;
using System.Net.Mail;
using System.Numerics;
using System.Security.Cryptography;
internal class Program
{
    static Random random = new Random();

    static int getPlateNum(string[] cities, string city)
    {
        for (int i = 1; i <= 81; i++)
        {
            if (cities[i] == city) { return i; } 
        }
        return 0;
    }
    static string[] readCities(string name)
    {
        string[] cities = new string[82];
        int index = 1;
        StreamReader file = new StreamReader(name);
        string line= file.ReadLine();
        while (line != null)
        {
            cities[index++] = line;
            line = file.ReadLine();
        }
        return cities;
    }
    static Hashtable readNeighbours(string name)
    {
        Hashtable neighborhood = new Hashtable();
        StreamReader file = new StreamReader("neighbourCities.txt");
        string line = file.ReadLine();

        while (line != null)
        {
            string[] temp = line.Split(new char[] { ',' });
            ArrayList temp2 = new ArrayList();
            foreach (string i in temp.Skip(1)) { temp2.Add(i); }
            neighborhood[temp[0]] = temp2;
            line = file.ReadLine();
        }
        return neighborhood;
    }
    static int[,] readCityDistancesXLSX(string path)
    {
        XLWorkbook table = new XLWorkbook(path);
        int[,] distances = new int[82, 82];

        IXLWorksheet worksheet = table.Worksheet(1);

        for (int r = 3; r<=83; r++)
        {
            for (int c = 3; c<=83; c++)
            {
                if (worksheet.Row(r).Cell(c).Value.ToString() != "")
                {
                    distances[r - 2, c - 2] = int.Parse(worksheet.Row(r).Cell(c).Value.ToString());
                }
                else
                {
                    distances[r - 2, c - 2] = 0;
                }
                
            }
        }

        return distances;
    }
    static void tenCityTrip(string[] cities, int[,] distances)
    {
        int totalDistance = 0;
        int prevCity = random.Next(1, 82);
        Console.WriteLine("--------------------------------");
        Console.Write(cities[prevCity] + " ");
        
        for (int i = 0; i < 9; i++)
        {
            int nextCity = random.Next(1,82);
            Console.Write("({0}) ",distances[prevCity,nextCity]);
            Console.Write(cities[nextCity]+" ");
            totalDistance += distances[prevCity, nextCity];
            prevCity = nextCity;
        }
        
        Console.WriteLine();
        Console.WriteLine("TOTAL DISTANCE TRAVELLED: "+totalDistance);
        Console.WriteLine("--------------------------------");
    }
    static void mostDistantNeighbours(Hashtable neighborhood, int[,] distances, string[] cities)
    {
        int distance = 0;
        string c1=" ";
        string c2=" ";
        IDictionaryEnumerator enumerator = neighborhood.GetEnumerator();
        
        while (enumerator.MoveNext())
        {
            string city1 = (string)enumerator.Key;
            int plate1 = getPlateNum(cities,city1);
            
            foreach (string city2 in (ArrayList)enumerator.Value)
            {
                int plate2 = getPlateNum(cities, city2);
                if (distances[plate1, plate2] > distance)
                {
                    distance = distances[plate1, plate2];
                    c1= city1;
                    c2= city2;
                }
            }
        }
        Console.WriteLine(c1 + "-" + c2 + " Distance:" + distance);  
    }
   

    private static void Main(string[] args)
    {
        string[] cities = readCities("cities.txt");
        Hashtable neighborhood = readNeighbours("neighbourCities.txt");
        int[,] distances = readCityDistancesXLSX("ilmesafe.xlsx");

        tenCityTrip(cities, distances);

        for (int i = 1; i < cities.Length; i++)
        {
            Console.Write(cities[i]+": ");
            
            foreach(string c in (ArrayList)neighborhood[cities[i]])
            {
                Console.Write(c+" ");
            }
            Console.WriteLine();
            
        }

        mostDistantNeighbours(neighborhood,distances,cities);
    }
}