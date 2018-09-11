/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel;
using System.Reflection;

namespace VRP.BLL.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    if (Attribute.GetCustomAttribute(field,
                        typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                    {
                        return attribute.Description;
                    }
                }
            }
            return null;
        }

        public static T FromDescription<T>(this string text) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum) throw new ArgumentException("Provided generic type must be an enum.");

            foreach (T value in Enum.GetValues(typeof(T)))
            {
                if ((string)typeof(T).GetMethod("GetDescription").Invoke(value, new object[0]) == text)
                {
                    return value;
                }
            }
            return default(T);
        }
    }
}
