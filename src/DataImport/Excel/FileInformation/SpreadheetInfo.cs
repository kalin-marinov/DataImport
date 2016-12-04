using System;

namespace DataImport.Excel
{
    public abstract class SpreadheetInfo
    {
        protected string sheetName;
        public ExcelFileExtension Extension { get; protected set; }

        /// <summary>
        /// Information for an excel file
        /// </summary>
        /// <param name="excelFile"> The full path of the fail, containing the extension </param>
        /// <param name="sheetName"> The excel sheet from which the data will be extracted </param>
        /// <param name="hasHeaders"> Whether the first row of the file contains the column headers </param>
        /// <param name="selectedColumns"> Which columns to be loaded out of the file </param>
        public SpreadheetInfo(ExcelFileExtension extension = ExcelFileExtension.xls, bool hasHeaders = true, string sheetName = "Sheet1")
        {
            this.SheetName = sheetName;
            this.HasHeaders = hasHeaders;
            this.Extension = extension;
        }

        public string SheetName
        {
            get { return this.sheetName; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The sheet name cannot be null or empty");
                }

                this.sheetName = value;
            }
        }

        public bool HasHeaders { get; protected set; }
    }
}
