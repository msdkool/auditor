using System;
namespace Auditor.Helper
{
    public class TypeHelper
    {
        public static bool IsCoreType(Type type)
        {
            return (type == typeof(string) || type == typeof(Int16) || type == typeof(Int32) || type == typeof(Int64) || type == typeof(decimal) || type == typeof(double) || type == typeof(Boolean) || type == typeof(string) || type == typeof(float) || type == typeof(byte) || type == typeof(char) || type == typeof(byte[]) || type == typeof(DateTime) || type == typeof(byte?) || type == typeof(int?) || type == typeof(DateTime?) || type == typeof(decimal?) || type == typeof(double?) || type == typeof(float?));
        }
    }
}
