using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataImport.Excel
{
    public class ExcelFileInfo : SpreadheetInfo
    {
        const string OleDBConnectionTemplate = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};"
                + "Extended Properties=\"Excel {1};HDR={2}\";";

        private string connectionString;
        private string selectCommand;
        private string fileName;

        /// <summary>
        /// Information for an excel file suitable for OLE-DB loading
        /// </summary>
        /// <param name="excelFile"> The full path of the fail, containing the extension </param>
        /// <param name="sheetName"> The excel sheet from which the data will be extracted </param>
        /// <param name="hasHeaders"> Whether the first row of the file contains the column headers </param>
        /// <param name="selectedColumns"> Which columns to be loaded out of the file </param>
        public ExcelFileInfo(FileInfo excelFile, bool hasHeaders = true,
            string sheetName = "Sheet1", IEnumerable<string> selectedColumns = null)
        {
            this.FileName = excelFile.FullName;
            this.SheetName = sheetName;
            this.HasHeaders = hasHeaders;
            var extension = excelFile.Extension.Replace(".", "");
            this.Extension = (ExcelFileExtension)Enum.Parse(typeof(ExcelFileExtension), extension);
            this.SelectedColumns = selectedColumns;
        }

        public string FileName
        {
            get { return this.fileName; }
            private set
            {        
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("The file name cannot be null or empty");
                }

                if (!File.Exists(value))
                {
                    throw new ArgumentException(string.Format("The file {0} was not found on the file system", value));
                }

                this.fileName = value;
            }
        }

        public IEnumerable<string> SelectedColumns { get; private set; }

        public string OleDbConnectionString
        {
            get
            {
                if (connectionString == null)
                {
                    connectionString = CreateConnectionString();
                }
                return connectionString;
            }
        }

        public string SelectCommandText
        {
            get
            {
                if (selectCommand == null)
                {
                    selectCommand = CreateSelectCommand();
                }
                return selectCommand;
            }
        }

        private string CreateConnectionString()
        {
            string headers = this.HasHeaders ? "YES" : "NO";
            string version = Extension == ExcelFileExtension.xlsx ? "12.0 Xml" : "8.0";

            return string.Format(OleDBConnectionTemplate, this.FileName, version, headers);
        }

        private string CreateSelectCommand()
        {
            string selectString;

            if (this.SelectedColumns == null)
            {
                selectString = "*";
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                foreach (var columnName in this.SelectedColumns)
                {
                    builder.AppendFormat("[{0}] ", columnName);
                }

                selectString = builder.ToString();
            }

            string commandText = string.Format("Select {0} from [{1}$]", selectString, this.SheetName);
            return commandText;
        }
    }
}
