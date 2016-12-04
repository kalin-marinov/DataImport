using System;

namespace DataImport.Excel
{
    internal class DataImportException : Exception
    {
        private Exception ex;
        private string Message;

        public int Row { get; set; }


        public DataImportException(string message) : base(message)
        {
        }

        public DataImportException(string message, int row) : this(message)
        {
            this.Row = row;
        }

        public DataImportException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DataImportException(string message, Exception inner, int row) : this(message, inner)
        {
            this.Message = message;
            this.Row = row;
        }
    }
}