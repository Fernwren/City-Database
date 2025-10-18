using System.Collections;
using System.IO;
using System.Net.Mail;
internal class Program
{
    static Random random = new Random();
    static string[] cities = new string[82];

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
        string[] cities = readCities("cities.txt");
        tenCityTrip(cities, null);
        Hashtable neighborhood = readNeighbours("neighbourCities.txt");
        
        for(int i = 1; i < cities.Length; i++)
        {
            Console.Write(cities[i]+": ");
            if (neighborhood[cities[i]].GetType()==new ArrayList().GetType())
            {
                foreach(string c in (ArrayList)neighborhood[cities[i]])
                {
                    Console.Write(c+" ");
                }
                Console.WriteLine();
            }
        }
    }
}