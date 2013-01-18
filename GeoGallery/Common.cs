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

    // структура для сохранения и обработки привязки фото к геолокации
    public struct PushpinData
    {
        
        public byte[] ImgSource; 
        public string Name; 
        public GeoCoordinate GC;
        //public int Group;

        
        //public ImageSource ImageSource;

        public Pushpin GetPushPin() {
            Pushpin p = new Pushpin();
            p.BorderBrush     = new SolidColorBrush(Colors.Blue);
            p.BorderThickness = new Thickness(10);
            
            p.Location = this.GC;
            p.Tag = this.Name;
            p.Content = new Image() { Source = ImageConverter.ConvertToImage(this.ImgSource), Width = 50, Height = 50 };
            p.MouseLeftButtonUp += (s, e) =>
            {
                Pushpin pp = (Pushpin)s;

                MessageBox.Show(pp.Tag.ToString());
            };           
            return p;
        }

        
        public PushpinData(ImageSource pImg, string pName, GeoCoordinate pGC) {
            Name = pName;
            GC   = pGC;
            ImgSource = ImageConverter.ConvertToBytes(pImg as BitmapSource);
            //Group = -1;
            
            //ImageSource = pImg;
        }

        private Image GetImage(){

            MediaLibrary ml = new MediaLibrary();
            string name = this.Name;
            Stream s = (from res in ml.Pictures where res.Name == name select res.GetImage()).First();

            Image img = new Image() { Source = (ImageSource)PictureDecoder.DecodeJpeg(s, MainPage.PushWidth, MainPage.PushWidth) };
            
            return img;
        }
    }
    
    
    
    
    // Конвертер изображения в массив байт и обратно
    public sealed class ImageConverter
    {
        public static byte[] ConvertToBytes(BitmapSource bitmapImage)
        {
            if (bitmapImage == null)
            {
                return null;
            }
            WriteableBitmap image = new WriteableBitmap(bitmapImage);

            using (MemoryStream stream = new MemoryStream())
            {

                image.SaveJpeg(stream, image.PixelWidth, image.PixelHeight, 0, 100);
                return stream.ToArray();
            }
        }

        public static BitmapImage ConvertToImage(byte[] byteArray)
        {
            if (byteArray == null)
            {
                return null;
            }

            BitmapImage bitmapImage = new BitmapImage();

            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                bitmapImage.SetSource(stream);
            }
            return bitmapImage;
        }
    }


}
