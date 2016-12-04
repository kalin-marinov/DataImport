using System.IO;

namespace DataImport.Excel
{
    public class MemorySpreadsheedInfo : SpreadheetInfo
    {
        public Stream FileData { get; private set; }

        /// <summary>
        /// Information for an excel file 
        /// </summary>
        /// <param name="excelFile"> The full path of the fail, containing the extension </param>
        /// <param name="sheetName"> The excel sheet from which the data will be extracted </param>
        /// <param name="hasHeaders"> Whether the first row of the file contains the column headers </param>
        /// <param name="selectedColumns"> Which columns to be loaded out of the file </param>
        public MemorySpreadsheedInfo(Stream fileData, ExcelFileExtension fileType, bool hasHeaders = true, string sheetName = "Sheet1")
            : base(fileType, hasHeaders, sheetName)
        {
            this.FileData = fileData;
        }
    }
}
