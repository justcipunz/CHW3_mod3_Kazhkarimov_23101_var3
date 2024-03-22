using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataProcessing;

namespace CHW3_mod3_Kazhkarimov_23101_var3;

/// <summary>
/// Class allowing to sort lists of attractions by several criterions.
/// </summary>
public class SortingData
{
    /// <summary>
    /// Comparator for sorting list of attractions.
    /// </summary>
    /// <param name="x"> First element to compare. </param>
    /// <param name="y"> Second element to compare. </param>
    /// <returns> The result of comparing two attraction interpreted as int number. </returns>
    private static int Comparator(Attraction x, Attraction y) => string.Compare(x.AdmArea, y.AdmArea);

    /// <summary>
    /// Sorts list of attractions by AdmArea field.
    /// </summary>
    /// <param name="data"> List of attractions. </param>
    /// <param name="isReverseRequired"> Determines whether reversed list required. </returns>
    public static List<Attraction> SortByAdmArea(List<Attraction> data, bool isReverseRequired = false)
    {
        Logger.WriteStartLog(nameof(SortByAdmArea));

        List<Attraction> sortedData = new List<Attraction>(data);

        sortedData.Remove(data[0]);
        sortedData.Remove(data[1]);

        sortedData.Sort(Comparator);
        if (isReverseRequired)
            sortedData.Reverse();

        sortedData.Insert(0, data[1]);
        sortedData.Insert(0, data[0]);

        Logger.WriteStopLog(nameof(SortByAdmArea));
        return sortedData;
    }
}
