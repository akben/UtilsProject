using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FindChangedPropertyNamesInList
{
    internal class ObjectUtil
    {
        private static Dictionary<Type, PropertyInfo[]> _propertyCache = new Dictionary<Type, PropertyInfo[]>();
        private static readonly Type _dateType = typeof(DateTime);

        public static List<string> FindChangedPropertyNames<T>(T oldObject, T newObject)
        {
            var elementType = typeof(T);
            PropertyInfo[] properties = null;
            if (!_propertyCache.TryGetValue(elementType, out properties))
            {
                properties = elementType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                _propertyCache.Add(elementType, properties);
            }
            List<string> diffPropNames = new List<string>();
            foreach (var propInfo in properties)
            {
                if (propInfo.PropertyType == _dateType)
                {
                    var oldDate = (DateTime)propInfo.GetValue(oldObject, null);
                    var newDate = (DateTime)propInfo.GetValue(newObject, null);
                    if (!(oldDate.Year == newDate.Year &&
                        oldDate.Month == newDate.Month &&
                        oldDate.Day == newDate.Day &&
                        oldDate.Hour == newDate.Hour &&
                        oldDate.Minute == newDate.Minute &&
                        oldDate.Second == newDate.Second))
                    {
                        diffPropNames.Add(propInfo.Name);
                    }
                }
                else if (!object.Equals(propInfo.GetValue(oldObject, null), propInfo.GetValue(newObject, null)) &&
                    !diffPropNames.Contains(propInfo.Name))
                {
                    diffPropNames.Add(propInfo.Name);
                }
            }
            return diffPropNames;
        }

        public static List<string> FindChangedPropertyNamesInList<T>(IList<T> oldList, IList<T> newList)
        {
            if (oldList == null && newList == null)
            {
                return new List<string>();
            }
            if ((oldList == null && newList != null) ||
                (oldList != null && newList == null) ||
                (oldList.Count != newList.Count))
            {
                return new List<string>() { "different element counts" };
            }
            HashSet<string> diffPropNames = new HashSet<string>();
            for (int i = 0; i < oldList.Count; i++)
            {
                var oldItem = oldList[i];
                var newItem = newList[i];
                var diffPropNamesOfItem = FindChangedPropertyNames(oldItem, newItem);
                foreach (var propName in diffPropNamesOfItem)
                {
                    if (!diffPropNames.Contains(propName))
                    {
                        diffPropNames.Add(propName);
                    }
                }
            }
            return diffPropNames.ToList();
        }
    }
}
