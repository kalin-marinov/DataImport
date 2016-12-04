using DataImport.Excel;
using DataImport.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DataImport.CSV
{
    public class CsvDataLoader<T> : IDataLoader<T> where T : new()
    {
        private IMappingRules<T, int> rules;
        private CsvFileInfo file;

        /// <summary>
        /// Loads an IEnumerable<typeparamref name="T"> type of model </typeparamref> from the given xml file
        /// <para> String -> Property mappings are not supported </para>
        /// </summary>
        /// <param name="fileInfo"> The information for the csv file from which the data will be loaded </param>
        /// <param name="rules"> Rules mapping - should contain only int -> property mappings. Where the integer key is the index of the column </param>
        /// <exception cref="System.ArgumentException"> Thrown if the given file or mapping is null </exception>
        public CsvDataLoader(CsvFileInfo fileInfo, IMappingRules<T, int> rules)
        {
            if (rules == null)
            {
                throw new DataImportException("A proper rules mapping should be specified!");
            }

            this.file = fileInfo;
            this.rules = rules;
        }

        public IEnumerable<T> LoadData()
        {
            using (var reader = new StreamReader(new FileStream(file.FilePath.FullName, FileMode.Open)))
            {
                var tasks = new List<Task<T>>();
                int counter = 0;

                if (file.HasHeaders)
                    reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var newItem = new T();
                    var cells = line.Split(new string[] { file.ItemDelimiter }, StringSplitOptions.None);

                    foreach (var rule in rules.GetMappings())
                    {
                        object value = string.IsNullOrWhiteSpace(cells[rule.Key]) ? null : cells[rule.Key];
                        var parseFunc = rule.Value.ParseFunction;

                        try
                        {
                            if (parseFunc != null)
                                value = parseFunc(value);

                            newItem.SetPropertyValue(rule.Value.Expression, value);
                        }
                        catch (DataImportException ex)
                        {
                            if (ex.Message != null)
                            {
                                var exText = "The row with incorrect values is {0}";
                                throw new DataImportException(exText, ex, counter + 1);
                            }
                            else
                            {
                                var exText = "Error occuered on row {0}. Please ensure the data is in the right format!";
                                throw new DataImportException(exText, counter + 1);
                            }
                        }
                        catch (Exception)
                        {
                            var exText = "Error occuered on row {0}. Please ensure the data is in the right format!";
                            throw new DataImportException(exText, counter + 1);
                        }
                    }

                    counter++;
                    yield return newItem;
                }
            }
        }

        // TODO: Seriously consider Reactive Extensions or DataFlow TPL
        public async Task<IEnumerable<T>> LoadDataAsync()
        {
            using (var reader = new StreamReader(File.OpenRead(file.FilePath.FullName)))
            {
                var result = new List<T>();
                int counter = 0;

                if (file.HasHeaders)
                    await reader.ReadLineAsync();

                var line = await reader.ReadLineAsync();
                while (line != null)
                {
                    var nextLineTask = reader.ReadLineAsync();

                    var newItem = new T();
                    var cells = line.Split(new string[] { file.ItemDelimiter }, StringSplitOptions.None);

                    foreach (var rule in rules.GetMappings())
                    {
                        object value = string.IsNullOrWhiteSpace(cells[rule.Key]) ? null : cells[rule.Key];
                        var parseFunc = rule.Value.ParseFunction;

                        try
                        {
                            if (parseFunc != null)
                                value = parseFunc(value);

                            newItem.SetPropertyValue(rule.Value.Expression, value);
                        }
                        catch (DataImportException ex)
                        {
                            if (ex.Message != null)
                            {
                                var exText = "The row with incorrect values is {0}";
                                throw new DataImportException(exText, ex, counter + 1);
                            }
                            else
                            {
                                var exText = "Error occuered on row {0}. Please ensure the data is in the right format!";
                                throw new DataImportException(exText, counter + 1);
                            }
                        }
                        catch (Exception)
                        {
                            var exText = "Error occuered on row {0}. Please ensure the data is in the right format!";
                            throw new DataImportException(exText, counter + 1);
                        }


                        counter++;
                        result.Add(newItem);
                        line = await nextLineTask;
                    }

                }
                return result;
            }
        }
    }
}
