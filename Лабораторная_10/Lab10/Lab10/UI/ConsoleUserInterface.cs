using Lab10.ClassLibrary;

namespace Lab10.UI
{
    public class ConsoleUserInterface : IUserInterface
    {
        public void WriteColored(string text)
        {
            ColorWriteManager.HandleColorStr(text);
        }

        public string ReadLine()
        {
            return Console.ReadLine() ?? string.Empty;
        }

        public int ReadInt()
        {
            return UserIO.ReadUserInt();
        }

        public DateTime ReadDate()
        {
            WriteColored("<WHITE>Введите дату (в формате <MAGENTA>дд<WHITE>.<MAGENTA>мм<WHITE>.<MAGENTA>гггг<WHITE>): ");
            return UserIO.ReadUserDate();
        }

        public int ReadIntInArray(int[] validValues)
        {
            return UserIO.ReadUserIntInArray(validValues);
        }

        public string ReadUserString()
        {
            return UserIO.ReadUserString();
        }
    }
}
