using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace CP
{
    public partial class WindowAd : Window
    {
        private int idAD;
        public WindowAd(int id)
        {
            InitializeComponent();
            idAD = id;
            StartSale();
        }


        private async void StartSale()
        {
            await GetMyRealList();
        }

        private async Task GetMyRealList()
        {

            using (RealContext db = new())
            {
                var goquerty = from real in await db.Realties.ToListAsync()
                               join objtype in await db.ObjectTypes.ToListAsync() on real.TypeObject equals objtype.Id
                               join foto in await db.Photos.ToListAsync() on real.IdPhoto equals foto.Id
                               join owner in await db.Clients.ToListAsync() on real.Salesman equals owner.Id
                               join are in await db.Districts.ToListAsync() on real.Area equals are.Id
                               where real.Id == idAD
                               select new
                               {
                                   Name = real.NameReal,
                                   AdddressReal = $"{are.Name}, {real.Adress}",
                                   Status = $"Статус: {real.ProOrAre}",
                                   Opisan = real.Description,
                                   ClientName = owner.Name,
                                   Contact = $"Владелец: {owner.Firstname} {owner.Name} {owner.Lastname}\nТелефон: {owner.Numberphone}",
                                   Image1 = LoadImage(foto.Image1),
                                   Image2 = LoadImage(foto.Image2),
                                   Image3 = LoadImage(foto.Image3),
                                   Image4 = LoadImage(foto.Image4),
                                   Image5 = LoadImage(foto.Image5),
                                   Image6 = LoadImage(foto.Image6),
                                   Image7 = LoadImage(foto.Image7),
                                   Image8 = LoadImage(foto.Image8),
                                   Image9 = LoadImage(foto.Image9),
                                   Image10 = LoadImage(foto.Image10)
                               };
                if (goquerty != null)
                {
                    foreach (var item in goquerty)
                    {
                        Name.Text = item.Name;
                        Adddress.Text = item.AdddressReal;
                        Status.Text = item.Status;
                        Image1.Source = item.Image1;
                        Image2.Source = item.Image2;                                               
                        Image3.Source = item.Image3;                                               
                        Image4.Source = item.Image4;                                               
                        Image5.Source = item.Image5;                                               
                        Image6.Source = item.Image6;                                               
                        Image7.Source = item.Image7;                                               
                        Image8.Source = item.Image8;                                               
                        Image9.Source = item.Image9;
                        Image10.Source = item.Image10;
                        Opisan.Text = item.Opisan;
                        Contact.Text = item.Contact;
                    }
                }
                else
                    MessageBox.Show("Что то пошло не так, пинайте админа(Тимура)");
                
            }

        }




        private static BitmapImage? LoadImage(byte[] imageData)
        {
            var image = new BitmapImage();
            if (imageData != null)
            {
                using (var mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            else return null;
        }


    }
}
