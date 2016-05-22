﻿using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using ScreenToGif.ImageUtil;

namespace ScreenToGif.Util.Converters
{
    /// <summary>
    /// URI to BitmapImage converter.
    /// </summary>
    public class UriToBitmap : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value as string;
            var size = parameter as string;

            if (String.IsNullOrEmpty(stringValue))
                return null; //DependencyProperty.UnsetValue;

            if (!File.Exists(stringValue))
                return null;

            //BitmapImage bi = new BitmapImage();
            //bi.BeginInit();

            //if (!String.IsNullOrEmpty(size))
            //    bi.DecodePixelHeight = System.Convert.ToInt32(size);

            //bi.CacheOption = BitmapCacheOption.OnLoad;
            //bi.UriSource = new Uri(stringValue);
            //bi.EndInit();

            //return bi;

            
            if (!String.IsNullOrEmpty(size))
                return stringValue.SourceFrom(System.Convert.ToInt32(size));

            return stringValue.SourceFrom();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
