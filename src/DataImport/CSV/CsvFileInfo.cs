using System.IO;

namespace DataImport.CSV
{
    public class CsvFileInfo
    {
        public FileInfo FilePath { get; private set; }

        public bool HasHeaders { get; private set; }

        public string ItemDelimiter { get; private set; }

        /// <summary>
        /// Information for a csv file
        /// </summary>
        /// <param name="file"> The full path of an existing .csv file </param>
        /// <param name="hasHeaders"> Whether the first row of the csv file is a header row </param>
        /// <param name="rowDelimiter"> The symbol(s) used as row separator </param>
        /// <param name="itemDelimiter"> The symbol(s) used as item (column) separator </param>
        public CsvFileInfo(FileInfo file, bool hasHeaders, string itemDelimiter, params string[] rowDelimiters)
        {
            this.FilePath = file;
            this.HasHeaders = hasHeaders;
            this.ItemDelimiter = itemDelimiter;
        }

        /// <summary>
        /// Information for a csv file
        /// </summary>
        /// <param name="file"> The full path of an existing .csv file </param>
        /// <param name="hasHeaders"> Whether the first row of the csv file is a header row </param>
        /// <param name="rowDelimiter"> The symbol(s) used as row separator </param>
        /// <param name="itemDelimiter"> The symbol(s) used as item (column) separator </param>
        public CsvFileInfo(FileInfo file, bool hasHeaders)
        {
            this.FilePath = file;
            this.HasHeaders = hasHeaders;
            this.ItemDelimiter = ";";
        }
    }
}
