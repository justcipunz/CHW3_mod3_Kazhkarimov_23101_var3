using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataProcessing;

/// <summary>
/// Class for working json files.
/// </summary>
public class JSONProcessing
{
    private static int countOfOutput = 0;

    /// <summary>
    /// Converts data from list of attractions into json format 
    /// and records it to stream.
    /// </summary>
    /// <param name="attractions"> List of attractions. </param>
    /// <param name="outputPathJSON"> Output file path. </param>
    /// <returns> Output stream. </returns>
    public static Stream Write(List<Attraction> attractions, string outputPathJSON)
    {
        var options = new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = true
        };
        string jsonString = JsonSerializer.Serialize(attractions, options);

        
        string destinationFilePath = 
            outputPathJSON + $"\\output{++countOfOutput}.json";
        Stream fileStream = File.Create(destinationFilePath);
        TextWriter oldOut = Console.Out;
        using (StreamWriter sw = new StreamWriter(fileStream, Encoding.UTF8))
        {
            Console.SetOut(sw);
            Console.Write(jsonString);
        }
        Console.SetOut(oldOut);

        fileStream.Close();

        return new FileStream(destinationFilePath, FileMode.Open);
    }

    /// <summary>
    /// Reads data from input stream and converts it into list of attractions.
    /// </summary>
    /// <param name="stream"> Input stream. </param>
    /// <returns> List of attractions. </returns>
    public static List<Attraction> Read(Stream stream)
    {
        string text = "";
        TextReader oldIn = Console.In;
        using (StreamReader sr = new StreamReader(stream))
        {
            Console.SetIn(sr);
            text = sr.ReadToEnd();
        }
        Console.SetIn(oldIn);

        var attractions = JsonSerializer.Deserialize<List<Attraction>>(text);
        stream.Close();

        return attractions;
    }
}
