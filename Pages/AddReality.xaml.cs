using CP.Another;
using CP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace CP.Pages
{
    public partial class AddReality : Page
    {
        private static int act = 0;
        public static bool potok = true;
        public AddReality()
        {
            InitializeComponent();
            new Thread(UpDateData).Start();
        }


        //Заполнить Comboboxes данными из связанных таблиц(решено)
        private void UpDateData()
        {
            using(RealContext db = new())
            {
                //Типы объектов
                List<ObjectType> typeObj = db.ObjectTypes.FromSqlRaw("SELECT * FROM ObjectType").ToList();
                List<string> list = new();
                foreach (var type in typeObj){list.Add(type.Name);}

                //Районы
                List<District> distr = db.Districts.FromSqlRaw("SELECT * FROM Districts").ToList();
                List<string> list1 = new();
                foreach (var item in distr) {list1.Add(item.Name);}
               
                //Санузел
                List<BathroomType> sunuz = db.BathroomTypes.FromSqlRaw("SELECT * FROM BathroomType").ToList();
                List<string> list2 = new();
                foreach (var item in sunuz) { list2.Add(item.Type); }
                

                Dispatcher.Invoke(() =>
                {
                    category.ItemsSource = list;
                    rayon.ItemsSource = list1;
                    sunuzel.ItemsSource = list2;
                });
            }
        }

        //Добавить фотки
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            potok = false;
            OpenFileDialog ofdPicture = new()
            {
                Filter =
                "Images (*.BMP;*.JPG;*.GIF;*.JFIF;*.JPEG;*.PNG)|*.BMP;*.JPG;*.GIF;*.JFIF;*.JPEG;*.PNG|" +
                "All files (*.*)|*.*",
                Multiselect = true
            };
            Image ImageName;
            BitmapImage mpImg;
            List<string> images = new List<string>() { "", "", "", "", "", "", "", "", "", "" };
            if (ofdPicture.ShowDialog() == true)
            {
                panel.Children.Clear();
                int temp = 0;

                foreach (var item in ofdPicture.FileNames)
                {

                    //Список картинок(пути к ним)
                    images[temp] = item;
                    temp++;
                    ImageName = new Image();
                    mpImg = new BitmapImage();
                    mpImg.BeginInit();
                    mpImg.UriSource = new Uri(item);
                    ImageName.Width = 120;
                    ImageName.Height = 100;
                    ImageName.Source = mpImg;

                    panel.Children.Add(ImageName);
                    mpImg.EndInit();

                    if (temp == 10)
                    {
                        break;
                    }

                }
            }
            await Task.Run(() =>
            {
                try
                {
                    using (RealContext db = new())
                    {
                        Photo photo = new();

                        if (images[0].Length > 0)
                            photo.Image1 = imageBit(images[0]);

                        if (images[1].Length > 0)
                            photo.Image2 = imageBit(images[1]);

                        if (images[2].Length > 0)
                            photo.Image3 = imageBit(images[2]);

                        if (images[3].Length > 0)
                            photo.Image4 = imageBit(images[3]);

                        if (images[4].Length > 0)
                            photo.Image5 = imageBit(images[4]);

                        if (images[5].Length > 0)
                            photo.Image6 = imageBit(images[5]);

                        if (images[6].Length > 0)
                            photo.Image7 = imageBit(images[6]);

                        if (images[7].Length > 0)
                            photo.Image8 = imageBit(images[7]);

                        if (images[8].Length > 0)
                            photo.Image9 = imageBit(images[8]);

                        if (images[9].Length > 0)
                            photo.Image10 = imageBit(images[9]);

                        //db.Photos.Add(photo);
                        //db.SaveChanges();
                        MessageBox.Show("Фотографии добавлены");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Что то пошло не так");
                }
            });


            static byte[] imageBit(string str)
            {
                byte[] bytes;
                FileInfo _imgInfo = new FileInfo(str);
                long _numBytes = _imgInfo.Length;
                FileStream _fileStream = new(str, FileMode.Open, FileAccess.Read); // читаем изображение
                BinaryReader _binReader = new(_fileStream);
                bytes = _binReader.ReadBytes((int)_numBytes); // изображение в байтах
                return bytes;

            }

        }



        #region Добавить владельца(решено)
        //Добавить владельца
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            potok = true;
            new Thread(UpDateToClient).Start();
            sobstvennik.Text = "";
            SalessMan salessMan = new();
            salessMan.ShowDialog();
        }

        //Обновление данных о клиенте
        private void UpDateToClient()
        {
            while (potok)
            {
                if (act != MainWindow.salesmanhik && MainWindow.salesmanhik > 0)
                {
                    using (RealContext db = new())
                    {
                        var getmypoc = db.Clients.FromSqlRaw("SELECT * FROM Client").Where(u => u.Id == MainWindow.salesmanhik).ToList();
                        foreach (var item in getmypoc)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                sobstvennik.Text = $"{item.Firstname} {item.Name} {item.Lastname}";
                            });
                            break;
                        }
                        act = MainWindow.salesmanhik;
                    }
                }
            }
        }



        #endregion


        //Добавить объявление
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Если все ок, то поток закрываю potok
            potok= false;
        }


        //Добавить свидетельство
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            potok = false;
        }
    }
}
