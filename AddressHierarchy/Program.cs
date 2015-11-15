using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressHierarchy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;
            ListAllLocations();
            ListCities();
            InsertAddress("C/ Mestre Joan Corrales 62","Esplugues de Llobregat");
            InsertAddress("C/ Joan de la Cierva 15", "Sant Just Desvern");
            ListAddresses();
        }

        private static void ListAllLocations()
        {
            Console.WriteLine();
            Console.WriteLine("--- START List of all locations ---");
            Console.WriteLine();

            using (var db = new addressEntities())
            {
                db.Database.Log = message => Trace.WriteLine(message);

                foreach (var country in db.Locations.OfType<Country>())
                {
                    Console.WriteLine("Country: {0}", country.Name);
                    foreach (var state in country.States.OrderBy(s => s.Name))
                    {
                        Console.WriteLine("  State: {0}", state.Name);
                        foreach (var province in state.Provinces.OrderBy(p => p.Name))
                        {
                            Console.WriteLine("    Province: {0}", province.Name);
                            foreach (var region in province.Regions.OrderBy(r => r.Name))
                            {
                                Console.WriteLine("      Region: {0}", region.Name);
                                foreach (var city in region.Cities.OrderBy(c => c.Name))
                                {
                                    Console.WriteLine("        City: {0}", city.Name);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("--- END List of all locations ---");
            Console.WriteLine();
        }

        private static void ListCities()
        {
            Console.WriteLine();
            Console.WriteLine("--- START List of all cities ---");
            Console.WriteLine();

            using (var db = new addressEntities())
            {
                db.Database.Log = message => Trace.WriteLine(message);

                var cities = db.Locations
                            .Include("Region.Province.State.Country")
                            .OfType<City>();

                if (cities == null)
                    return;

                foreach (var c in cities)
                {
                    Console.Write("City: {0}", c.Name);
                    Console.Write(" / Region: {0}", c.Region.Name);
                    Console.Write(" / Province: {0}", c.Region.Province.Name);
                    Console.Write(" / State: {0}", c.Region.Province.State.Name);
                    Console.Write(" / Country: {0}", c.Region.Province.State.Country.Name);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.WriteLine("--- END List of all cities ---");
            Console.WriteLine();
        }

        private static void InsertAddress(string line1, string cityName)
        {
            using (var db = new addressEntities())
            {
                db.Database.Log = message => Trace.WriteLine(message);

                var city = db.Locations
                            .Include("Region.Province.State.Country")
                            .OfType<City>()
                            .Where(c => c.Name == cityName)
                            .FirstOrDefault();

                if (city == null)
                    return;

                var address = new Address
                {
                    AddressLine1 = line1,
                    City = city,
                    Region = city.Region,
                    Province = city.Region.Province,
                    State = city.Region.Province.State,
                    Country = city.Region.Province.State.Country
                };

                db.Addresses.Add(address);
                db.SaveChanges();
            }
        }

        private static void ListAddresses()
        {
            Console.WriteLine();
            Console.WriteLine("--- START List of all addresses ---");
            Console.WriteLine();

            using (var db = new addressEntities())
            {
                db.Database.Log = message => Trace.WriteLine(message);

                var addresses = db.Addresses
                            .Include("Region")
                            .Include("Province")
                            .Include("State")
                            .Include("Country");

                if (addresses == null)
                    return;

                foreach (var a in addresses)
                {
                    Console.WriteLine(a.AddressLine1);
                    Console.WriteLine("  City: {0}", a.City.Name);
                    Console.WriteLine("  Region: {0}", a.Region.Name);
                    Console.WriteLine("  Province: {0}", a.Region.Province.Name);
                    Console.WriteLine("  State: {0}", a.Region.Province.State.Name);
                    Console.WriteLine("  Country: {0}", a.Region.Province.State.Country.Name);
                    Console.WriteLine();
                }
            }
            Console.WriteLine();
            Console.WriteLine("--- END List of all addresses ---");
            Console.WriteLine();
        }

    }
}
