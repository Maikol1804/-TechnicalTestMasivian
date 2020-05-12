using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace RouletteWebApi.Transverse
{
    public static class Enumerators
    {

        public enum State
        {
            [Description("Ok")]
            Ok = 1,
            [Description("Error")]
            Error = 2
        }

        public enum BetTypes 
        { 
            [Description("With Number")]
            Number = 1,
            [Description("With Color")]
            Color = 2
        }

        public enum Colors
        {
            [Description("Black")]
            Black = 1,
            [Description("Red")]
            Red = 2
        }

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        if (memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() is DescriptionAttribute descriptionAttribute)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }
            return null;
        }

    }
}
