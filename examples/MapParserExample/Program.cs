using GBXMapParser;
using System;
using System.Diagnostics;
using System.Reflection;

namespace MapParserExample
{
    /// <summary>
    /// Simple demo program demonstrating the result of a map parse.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main program method.
        /// </summary>
        /// <param name="args">Irrelevant arguments</param>
        static void Main(string[] args)
        {
            // Retrieve the map information for 'Tiger Blood' (most awarded map on ManiaExchange on 2019-02-15).
            string fileName = "Tiger Blood.Map.Gbx";

            // Time the parsing of the file.
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Parse the file.
            MapInformation map = MapParser.ReadFile(fileName);

            stopwatch.Stop();

            // Write short header.
            Console.WriteLine("#######################################################################################################################");
            Console.WriteLine(string.Format("Loaded '{0}' by '{1}' ('{2}')", map.Name, map.AuthorLogin, map.AuthorNickName));
            Console.WriteLine("#######################################################################################################################");

            // Write all properties of the MapInformation object.
            foreach (PropertyInfo property in typeof(MapInformation).GetProperties())
            {
                Console.WriteLine(string.Format("{0}: '{1}'", property.Name, property.GetValue(map)));
            }

            Console.WriteLine("#######################################################################################################################");
            Console.WriteLine(string.Format("Map parsed in {0}ms", stopwatch.ElapsedMilliseconds));
            Console.WriteLine("#######################################################################################################################");

            Console.ReadLine();
        }
    }
}
