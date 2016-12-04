using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DataImport
{
    public static class PropertyExtensions
    {
        public static void SetPropertyValue<T>(this T target, Expression<Func<T, object>> memberLamda, object value)
        {
            var memberSelectorExpression = memberLamda.Body as MemberExpression;
            if (memberSelectorExpression != null)
            {
                var property = memberSelectorExpression.Member as PropertyInfo;
                SetPropertyValue<T>(target, value, property);
            }
            else
            {
                var unaryExpression = memberLamda.Body as UnaryExpression;
                if (unaryExpression != null)
                {
                    var property = (unaryExpression.Operand as MemberExpression).Member as PropertyInfo;
                    SetPropertyValue<T>(target, value, property);
                }
            }
        }

        public static void SetPropertyValue<T>(this T target, object value, PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentException("Expected property was not found", property.Name);
            }

            try
            {
                var type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var convertedValue = (value == null) ? null : Convert.ChangeType(value, type);
                property.SetValue(target, convertedValue);
            }
            catch (Exception ex)
            {
                string message = string.Format("Failed to set the value of {0}. The given value: {1} was incorrect for that field",
                    property.Name, value);

                throw new ArgumentException(message, ex);
            }
        }
    }
}
