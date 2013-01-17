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

namespace GeoGallery
{
    
    
    
        public partial class MainPage
    {

          private void StartProcess(Action<string> callback) {
              var _Worker = new BackgroundWorker();
              _Worker.WorkerReportsProgress = true;
              _Worker.DoWork += (s, e) => { GetPins(s, e); e.Result = "OK";};
              _Worker.ProgressChanged += new ProgressChangedEventHandler(_Worker_ProgressChanged);
              
              _Worker.RunWorkerCompleted += (s, e) => { callback.Invoke(e.Result.ToString());  };
              _Worker.RunWorkerAsync();          
          }

          void _Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
          {
              //************************************************
              //**********генерим byte[] из потока  
              var str = ((e.UserState as object[])[0]) as Stream;
              

              if ((str != null) && ((e.UserState as object[])[1] != null))
              {

                 PushpinData pd = (PushpinData)((e.UserState as object[])[1]);    
                 pd.ImgSource = ImageConverter.ConvertToBytes(PictureDecoder.DecodeJpeg(str, PushWidth, PushHeigh));
                 //pd.Name = "ggg";
                 PushpinDataList.Add(pd);
              }              

              //****************************************************************************

              this.textFilesCount.Text = (string)((e.UserState as object[])[2]);
              this.pbProcess.Value = e.ProgressPercentage;
          }

          void Callback(string value) {
              //MessageBox.Show(value);
              this.pbProcess.Visibility = System.Windows.Visibility.Collapsed;
              this.textFilesCount.Visibility = System.Windows.Visibility.Collapsed;
              FinalizeSettings();

              
  
              foreach (var p in PushpinList)
              {
                  googlemap.Children.Add(p);
              }

          }


        /*
            public class MyEventArgs : EventArgs {
                public string value { get; set; }

            
            }          
            
            event EventHandler<MyEventArgs> StartProcessCompleted;

            private void StartProcess() {
                var _Worker = new BackgroundWorker();
                _Worker.DoWork += (s, e) => { GetPins(); }; // запускаем получение пинов

                _Worker.RunWorkerCompleted += (s, e) => { StartProcessCompleted(this, new MyEventArgs { value = e.Result.ToString() }); };
                _Worker.RunWorkerAsync();
            }

            void GetPins_StartProcessCompleted(object sender, MyEventArgs e) {
                MessageBox.Show(e.value);
                
            }
    
         */   
        }
    }
