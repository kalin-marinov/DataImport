using System;
using System.IO;
using System.Linq;
using DataImport.CSV;
using DataImport.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExcelReader.Tests.Models;
using System.Threading.Tasks;

namespace DataImport.Tests.CSV
{
    [TestClass]
    public class CsvDataLoaderTests
    {
        [TestMethod]
        public  void CanCorrectlyImportCsvFiles()
        {
            var csvFileInfo = new CsvFileInfo(new FileInfo(@"CSV\Resources\People.csv"), true);
            var mapping = new MappingRules<Person, int>();

            mapping.AddMapping(0, p => p.Name);
            mapping.AddMapping(1, p => p.FriendsCount);
            mapping.AddMapping(2, p => p.DateOfBirth);
            mapping.AddMapping(3, p => p.Sex, value => Enum.Parse(typeof(Gender), value as string));

            var dataLoader = new CsvDataLoader<Person>(csvFileInfo, mapping);
            var result = dataLoader.LoadData().ToArray();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public async Task CanCorrectlyImportCsvFilesAsync()
        {
            var csvFileInfo = new CsvFileInfo(new FileInfo(@"CSV\Resources\People.csv"), true);
            var mapping = new MappingRules<Person, int>();

            mapping.AddMapping(0, p => p.Name);
            mapping.AddMapping(1, p => p.FriendsCount);
            mapping.AddMapping(2, p => p.DateOfBirth);
            mapping.AddMapping(3, p => p.Sex, value => Enum.Parse(typeof(Gender), value as string));

            var dataLoader = new CsvDataLoader<Person>(csvFileInfo, mapping);
            var result = (await dataLoader.LoadDataAsync()).ToArray();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void CanCorrectlyImportCsvFilesWithoutHeaders()
        {
            var csvFileInfo = new CsvFileInfo(new FileInfo(@"CSV\Resources\People_NoHeaders.csv"), hasHeaders: false);
            var mapping = new MappingRules<Person, int>();

            mapping.AddMapping(0, p => p.Name);
            mapping.AddMapping(1, p => p.FriendsCount);
            mapping.AddMapping(2, p => p.DateOfBirth);
            mapping.AddMapping(3, p => p.Sex, value => Enum.Parse(typeof(Gender), value as string));

            var dataLoader = new CsvDataLoader<Person>(csvFileInfo, mapping);
            var result = dataLoader.LoadData().ToArray();


            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }
    }
}
