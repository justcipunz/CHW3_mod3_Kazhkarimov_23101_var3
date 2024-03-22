using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Types;
using static CHW3_mod3_Kazhkarimov_23101_var3.ConstantProperties;
using DataProcessing;

namespace CHW3_mod3_Kazhkarimov_23101_var3;

public class BotHandler
{
    private readonly string token; 
    private static Stream lastDownloadedJson, lastDownloadedCsv;
    private static Stream lastUploadedJson, lastUploadedCsv;
    private static List<Attraction> lastUpdatedFile; 
    private static TelegramBotClient botClient;

    public BotHandler(string token)
    {
        this.token = token;
    }

    /// <summary>
    /// Downloads data from user.
    /// </summary>
    /// <param name="update"> User query. </param>
    private async Task DownloadData(Update update)
    {
        Logger.WriteStartLog(nameof(DownloadData));

        string fileName = update.Message.Document.FileName;
        if (fileName.EndsWith(".csv"))
        {
            lastDownloadedCsv = await Download(botClient, update, "csv");
            lastUpdatedFile = CSVProcessing.Read(lastDownloadedCsv);

            lastUploadedCsv = CSVProcessing.Write(lastUpdatedFile, OutputPathCSV);            
            lastUploadedJson = JSONProcessing.Write(lastUpdatedFile, OutputPathJSON);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else if (fileName.EndsWith(".json"))
        {
            lastDownloadedJson = await Download(botClient, update, "json");
            lastUpdatedFile = JSONProcessing.Read(lastDownloadedJson);
            lastUploadedCsv = CSVProcessing.Write(lastUpdatedFile, OutputPathCSV);
            lastUploadedJson = JSONProcessing.Write(lastUpdatedFile, OutputPathJSON);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
        }

        Logger.WriteStopLog(nameof(DownloadData));
    }

    /// <summary>
    /// Saves data changed by user.
    /// </summary>
    /// <param name="update"> User query. </param>
    /// <param name="editedTable"> Edited list of attractions. </param>
    private async Task CompleteEditingTask(Update update, List<Attraction> editedTable)
    {
        Logger.WriteStartLog(nameof(CompleteEditingTask));

        if (editedTable.Count == 2)
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, UnluckyMessage, replyMarkup: Buttons.GetMenuButtons());
        }
        else
        {
            lastUploadedCsv = CSVProcessing.Write(editedTable, OutputPathCSV);
            lastUploadedJson = JSONProcessing.Write(editedTable, OutputPathJSON);
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, SuccessfulSaveMessage, replyMarkup: Buttons.GetMenuButtons());
        }

        Logger.WriteStopLog(nameof(CompleteEditingTask));
    }

    /// <summary>
    /// Handles user messages.
    /// </summary>
    /// <param name="update"> User message. </param>
    private async void ProcessUpdateAsync(Update update)
    {
        Logger.WriteStartLog(nameof(ProcessUpdateAsync));

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var command = update.Message.Text;
            if (command == null && update.Message.Type != Telegram.Bot.Types.Enums.MessageType.Document)
            {
                await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
                return;
            }

            if (lastUpdatedFile is null) // Поведение программы, если пользователь еще не загрузил таблицу с данными.
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document)
                {
                    try
                    {
                        await DownloadData(update);
                    }
                    catch (Exception ex)
                    {
                        Logger.WriteErrorLog(nameof(ProcessUpdateAsync), ex);
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, ErrorMessage + ex.Message);
                    }
                }
                else
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat.Id, StoppedMessage);
                }
            }
            else // Поведение программы, если пользователь загрузил таблицу с данными.
            {
                if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Document) // Если сообщение - документ.
                {
                    await DownloadData(update);
                    return;
                }

                // Если пользователь хочет произвести выборку, ему необходимо в нужном формате прислать запрос.
                if (command.StartsWith(FilterButtonText1))
                {
                    if (!command.Contains('"'))
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, InvalidQueryMessage);
                        return;
                    }
                    List<Attraction> editedTable = FilteringData.FilterByOneCriterion(lastUpdatedFile, command);
                    await CompleteEditingTask(update, editedTable);
                    return;
                }
                else if (command.StartsWith(FilterButtonText2))
                {
                    if (!command.Contains('"'))
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, InvalidQueryMessage);
                        return;
                    }
                    List<Attraction> editedTable = FilteringData.FilterByOneCriterion(lastUpdatedFile, command);
                    await CompleteEditingTask(update, editedTable);
                    return;
                }
                else if (command.StartsWith(FilterButtonText3))
                {
                    if (!command.Contains('"'))
                    {
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, InvalidQueryMessage);
                        return;
                    }
                    List<Attraction> editedTable = FilteringData.FilterByTwoCriterions(lastUpdatedFile, command);
                    await CompleteEditingTask(update, editedTable);
                    return;
                }

                // Основное меню команд, если в таблице есть данные.
                switch (command)
                {
                    case MenuButtonText1:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, InputMessage, replyMarkup: Buttons.GetMenuButtons());
                            break;
                        }

                    case MenuButtonText2:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseFilterMessage);
                            break;
                        }

                    case MenuButtonText3:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseCommandMessage, replyMarkup: Buttons.GetSortingButtons());
                            break;
                        }

                    case MenuButtonText4:
                        {
                            await botClient.SendTextMessageAsync(update.Message.Chat.Id, ChooseCommandMessage, replyMarkup: Buttons.GetOutputButtons());
                            break;
                        }

                    case SortingButtonText1:
                        {
                            List<Attraction> editedTable = SortingData.SortByAdmArea(lastUpdatedFile);
                            await CompleteEditingTask(update, editedTable);
                            break;
                        }

                    case SortingButtonText2:
                        {
                            List<Attraction> editedTable = SortingData.SortByAdmArea(lastUpdatedFile, true);
                            await CompleteEditingTask(update, editedTable);
                            break;
                        }

                    case OutputButtonText1:
                        {
                            await Upload(botClient, update, lastUploadedJson, "json");
                            break;
                        }

                    case OutputButtonText2:
                        {
                            await Upload(botClient, update, lastUploadedCsv, "csv");
                            break;
                        }

                    default:
                        await botClient.SendTextMessageAsync(update.Message.Chat.Id, CommandErrorMessage, replyMarkup: Buttons.GetMenuButtons());
                        break;
                }
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(update.Message.Chat.Id, TypeErrorMessage);
        }

        Logger.WriteStopLog(nameof(ProcessUpdateAsync));
    }

    /// <summary>
    /// Gets new updates from user.
    /// </summary>
    public void GetUpdates()
    {
        Logger.WriteStartLog(nameof(GetUpdates));

        botClient = new TelegramBotClient(token);
        var me = botClient.GetMeAsync().Result;
        if (me != null && !string.IsNullOrEmpty(me.Username))
        {
            int offset = 0;
            while (true)
            {
                try
                {
                    var updates = botClient.GetUpdatesAsync(offset).Result;
                    if (updates != null && updates.Count() > 0)
                    {
                        foreach (var update in updates)
                        {
                            ProcessUpdateAsync(update);
                            offset = update.Id + 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.WriteErrorLog(nameof(GetUpdates), ex);
                }
                Thread.Sleep(1000);
            }
        }

        Logger.WriteStopLog(nameof(GetUpdates));
    }

    /// <summary>
    /// Downloads data from user to file.
    /// </summary>
    /// <param name="botClient"> Bot client. </param>
    /// <param name="update"> User query. </param>
    /// <returns></returns>
    public static async Task<Stream> Download(ITelegramBotClient botClient, Update update, string fileType)
    {
        Logger.WriteStartLog(nameof(Download));

        var fileId = update.Message.Document.FileId;
        string destinationFilePath = DataPath + $"{Path.DirectorySeparatorChar}LastInput.{fileType}";

        Stream fileStream = System.IO.File.Create(destinationFilePath);
        using (fileStream)
        {
            await botClient.GetInfoAndDownloadFileAsync(fileId: fileId, destination: fileStream);
        }

        Logger.WriteStopLog(nameof(Download));
        return new FileStream(destinationFilePath, FileMode.Open);
    }

    /// <summary>
    /// Uploads file to user.
    /// </summary>
    /// <param name="botClient"> Bot client. </param>
    /// <param name="update"> User query. </param>
    /// <param name="stream"> Output stream. </param>
    public static async Task Upload(ITelegramBotClient botClient, Update update, Stream stream, string fileType)
    {
        Logger.WriteStartLog(nameof(Upload));

        Message message = await botClient.SendDocumentAsync(
            chatId: update.Message.Chat.Id,
            document: InputFile.FromStream(stream: stream, fileName: $"Table.{fileType}"),
            replyMarkup: Buttons.GetMenuButtons());

        Logger.WriteStopLog(nameof(Upload));
    }
}