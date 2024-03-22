using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataProcessing;

/// <summary>
/// Class for working csv files.
/// </summary>
public class CSVProcessing
{
    private static char sepCSV = ';';
    private static int ColumnsQuantity = 11;
    private static int countOfOutput = 0;

    /// <summary>
    /// Removes unnecessary symbols.
    /// </summary>
    /// <param name="line"> Text to format. </param>
    /// <returns> Formatted text. </returns>
    private static string RemoveExtraCharacters(string line) =>
        new(line.Where(sym => sym != '\"' && sym != ';').ToArray());

    /// <summary>
    /// Removes unnecessary blank strings.
    /// </summary>
    /// <param name="data"> List of strings to format. </param>
    /// <returns> List of formatted strings. </returns>
    private static List<string> DataCorrection(string[] data)
    {
        List<string> result = new List<string>();
        foreach (string line in data)
        {
            string newLine = RemoveExtraCharacters(line);
            if (result.Count < ColumnsQuantity)
            {
                result.Add(newLine);
            }
        }
        return result;
    }

    /// <summary>
    /// Converts data from list of attractions into csv format
    /// and records it to stream.
    /// </summary>
    /// <param name="attractions"> List of attractions. </param>
    /// <param name="outputPathCSV"> Output file path. </param>
    /// <returns> Output stream. </returns>
    public static Stream Write(List<Attraction> attractions, string outputPathCSV)
    {
        string destinationFilePath =
            outputPathCSV + $"\\output{++countOfOutput}.csv";
        Stream fileStream = File.Create(destinationFilePath);
        using (var sw = new StreamWriter(fileStream, Encoding.UTF8))
        {
            foreach (var att in attractions)
            {
                sw.WriteLine(att.ToString());
            }
        }
        fileStream.Close();
        return new FileStream(destinationFilePath, FileMode.Open);
    }

    /// <summary>
    /// Reads data from input stream and converts it into list of attractions.
    /// </summary>
    /// <param name="stream"> Input stream. </param>
    /// <returns> List of attractions. </returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static List<Attraction> Read(Stream stream)
    {

        int indOfLine = 1;
        List<Attraction> table = new List<Attraction>();
        using (StreamReader sr = new StreamReader(stream))
        {
            string curLine = sr.ReadLine();
            List<string> headersEng = new List<string>(DataCorrection(curLine.Split(sepCSV)));
            if (headersEng is null || headersEng.Count == 0)
            {
                throw new ArgumentNullException("Пустые английские заголовки");
            }
            ++indOfLine;

            curLine = sr.ReadLine();
            List<string> headersRus = new List<string>(DataCorrection(curLine.Split(sepCSV)));
            if (headersRus is null || headersRus.Count == 0)
            {
                throw new ArgumentNullException("Пустые русские заголовки");
            }
            ++indOfLine;

            if (headersEng.Count != ColumnsQuantity || headersRus.Count != ColumnsQuantity)
            {
                throw new ArgumentException("Неверное количество заголовков");
            }

            List<List<string>> stringData = new List<List<string>>();
            while ((curLine = sr.ReadLine()) is not null)
            {
                List<string> values = new List<string>(DataCorrection(curLine.Split(sepCSV)));
                if (values.Count == ColumnsQuantity)
                {
                    stringData.Add(values);
                }
                indOfLine++;
            }

            table.Add(new Attraction(headersEng));
            table.Add(new Attraction(headersRus));
            foreach (var row in stringData)
            {
                table.Add(new Attraction(row));
            }
        }
        stream.Close();
        return table;
    }
}
