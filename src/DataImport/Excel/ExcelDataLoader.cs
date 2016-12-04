using DataImport.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Excel;

namespace DataImport.Excel
{
    public class ExcelDataLoader<T> : IDataLoader<T> where T : new()
    {
        private IMappingRules<T, int> intRules;
        private SpreadheetInfo excelFile;

        /// <summary>
        /// Loads the excel data from the given file and maps them to the model according to the mapping rules
        /// </summary>
        /// <param name="excelFile"> Dataholder for information for the excel file  </param>
        /// <param name="intRules"> Mapping rules - specifies how the excel file columns will be mapped to the properties of the model. Using the index of the column (int) </param>
        public ExcelDataLoader(SpreadheetInfo excelFile, IMappingRules<T, int> intRules)
        {
            this.excelFile = excelFile;
            this.intRules = intRules;
        }


        public IEnumerable<T> LoadData()
        {
            IExcelDataReader reader;
            var result = new List<T>();

            if (excelFile is ExcelFileInfo)
            {
                reader = LoadFromFile((excelFile as ExcelFileInfo).FileName, excelFile.Extension);
            }
            else
            {
                var file = (excelFile as MemorySpreadsheedInfo);
                reader = LoadFromStream(file.FileData, excelFile.Extension);
            }

            if (excelFile.HasHeaders)
                reader.Read();


            while (reader.Read())
            {
                var test = reader.GetValue(0);

                var newItem = new T();

                foreach (var mappedItem in intRules.GetMappings())
                {
                    var value = reader.GetValue(mappedItem.Key);
                    var property = mappedItem.Value;
                    try
                    {
                        var parsedValue = property.ParseFunction != null ? property.ParseFunction(value) : value;
                        newItem.SetPropertyValue(property.Expression, parsedValue);
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != null)
                        {
                            var exText = "The row with incorrect values is {0}";
                            throw new DataImportException(exText, ex, 1);
                        }
                        else
                        {
                            var exText = "Error occuered on row {0}. Please ensure the data is in the right format!";
                            throw new DataImportException(exText, 1);
                        }
                    }

                }

                result.Add(newItem);
            }

            reader.Dispose();
            return result;
        }

        IExcelDataReader LoadFromFile(string fileName, ExcelFileExtension fileType)
          => LoadFromStream(new FileStream(fileName, FileMode.Open), fileType);


        IExcelDataReader LoadFromStream(Stream stream, ExcelFileExtension fileType)
        {
            if (fileType == ExcelFileExtension.xlsx)
                return ExcelReaderFactory.CreateOpenXmlReader(stream);

            return ExcelReaderFactory.CreateBinaryReader(stream);
        }

    }
}
