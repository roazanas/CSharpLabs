using System.Collections;

namespace Lab12.ClassLibrary
{
    public class DocumentDateComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x == null || y == null)
                return 0;

            if (x is Document doc1 && y is Document doc2)
            {
                if (doc1.Date == null || doc2.Date == null)
                    return 0;

                return doc1.Date.Value.Date.CompareTo(doc2.Date.Value.Date);
            }

            throw new ArgumentException("Объекты не являются документами");
        }
    }
}
