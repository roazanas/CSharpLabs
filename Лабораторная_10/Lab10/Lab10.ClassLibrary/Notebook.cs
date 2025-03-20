using System;
using Lab10.ClassLibrary.Utils;

namespace Lab10.ClassLibrary
{
    public class Notebook : IInit
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }

        public Notebook()
        {
            Title = "Без заголовка";
            Content = "Пусто";
            CreationDate = DateTime.Now;
        }

        public void CopyFrom(Notebook other)
        {
            Title = other.Title;
            Content = other.Content;
            CreationDate = other.CreationDate;
        }

        public void Init()
        {
            CopyFrom(DocumentGenerator.CreateNotebookFromUserInput());
        }

        public void RandomInit()
        {
            CopyFrom(DocumentGenerator.GenerateRandomNotebook());
        }

        public override string ToString()
        {
            return $"<CYAN>Заметка<WHITE>: {Title} | {Content} | Создано: {CreationDate:dd.MM.yyyy}";
        }
    }
}
