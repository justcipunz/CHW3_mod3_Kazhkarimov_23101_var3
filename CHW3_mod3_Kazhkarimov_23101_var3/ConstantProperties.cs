using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CHW3_mod3_Kazhkarimov_23101_var3;

public class ConstantProperties
{
    public const string Token = "7094618067:AAGBL7uHcglGQ69ZopmMgn1cxryH54R2BGs"; 

    public const string MenuButtonText1 = "Загрузить новый файл";
    public const string MenuButtonText2 = "Сделать выборку";
    public const string MenuButtonText3 = "Отсотировать";
    public const string MenuButtonText4 = "Скачать последний обработанный файл";

    public const string FilterButtonText1 = "Фильтрация по District";
    public const string FilterButtonText2 = "Фильтрация по LocationType";
    public const string FilterButtonText3 = "Фильтрация по AdmArea и Location";

    public const string SortingButtonText1 = "Сортировка по AdmArea в алфавитном порядке";
    public const string SortingButtonText2 = "Сортировка по AdmArea в обратном алфавитном порядке";

    public const string OutputButtonText1 = "Скачать в JSON";
    public const string OutputButtonText2 = "Скачать в CSV";

    public const string InputMessage = "Загрузите файл с расширением .json или .csv";
    public const string ErrorMessage = "Упс.. Ваш запрос сломал программу :(";
    public const string StoppedMessage = "Для использования инструментала бота необходимо загрузить файл.";
    public const string TypeErrorMessage = "Программа не поддерживает такой тип данных.";
    public const string CommandErrorMessage = "Кажется, вы ввели несуществующую команду.";
    public const string ChooseCommandMessage = "Выберите следующее действие.";
    public const string SuccessfulSaveMessage = "Данные успешно обработаны и сохранены!";
    public const string UnluckyMessage = "По данному запросу не нашлось результатов :(";
    public const string InvalidQueryMessage = "Запрос не соответствует формату. Проверьте, в кавычках ли запрашиваемое значение?";
    public const string ChooseFilterMessage = "Введите запрос в одном из следующих форматов:\n\n" +
                                          $"{FilterButtonText1} \"значение поля\"\n\n" +
                                          $"{FilterButtonText2} \"значение поля\"\n\n" +
                                          $"{FilterButtonText3} \"значение поля\" \"значение поля\"";

    public static readonly string ExecutablePath; // Путь до папки проекта.
    public static readonly string OutputPathJSON; // Путь до папки, где хранятся файлы для вывода JSON.
    public static readonly string OutputPathCSV;  // Путь до папки, где хранятся файлы для вывода CSV.
    public static readonly string DataPath;       // Путь до папки, где хранятся все файлы с данными.
    public static readonly string LogPath;        // Путь до папки, где хранятся файлы для логирования.

    /// <summary>
    /// Creates necessary directories for input and output files.
    /// </summary>
    static ConstantProperties()
    {
        char dirSep = Path.DirectorySeparatorChar;

        Process currentProcess = Process.GetCurrentProcess();
        ExecutablePath = currentProcess.MainModule.FileName;

        for (int i = 0; i < 5; ++i)
        {
            ExecutablePath = Path.GetDirectoryName(ExecutablePath);
        }

        DataPath = ExecutablePath + $"{dirSep}data";
        if (!Directory.Exists(DataPath))
        {
            Directory.CreateDirectory(DataPath);
        }

        OutputPathJSON = DataPath + $"{dirSep}JSONOutput";
        if (!Directory.Exists(OutputPathJSON))
        {
            Directory.CreateDirectory(OutputPathJSON);
        }

        OutputPathCSV = DataPath + $"{dirSep}CSVOutput";
        if (!Directory.Exists(OutputPathCSV))
        {
            Directory.CreateDirectory(OutputPathCSV);
        }

        LogPath = ExecutablePath + $"{dirSep}var";
        if (!Directory.Exists(LogPath))
        {
            Directory.CreateDirectory(LogPath);
        }
    }
}