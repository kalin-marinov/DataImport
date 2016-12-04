using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataImport.Mapping
{
    /// <summary>
    /// Provides mapping for objects of type TModel by mapping properties to keys of type TKey
    /// </summary>
    /// <typeparam name="TModel"> The type of object whose properties will be mapped </typeparam>
    /// <typeparam name="TKey"> The key to which the properties will be mapped </typeparam>
    public interface IMappingRules<TModel, TKey>
    {
        /// <summary>
        /// Maps a specified item name to a property of the model type
        /// </summary>
        /// <param name="key"> The name of the source field / item (from which the data will be taken from).
        /// It could be index of column (int) or name of column (string)  </param>
        /// <param name="property"> A projection expression for choosing the property to which this value should be assigned
        /// <para> <example> Ex: m => m.FirstName  </example>  </para>
        /// </param>
        /// <param name="parsingFunction"> A function that parses / transforms / converts the value in the column in order to fit the property of the given object  </param>
        /// <exception cref="System.ArgumentNullException"> if null item is added as a rule</exception>
        /// <exception cref="System.ArgumentException">if duplicate keys are added (when one source index is mapped to multiple destination fields)</exception>
        void AddMapping(TKey key, Expression<Func<TModel, object>> property, Func<object, object> parsingFunction = null);

        /// <summary>
        /// Returns the mapping rules specified
        /// </summary>
        IEnumerable<KeyValuePair<TKey, MappingExpresion<TModel>>> GetMappings();
    }
}
