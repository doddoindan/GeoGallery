using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Microsoft.Phone;
using System.IO;
using Microsoft.Phone.Controls.Maps;
using System.Device.Location;
using System.Linq;
using System.Collections.Generic;
using System.Threading;

namespace GeoGallery
{
         
        
    
        public partial class MainPage
        {
            public static int PushWidth = 50;
            public static int PushHeigh = 50;



            #region Асинхронная загрузка изображений
            private void StartProcessImage(Action<int> callback) {
              var _Worker = new BackgroundWorker();
              _Worker.WorkerReportsProgress = true;
              _Worker.DoWork += (s, e) => { e.Result = GetPins(s, e); };
              _Worker.ProgressChanged += new ProgressChangedEventHandler(_Worker_ProgressChangedImage);
              
              _Worker.RunWorkerCompleted += (s, e) => { callback.Invoke((int)e.Result);  };
              _Worker.RunWorkerAsync();          
          }

          void _Worker_ProgressChangedImage(object sender, ProgressChangedEventArgs e)
          {
              //************************************************
              //**********генерим byte[] из потока  
              var str = ((e.UserState as object[])[0]) as Stream;

          
              PushpinData pd = (PushpinData)((e.UserState as object[])[1]);
              
              if ((str != null) && ((e.UserState as object[])[1] != null))
              {                 
                 pd.ImgSource = ImageConverter.ConvertToBytes(PictureDecoder.DecodeJpeg(str, PushWidth, PushHeigh));                                 
              }
              if ((e.UserState as object[])[1] != null){                  
                  PushpinDataList.Add(pd);
              }              
              //************************************************************************
              //прогресс
              this.textFilesCount.Text = (string)((e.UserState as object[])[2]);
              this.pbProcess.Value = e.ProgressPercentage;
          }
          void CallbackImage(int value) {

              this.pbProcess.Visibility = System.Windows.Visibility.Collapsed;
              this.textFilesCount.Text = "";//System.Windows.Visibility.Collapsed;
              FinalizeSettings();
              if (value == 0) {
              }else{
                  MessageBox.Show("something wrong");
              }            
     
          }
           #endregion

              //*******************************************
            #region События на карте




          private void googlemap_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
          {
              
              var fe = e.OriginalSource as FrameworkElement;
              //MessageBox.Show(e.OriginalSource.GetType().ToString());
              //MessageBox.Show(fe.DataContext.ToString());
              if (fe.DataContext is string)
              {
                 // List<Pushpin> lp = (List<Pushpin>)fe.Tag;
                //  MessageBox.Show(fe.DataContext.ToString());
              }

              if (fe.DataContext is IEnumerable<object>)
              {
                  //itemList.ItemsSource = (fe.DataContext as IEnumerable<object>).Cast<string>();
              }

          }    


            
            #endregion





        }
    }
