namespace Lab12.ClassLibrary
{
    public interface IUserInterface
    {
        void WriteColored(string text);
        string ReadLine();
        string ReadUserString();
        int ReadInt();
        DateTime ReadDate();
        int ReadIntInArray(int[] validValues);
    }
}
