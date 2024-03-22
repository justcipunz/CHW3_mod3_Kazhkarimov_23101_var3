using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessing;

namespace CHW3_mod3_Kazhkarimov_23101_var3;

/// <summary>
/// Class allowing to filter lists of attractions by several criterions.
/// </summary>
public class FilteringData
{
    /// <summary>
    /// Parses filter query for one criterion.
    /// </summary>
    /// <param name="message"> Query text. </param>
    /// <param name="criterion"> Filter criterion. </param>
    /// <param name="value"> Filter value. </param>
    private static void ParseFilterQueryForOneCriterion(string message, out string criterion, out string value)
    {
        string text = "Фильтрация по ";
        string[] arr = message.Remove(0, text.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        criterion = result[0];
        value = result[1];
    }

    /// <summary>
    /// Filters data by one criterion.
    /// </summary>
    /// <param name="table"> List of attractions to filter. </param>
    /// <param name="message"> Filtering format info. </param>
    /// <returns> Filtered list of attractions. </returns>
    public static List<Attraction> FilterByOneCriterion(List<Attraction> table, string message)
    {
        Logger.WriteStartLog(nameof(FilterByOneCriterion));

        var result = new List<Attraction>(table);
        ParseFilterQueryForOneCriterion(message, out string criterion, out string value);

        result.Remove(table[0]);
        result.Remove(table[1]);

        if (criterion.StartsWith("District"))
        {
            result = result.Where(row => row.District == value).ToList();
        }
        else if (criterion.StartsWith("LocationType"))
        {
            result = result.Where(row => row.LocationType == value).ToList();
        }

        result.Insert(0, table[1]);
        result.Insert(0, table[0]);

        Logger.WriteStopLog(nameof(FilterByOneCriterion));
        return result;
    }

    /// <summary>
    /// Parses filter query for one criterion.
    /// </summary>
    /// <param name="message"> Query text. </param>
    /// <param name="formatText"> Text to determine format of parsing. </param>
    /// <param name="value1"> First criterion value. </param>
    /// <param name="value2"> Second criterion value. </param>
    private static void ParseFilterQueryForTwoCriterions(string message, string formatText, out string value1, out string value2)
    {
        string[] arr = message.Remove(0, formatText.Length).Split('\"');
        var result = arr.Where(x => x.Length > 1).ToList();
        value1 = result[0];
        value2 = result[1];
    }

    /// <summary>
    /// Filters data by twp criterions.
    /// </summary>
    /// <param name="table"> List of attractions to filter. </param>
    /// <param name="message"> Filtering format info. </param>
    /// <returns> Filtered list of attractions. </returns>
    public static List<Attraction> FilterByTwoCriterions(List<Attraction> table, string message)
    {
        Logger.WriteStartLog(nameof(FilterByTwoCriterions));

        var result = new List<Attraction>(table);
        ParseFilterQueryForTwoCriterions(message, ConstantProperties.FilterButtonText3, out string value1, out string value2);

        result.Remove(table[0]);
        result.Remove(table[1]);

        result = result.Where(row => row.AdmArea == value1 && row.Location == value2).ToList();

        result.Insert(0, table[0]);
        result.Insert(1, table[1]);

        Logger.WriteStopLog(nameof(FilterByTwoCriterions));
        return result;
    }
}
