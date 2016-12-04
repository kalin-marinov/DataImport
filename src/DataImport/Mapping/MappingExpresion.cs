using System;
using System.Linq.Expressions;

namespace DataImport.Mapping
{
    public class MappingExpresion<T>
    {
        public  Expression<Func<T, object>> Expression { get; set; }

        public Func<object, object> ParseFunction { get; set; }
    }
}
