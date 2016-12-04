using DataImport.Excel;
using DataImport.Mapping;
using ExcelReader.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace DataImport.Tests.Excel
{
    [TestClass]
    public class ExcelDataLoderTests
    {
        [TestMethod]
        public void CanCorrectlyImportXlsxFiles()
        {
            var excelFileInfo = new ExcelFileInfo(new FileInfo(@"Excel\Resources\People.xlsx"));
            var mapping = new MappingRules<Person, int>();

            mapping.AddMapping(0, p => p.Name);
            mapping.AddMapping(1, p => p.FriendsCount);
            mapping.AddMapping(2, p => p.DateOfBirth);
            mapping.AddMapping(3, p => p.Sex, value => Enum.Parse(typeof(Gender), value as string));

            var dataLoader = new ExcelDataLoader<Person>(excelFileInfo, mapping);
            var result = dataLoader.LoadData();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }

        [TestMethod]
        public void CanCorrectlyImportXlsFiles()
        {
            var excelFileInfo = new ExcelFileInfo(new FileInfo(@"Excel\Resources\People.xls"));
            var mapping = new MappingRules<Person, int>();

            mapping.AddMapping(0, p => p.Name);
            mapping.AddMapping(1, p => p.FriendsCount);
            mapping.AddMapping(2, p => p.DateOfBirth);
            mapping.AddMapping(3, p => p.Sex, value => Enum.Parse(typeof(Gender), value as string));

            var dataLoader = new ExcelDataLoader<Person>(excelFileInfo, mapping);
            var result = dataLoader.LoadData();

            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count());
        }
    }
}
