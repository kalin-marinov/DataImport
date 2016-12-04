using System.Collections.Generic;

namespace DataImport
{
    public interface IDataLoader<T>
    {
        /// <summary>
        /// Loads the data into an enumerable of the model type
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> LoadData();
    }
}
