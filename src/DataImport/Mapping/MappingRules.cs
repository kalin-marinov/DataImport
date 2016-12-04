using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DataImport.Mapping
{
    public class MappingRules<TModel, TKey> : IMappingRules<TModel, TKey>
    {
        private Dictionary<TKey, MappingExpresion<TModel>> rules;

        public MappingRules(int rulesCount = 20)
        {
            rules = new Dictionary<TKey, MappingExpresion<TModel>>(rulesCount);
        }

        public void AddMapping(TKey key, Expression<Func<TModel, object>> property, Func<object, object> parsingFunction = null)
        {
            var mappingExpr = new MappingExpresion<TModel> { Expression = property, ParseFunction = parsingFunction };
            rules.Add(key, mappingExpr);
        }

        public IEnumerable<KeyValuePair<TKey, MappingExpresion<TModel>>> GetMappings()
        {
            return this.rules;
        }
    }
}
