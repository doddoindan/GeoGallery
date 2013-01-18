using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using System.Windows.Shapes;
using System.Globalization;
using Microsoft.Phone;
using Microsoft.Phone.Controls;

using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Info;

using Microsoft.Xna.Framework.Media;
using System.IO;
using System.IO.IsolatedStorage;

using ExifLib;
//using Googlemap;

using Microsoft.Phone.Controls.Maps;
using System.Device.Location;


using System.Runtime.Serialization;
using System.Windows.Media.Imaging;


namespace GeoGallery
{
    // Методы-расширения для класса  JpegInfo    

        

    
        static class MyExtensions
        {

            public static double GetLatitude(this JpegInfo d)
            {
                return d.GpsLatitude[0] + d.GpsLatitude[1] / 60 + d.GpsLatitude[2] / 3600;
            }

            public static double GetLongitude(this JpegInfo d)
            {
                return d.GpsLongitude[0] + d.GpsLongitude[1] / 60 + d.GpsLongitude[2] / 3600;
            }


            // Расширения для пушпинлсит
            public static List<PushpinData> GetPushPinData(this List<Pushpin> pl)
            {
                List<PushpinData> pld = new List<PushpinData>();
                foreach (var itm in pl)
                {
                    //BitmapSource bm = ;
                    pld.Add(new PushpinData() { Name = itm.Tag.ToString(), GC = itm.Location/*, ImgSource = ImageConverter.ConvertToBytes((itm.Content as Image).Source as BitmapSource)*/ });

                }

                return pld;
            }

        

            public static void SetPushPinData(this List<Pushpin> pl, List<PushpinData> pld)
            {
                pl.Clear();
                foreach (var itm in pld) {
                    pl.Add(itm.GetPushPin());
                }

            }
        }
    }
