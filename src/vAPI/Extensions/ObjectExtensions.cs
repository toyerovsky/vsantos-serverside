using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace VRP.vAPI.Extensions
{
    public static class ObjectExtensions
    {
        public static void UpdateFieldsFromJson(this object obj, string json)
        {
            Type type = obj.GetType();
            foreach (var jsonProperty in JObject.Parse(json).Properties())
            {
                if (type.GetProperties().Any(prop => prop.Name == jsonProperty.Name))
                {
                    PropertyInfo property = type.GetProperty(jsonProperty.Name);
                    property.SetValue(obj, jsonProperty.Value);
                }
            }
        }
    }
}
