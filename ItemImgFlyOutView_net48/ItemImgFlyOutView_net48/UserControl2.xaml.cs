using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItemImgFlyOutView_net48
{
    /// <summary>
    /// UserControl2.xaml 的互動邏輯
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        private string _senderName = null;

        public string ItemID = null;

        public static bool ImgHostRegister = false;
        public static bool ImgHostConnecting = false;
        private static string _hostIP = string.Empty;

        public UserControl2()
        {
            this.InitializeComponent();
        }

        public static bool ImgHostConnection(string HostIP)
        {
            var reStr = InvokeExcute(@"NET USE \\" + HostIP + @"\ItemImg /user:Administrator Ste42876046");
            var reConnection = InvokeExcute(@"NET VIEW \\" + HostIP);
            ImgHostRegister = reStr.Contains("The command completed successfully");
            ImgHostConnecting = reConnection.Contains("ItemImg     Disk  (UNC)");
            if (ImgHostRegister == false || ImgHostConnecting == false) return false;
            _hostIP = HostIP;
            return true;
        }

        public void Load_Item_Image()
        {
            if (ItemID == null) return;
            FlyOutItemID_Str.Text = "物料編號:" + ItemID;
            string hostpath = @"\\" + _hostIP + @"\ItemImg\" + ItemID + @"\\";
            string[] imagePath =
            {
                hostpath +"1.png", hostpath + "2.png", hostpath + "3.png", hostpath + "4.png", hostpath + "5.png"
            };
            //FileInfo fi = new FileInfo(imagePath[0]);
            //bool exists = fi.Exists;
            #region 載入圖片區域
            if (File.Exists(imagePath[0]))
            {
                ImageShow.Source = BitmapFromUri(new Uri(imagePath[0], UriKind.Absolute));
            }
            else
            {
                ImageShow.Source = null;
            }
            if (new FileInfo(imagePath[0]).Exists)
            {
                Item_Image1.Source = BitmapFromUri(new Uri(imagePath[0], UriKind.Absolute));
            }
            else
            {
                Item_Image1.Source = null;
            }
            if (new FileInfo(imagePath[1]).Exists)
            {
                Item_Image2.Source = BitmapFromUri(new Uri(imagePath[1], UriKind.Absolute));
            }
            else
            {
                Item_Image2.Source = null;
            }
            if (new FileInfo(imagePath[2]).Exists)
            {
                Item_Image3.Source = BitmapFromUri(new Uri(imagePath[2], UriKind.Absolute));
            }
            else
            {
                Item_Image3.Source = null;
            }
            if (new FileInfo(imagePath[3]).Exists)
            {
                Item_Image4.Source = BitmapFromUri(new Uri(imagePath[3], UriKind.Absolute));
            }
            else
            {
                Item_Image4.Source = null;
            }
            if (new FileInfo(imagePath[4]).Exists)
            {
                Item_Image5.Source = BitmapFromUri(new Uri(imagePath[4], UriKind.Absolute));
            }
            else
            {
                Item_Image5.Source = null;
            }
            #endregion
        }

        private void clearAllItemImage()
        {
            ImageShow.Source = null;
            Item_Image1.Source = null;
            Item_Image2.Source = null;
            Item_Image3.Source = null;
            Item_Image4.Source = null;
            Item_Image5.Source = null;
        }

        private void Clear_Item_Image()
        {
            if (_senderName == null) return;
            ImageShow.Source = null;
            switch (_senderName)
            {
                case "Item_Image1":
                    Item_Image1.Source = null;
                    break;
                case "Item_Image2":
                    Item_Image2.Source = null;
                    break;
                case "Item_Image3":
                    Item_Image3.Source = null;
                    break;
                case "Item_Image4":
                    Item_Image4.Source = null;
                    break;
                case "Item_Image5":
                    Item_Image5.Source = null;
                    break;
            }
        }

        private void AddNewImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            string[] filePaths = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.Multiselect = true;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == true)
            {
                filePaths = openFileDialog.FileNames;
            }
            CopyNewImgInto(filePaths);
        }

        private void CopyNewImgInto(string[] filePaths)
        {
            if (filePaths == null || ItemID == null) return;
            string Path = @"\\" + _hostIP + "\\ItemImg\\" + ItemID + "\\";
            Directory.CreateDirectory(Path);
            int i = 1;
            foreach (var row in filePaths)
            {
                Save(resizeImage(row, 1024, 768), Path + i.ToString() + ".png");

                i += 1;
            }
            Load_Item_Image();
        }

        private void ClearSelectedImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            if (ImageShow.Source == null) return;
            string ImagePath = ((System.Windows.Media.Imaging.BitmapImage)ImageShow.Source).UriSource.OriginalString;
            Clear_Item_Image();
            File.Delete(ImagePath);
        }

        private void ClearAllImg_Btn_Click(object sender, RoutedEventArgs e)
        {
            #region 含有圖片需要清除的項目
            var ImagePath = new List<string>();
            if (ImageShow.Source != null)
            {
                ImagePath.Add(((System.Windows.Media.Imaging.BitmapImage)ImageShow.Source).UriSource.OriginalString);
            }
            if (Item_Image1.Source != null)
            {
                ImagePath.Add(((System.Windows.Media.Imaging.BitmapImage)Item_Image1.Source).UriSource.OriginalString);
            }
            if (Item_Image2.Source != null)
            {
                ImagePath.Add(((System.Windows.Media.Imaging.BitmapImage)Item_Image2.Source).UriSource.OriginalString);
            }
            if (Item_Image3.Source != null)
            {
                ImagePath.Add(((System.Windows.Media.Imaging.BitmapImage)Item_Image3.Source).UriSource.OriginalString);
            }
            if (Item_Image4.Source != null)
            {
                ImagePath.Add(((System.Windows.Media.Imaging.BitmapImage)Item_Image4.Source).UriSource.OriginalString);
            }
            if (Item_Image5.Source != null)
            {
                ImagePath.Add(((System.Windows.Media.Imaging.BitmapImage)Item_Image5.Source).UriSource.OriginalString);
            }
            #endregion

            clearAllItemImage();
            foreach (var row in ImagePath)
            {
                if (String.IsNullOrWhiteSpace(row)) return;
                File.Delete(row);
            }
        }

        private void Item_Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _senderName = ((System.Windows.Controls.Image)sender).Name;
            ImageShow.Source = ((System.Windows.Controls.Image)sender).Source;
        }

        public static void Save(BitmapImage image, string filePath)
        {
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }
        }

        public static BitmapImage resizeImage(string path, int new_height, int new_width)
        {
            ImageBrush img_brush = new ImageBrush();
            img_brush.ImageSource = new BitmapImage(new Uri(path, UriKind.Absolute));
            BitmapImage new_image = new BitmapImage();
            new_image.BeginInit();
            new_image.UriSource = ((System.Windows.Media.Imaging.BitmapImage)img_brush.ImageSource).UriSource;
            new_image.DecodePixelWidth = 500;
            new_image.EndInit();
            return new_image;
        }

        public static ImageSource BitmapFromUri(Uri source)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = source;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        private static string InvokeExcute(string Command)
        {
            Command = Command.Trim().TrimEnd('&') + "&exit";
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;        //是否使用操作系統shell啟動
                p.StartInfo.RedirectStandardInput = true;   //接受來自調用程序的輸入信息
                p.StartInfo.RedirectStandardOutput = true;  //由調用程序獲取輸出信息
                p.StartInfo.RedirectStandardError = true;   //重定向標准錯誤輸出
                p.StartInfo.CreateNoWindow = true;          //不顯示程序窗口
                p.Start();//啟動程序
                          //向cmd窗口寫入命令
                p.StandardInput.WriteLine(Command);
                p.StandardInput.AutoFlush = true;
                //獲取cmd窗口的輸出信息
                StreamReader reader = p.StandardOutput;//截取輸出流
                string str = reader.ReadToEnd();
                p.WaitForExit();//等待程序執行完退出進程
                p.Close();
                return str;
            }
        }
    }
}
