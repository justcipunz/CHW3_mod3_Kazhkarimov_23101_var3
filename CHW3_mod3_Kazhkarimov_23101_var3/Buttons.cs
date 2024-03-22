using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using static CHW3_mod3_Kazhkarimov_23101_var3.ConstantProperties;

namespace CHW3_mod3_Kazhkarimov_23101_var3;

/// <summary>
/// Class determines clickable buttons displayed in telegram chat as a main part of UI.
/// </summary>
public class Buttons
{
    /// <summary>
    /// Creates and shows clickable buttons in telegram chat 
    /// necessary to choose the sorting mode.
    /// </summary>
    public static IReplyMarkup? GetSortingButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(SortingButtonText1) },
                new List<KeyboardButton> { new KeyboardButton(SortingButtonText2) }
            }
        );
    }

    /// <summary>
    /// Creates and shows clickable buttons in telegram chat 
    /// necessary to choose the ouput mode.    
    /// </summary>
    public static IReplyMarkup? GetOutputButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(OutputButtonText1) },
                new List<KeyboardButton> { new KeyboardButton(OutputButtonText2) }
            }
        );
    }

    /// <summary>
    /// Creates and shows clickable starting buttons in telegram chat.
    /// </summary>
    public static IReplyMarkup? GetMenuButtons()
    {
        return new ReplyKeyboardMarkup(
            new List<List<KeyboardButton>>
            {
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText1) },
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText2) },
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText3) },
                new List<KeyboardButton> { new KeyboardButton(MenuButtonText4) }
            }
        );
    }
}
