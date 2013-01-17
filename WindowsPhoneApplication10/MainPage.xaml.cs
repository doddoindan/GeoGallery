
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

using System.Threading;
using System.ComponentModel;






namespace GeoGallery
{


   
    
    public partial class MainPage : PhoneApplicationPage
    {                  
        
        // поток для обработки изображений
     

        
        

        // Надо ли?
        public static int PushWidth = 50;
        public static int PushHeigh = 50;
        // изолирпованное хранилище
        readonly IsolatedStorageSettings _settings =
                IsolatedStorageSettings.ApplicationSettings;
        

        //Паблик лист с пушпинами
        
        List<Pushpin> PushpinList = new List<Pushpin>();
        List<PushpinData> PushpinDataList = new List<PushpinData>();
        

        // функция заполнения списка пушпинов

        public MainPage()
        {
            InitializeComponent();
            /* gt = new GoogleTile();
             googlemap.ZoomBarVisibility = System.Windows.Visibility.Visible;*/
            /*
            StartProcessCompleted += GetPins_StartProcessCompleted;
            Loaded += (s, e) => { StartProcess(); };
            */
            InitializeSettings();
            //MessageBox.Show(PushpinDataList.Count().ToString());
            Loaded += (s, e) => { StartProcess(Callback); };

            PushpinList.SetPushPinData(PushpinDataList);
            
            /*foreach (var p in PushpinList)
            {
                googlemap.Children.Add(p);    
            }*/
        }      

        private void InitializeSettings()
        {

            if (_settings.Contains("PushPinsList"))
            {
               // PushpinDataList = (List<PushpinData>)_settings["PushPinsList"];
            }
            
        }

        private void FinalizeSettings() {
            
            PushpinList.SetPushPinData(PushpinDataList);

            _settings["PushPinsList"] = PushpinDataList;            
                _settings.Save();
                
        }




        public  int GetPins(object s, DoWorkEventArgs e )/*(Object _pbProcess, Object _PushpinDataList)*/
        {
            // наш поток 
            BackgroundWorker b = s as BackgroundWorker;            
            
            MediaLibrary ml = new MediaLibrary();
            
            // Делаем выборку
            var lnq = from mz in ml.Pictures 
                      
                      where 
                          (( mz.Date >= DateTime.Today.AddDays(-3) )  &&

                            ((from pl in PushpinDataList where pl.Name == mz.Name select pl.Name).Count()==0 ))                          
            select mz;


            // данные для прогрессбара
            double i = (100.0 / lnq.Count());
            double p = 0;
            int cnt = lnq.Count();
            //*************************
            foreach (Picture pic in lnq)
            {               
                p = p + i;// прогрессбар               

                // вытягиваем экзиф 
                JpegInfo info = ExifLib.ExifReader.ReadJpeg(pic.GetImage(), pic.Name);                
                // если долгота и широта не ноль                
                PushpinData pd = new PushpinData();                    
                pd.Name = pic.Name;
                pd.GC = new GeoCoordinate() { Longitude = info.GetLongitude(), Latitude = info.GetLatitude(), Altitude = 0 };

                
                if ((info.GetLatitude()) != 0 && (info.GetLongitude()) != 0)
                {
                    // передаём объект ПД и поток, чтобы преобразовать его в UI потоке
                    b.ReportProgress((int)p, new object[] { pic.GetThumbnail(), pd, string.Format("{1}/{0}", cnt, lnq.Count()) });
                }
                else {
                    b.ReportProgress((int)p, new object[] { null, null, string.Format("{1}/{0}", cnt, lnq.Count()) });
                }
                //PushpinDataList.Add(pd);
                // добавляем
                
            }
            return 0;
        }

         

       
        private void PushClick(object Object, MouseButtonEventArgs e)
        {    
            Pushpin p = (Pushpin)Object;
            MessageBox.Show(p.Tag.ToString());                      
            
        }



    }

                      

        }

