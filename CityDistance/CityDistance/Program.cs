using System.Collections;
using System.IO;
using System.Net.Mail;
internal class Program
{
    static Random random = new Random();
    static string[] cities = new string[82];

    static void tenCityTrip(string[] cities, int[,] distances=null)
    {
        int prevCity = random.Next(1, 82);
        Console.WriteLine(cities[prevCity]);
        int totalDistance = 0;
        for (int i = 0; i < 9; i++)
        {
            int nextCity = random.Next(1,82);
            //Console.WriteLine(distances[prevCity,nextCity]);
            Console.WriteLine(cities[nextCity]);
            prevCity = nextCity;
            //totalDistance += distances[prevCity,nextCity];
        }
    }

    private static void Main(string[] args)
    {
        try
        {
            StreamReader file = new StreamReader("cities.txt");
            string line = file.ReadLine();
            int index = 1;

            while (line != null)
            {
                cities[index] = line;
                index++;
                line = file.ReadLine();
            }
            
            tenCityTrip(cities);
        }
        catch (Exception ex)
        {
            Console.WriteLine("fatal error");
        }


        try
        {
            Hashtable neighborhood = new Hashtable();
            StreamReader file = new StreamReader("neighbourCities.txt");
            string line = file.ReadLine();

            while (line != null)
            {
                string[] temp = line.Split(new char[] { ',' });
                neighborhood[temp[0]] = temp.Skip(1);
                //???????????????????????????????????????????????????????????
                line = file.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("fatal error");
        }
    }
}