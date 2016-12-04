using DataImport.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.XPath;

namespace DataImport.Xml
{
    public class XmlDataLoader<T> : IDataLoader<T> where T : new()
    {
        private XPathExpression itemExpression;
        private IMappingRules<T, XPathExpression> rules;
        private FileInfo file;

        /// <summary>
        /// Loads an IEnumerable<typeparamref name="T"> type of model </typeparamref> from the given xml file
        /// </summary>
        /// <param name="xmlFile"> The information for the xml file from which the data will be loaded </param>
        /// <param name="itemExpression"> The XPath expression for selecting the items whose properties will be mapped </param>
        /// <param name="propertyRules"> Rules mapping - should contain only XPath -> property mappings. The xml items will be located by XPath and assigned to the property </param>
        /// <exception cref="System.ArgumentException"> Thrown if the given file or mapping is null </exception>
        public XmlDataLoader(FileInfo xmlFile, XPathExpression itemExpression, IMappingRules<T, XPathExpression> propertyRules)
        {
            if (propertyRules == null)
            {
                throw new ArgumentException("A proper rules mapping should be specified");
            }

            this.file = xmlFile;
            this.itemExpression = itemExpression;
            this.rules = propertyRules;
        }

        public IEnumerable<T> LoadData()
        {
            var xml = XElement.Load(file.FullName);
            var result = new LinkedList<T>();

            var items = xml.XPathSelectElements(itemExpression.Expression, xml.CreateNavigator());

            foreach (var item in items)
            {
                try
                {
                    var newItem = new T();
                    foreach (var rule in rules.GetMappings())
                    {
                        var xPath = rule.Key;
                        object value = item.XPathSelectElement(xPath.Expression, item.CreateNavigator()).Value;

                        var parseFunc = rule.Value.ParseFunction;

                        if (parseFunc != null)
                        {
                            value = parseFunc(value);
                        }

                        newItem.SetPropertyValue(rule.Value.Expression, value);
                    }

                    result.AddLast(newItem);
                }
                catch (Exception ex)
                {                    
                    throw new ArgumentException("An error occured when parsing " + item.ToString(), ex);
                }
            }
            return result;
        }
    }
}
