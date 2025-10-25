    using ClosedXML;
    using ClosedXML.Excel;
    using DocumentFormat.OpenXml.Bibliography;
    using DocumentFormat.OpenXml.Drawing.Diagrams;
    using System.Collections;
    internal class Program
    {
        static Random random = new Random();
        class City
        {
            private string cityName;
            private int cityPlate;
            private List<string> neighbourList;
            public City(string cityName, int cityPlate, List<string> neighbourList)
            {
                this.cityName = cityName;
                this.cityPlate = cityPlate;
                this.neighbourList = neighbourList;
            }
            public string CityName { get {  return cityName; } }
            public int CityPlate { get { return cityPlate; } }
            public List<string> NeighbourList{ get { return neighbourList; } }

            public override string ToString()
            {
                return $"CityName: {cityName}, Plate: {cityPlate}, Neighbours: {string.Join(", ", neighbourList)}";
            }
    }
        static string[] readCities(string documentName)
        {
            string[] cities = new string[82];
            StreamReader reader = new StreamReader(documentName);
            int plateIndex = 1;
            string line = reader.ReadLine();
            while (line != null)
            {
                cities[plateIndex]=line.Trim();
                plateIndex++;
                line = reader.ReadLine();
            }
            reader.Close();
            return cities;
        }
        static int getPlateNumber(string cityName,string[] cities)
        {
            if(cityName==null || cities==null)
            {
                Console.WriteLine("deneme");
            }
            foreach (string city in cities)
            {
                if (city != null)
                {
                    if (city.Equals(cityName, StringComparison.OrdinalIgnoreCase))
                    {
                        return Array.IndexOf(cities, city);
                    }
                }
            
            }
            return 0;
        }
        static Hashtable neighborsCities(string documentName)
        {
            Hashtable neighbors = new Hashtable();
            StreamReader reader = new StreamReader(documentName);
            string line = reader.ReadLine();
            while (line != null)
            {
                List<string> parts = line.Split(',').ToList();
                neighbors[parts[0]] = parts.Skip(1).Select(i => i.Trim()).ToList();
                line = reader.ReadLine();
            }
            return neighbors;
        }
    static Hashtable citiesObj(string[] citiesName, Hashtable neighboursCities)
    {
        Hashtable cities = new Hashtable();
        foreach (string cityName in citiesName)
        {
            if (string.IsNullOrEmpty(cityName)) continue; // boş satırı atla

            if (!neighboursCities.ContainsKey(cityName))
            {
                Console.WriteLine($"Warning: Neighbour info missing for {cityName}");
                neighboursCities[cityName] = new List<string>(); // boş liste ile doldur
            }

            City newCity = new City(
                cityName,
                getPlateNumber(cityName, citiesName),
                (List<string>)neighboursCities[cityName]
            );
            cities[cityName] = newCity;
        }
        return cities;
    }

    static int[,] readCityDistances(string documentName)
        {
            int[,] distances = new int[82, 82];
            XLWorkbook wb = new XLWorkbook(documentName);
            var workSheet = wb.Worksheet(1);
            for (int r = 3; r < 84; r++)
            {
                for (int c = 3; c < 84; c++)
                {
                    int distance;
                    string cellValue=workSheet.Cell(r, c).Value.ToString();

                    if (cellValue=="")
                    {
                        distance = 0;
                        //Console.WriteLine((r - 2) + "+" + (c - 2) + "+" + distance);

                    }
                    else
                    {
                        distance = int.Parse(cellValue);
                        //Console.WriteLine((r-2)+"+"+(c-2)+"+"+distance);
                    }

                    distances[r - 2, c - 2]=distance;
                }
            }
            return distances;
        }
        static void findShortestRouteToIzmir(string[] cities, int[,] citiesDistances, Hashtable neighbors) {
            int izmirPlate = 35;
            int randomCityPlate = random.Next(1, 82);
            List<string> routes = new List<string>();
            int lastCityPlate = randomCityPlate;

            while (lastCityPlate != izmirPlate)
            {
                int compareDistance = citiesDistances[lastCityPlate, izmirPlate];
                foreach (string city in (List<string>)neighbors[cities[lastCityPlate]])
                {
                    int currentCityPlate = getPlateNumber(city, cities);
                    int currentCityDistance = citiesDistances[currentCityPlate, izmirPlate];
                    if (compareDistance > currentCityDistance)
                    {
                        lastCityPlate = currentCityPlate;
                        compareDistance = currentCityDistance;
                    }
                }
                routes.Add(cities[lastCityPlate]);
            }

            foreach(string route in routes)
            {
                Console.WriteLine(route);
            }


        }


        private static void Main(string[] args)
        {
            string[] cities=readCities("cities.txt");
            int[,] citiesDistances = readCityDistances("ilmesafe.xlsx");
            Hashtable neighbors = neighborsCities("neighbourCities.txt");
            int plate = getPlateNumber("Samsun", cities);
            Console.WriteLine(cities[55]);
            Hashtable citiesList= citiesObj(cities,neighbors);
            Console.WriteLine(citiesList["İZMİR"]);
            Console.WriteLine();

            Console.WriteLine(plate);
            Console.WriteLine(cities[27] + " ve " + cities[55]+" arasındaki mesafe "+citiesDistances[27,55]+" km");
            findShortestRouteToIzmir(cities, citiesDistances, neighbors);

        }
    }