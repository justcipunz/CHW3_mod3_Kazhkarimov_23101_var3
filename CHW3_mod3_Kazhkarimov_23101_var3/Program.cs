using DataProcessing;
using static CHW3_mod3_Kazhkarimov_23101_var3.ConstantProperties;

namespace CHW3_mod3_Kazhkarimov_23101_var3;

internal class Program
{
    static void Main(string[] args)
    {
        try
        {
            BotHandler hlp = new BotHandler(Token);
            Logger log = new Logger(); 
            hlp.GetUpdates(); 
        }
        catch (Exception ex)
        {
            Logger.WriteErrorLog(nameof(Main), ex);
        }
    }
}