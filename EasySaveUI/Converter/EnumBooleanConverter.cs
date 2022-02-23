using Avalonia.Data.Converters;
using EasySaveUI.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EasySaveUI.Converter
{
    public class EnumBooleanConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SaveType saveType = (SaveType)System.Enum.Parse(typeof(SaveType), value.ToString());

            string? param = parameter.ToString();
            switch (param)
            {
                case "Partial":
                    if (saveType == SaveType.Differential)
                        return true;
                    break;
                case "Complete":
                    if (saveType == SaveType.Complete)
                       return true;
                    break;
            }
           
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string? param = parameter.ToString();
            bool ischecked = (bool)value;
            switch (param)
            {
                case "Partial":
                    if (ischecked)
                        return SaveType.Differential;
                    break;
                case "Complete":
                    if (ischecked)
                        return SaveType.Complete;
                    break;
            }
            return default(SaveType);
                
            #endregion

        }
    }
}
