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

namespace CP.Another
{
    public partial class SalessMan : Window
    {
        //Переменная для того чтобы не выбрать в качестве покупателя продавца
        int r = 0;
        public SalessMan(int reald)
        {
            //Здесь просто задаю id покупателя
            InitializeComponent();
            GetMyClients();
            r = reald;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить в базу клиента
            //first - фамилия
            //nam - имя
            //last - отчество
            //numberfhone - номер телефона
            //
            //seria - паспорт серия
            //numb - паспорт номер
            //datevidach - дата выдачи
            //kemvidan - кем выдан
            List<TextBox> textBoxes = new() { first, nam, last, numberfhone, seria, numb, datevidach, kemvidan };
            int temp = 0;
            foreach (var item in textBoxes)
            {
                if (item.Text == "" || item.Text == null)
                temp++;
            }

            if (temp > 0)
                MessageBox.Show("Не заполнено одно из полей!", "Пропущено поле", MessageBoxButton.OK, MessageBoxImage.None);
            else if(temp == 0)
            {
                string ste = first.Text + nam.Text + last.Text + kemvidan.Text + datevidach.Text + numb.Text;
                for (int j = 0; j < ste.Length; j++)
                {
                    if (char.IsDigit(ste[j]))
                        temp++;
                        break;
                }
                ste = numberfhone.Text + seria.Text;
                for (int i = 0; i < ste.Length; i++)
                {
                    if (!char.IsDigit(ste[i]))
                        temp++;
                }
                if (temp > 0)
                    MessageBox.Show("Текстовые поля не должны содержать цифры\nЧисловые поля буквы", "Некорректно заполнено одно из полей!", MessageBoxButton.OK, MessageBoxImage.None);
                else if (temp == 0)
                {
                    //Проверки прошли, теперь добавим клиента в базу

                    
                }
            }
            
           
        }



        //Общая загрузка списка клиентов агенства (Без нареканий)
        private void GetMyClients()
        {
            
            poisk.Focus();
            using (RealContext db = new())
            {
                var getget = from clie in  db.Clients.AsNoTracking().ToList()
                             join pass in  db.Passports.AsNoTracking().ToList() on clie.PasswordFk equals pass.Id
                             where clie.Id != r
                             select new
                             {
                                 clie.Id,
                                 f = clie.Firstname,
                                 n = clie.Name,
                                 l = clie.Lastname,
                                 nf = clie.Numberphone,
                                 s = pass.Serial,
                                 number = pass.Number,
                                 dv = pass.Dateof,
                                 kv = pass.Isby
                             };
                listviewCards.ItemsSource = getget.ToList();
               
            }
        }

        //Динамический поиск (Без нареканий)
        private async void PoissKK(object sender, KeyEventArgs e)
        {
            //Динамический поиск
            string str = poisk.Text.ToLower().Replace(" ", "");
            using (RealContext db = new())
            {
                var getget = from clie in await db.Clients.AsNoTracking().ToListAsync()
                             join pass in await db.Passports.AsNoTracking().ToListAsync() on clie.PasswordFk equals pass.Id
                             where    clie.Firstname.ToLower().Contains(str) 
                                   || clie.Name.ToLower().Contains(str)
                                   || clie.Lastname.ToLower().Contains(str)
                                   && clie.Id != r
                             select new
                             {
                                 clie.Id,
                                 f = clie.Firstname,
                                 n = clie.Name,
                                 l = clie.Lastname,
                                 nf = clie.Numberphone,
                                 s = pass.Serial,
                                 number = pass.Number,
                                 dv = pass.Dateof,
                                 kv = pass.Isby
                             };
                listviewCards.ItemsSource = getget.ToList();
            }
        }

        //Получение id покупателя при двойном клике на любую запись в таблице(без нареканий)
        private async void GetSalesDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var str = GetMyId(listviewCards.SelectedItem.ToString());
            MainWindow.salesmanhik = Convert.ToInt32(str);
            using(RealContext db = new())
            {
                var ddd = await db.Clients.AsNoTracking().Where(u => u.Id == MainWindow.salesmanhik).ToListAsync();
                foreach (var item in ddd)
                {
                    str = $"{item.Firstname} {item.Name} {item.Lastname}";
                }
            }
            MessageBoxResult dialog = MessageBox.Show($"Добавить покупателя: {str}?", "Вы уверены в своем выборе?!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(dialog == MessageBoxResult.Yes)
            {
                Close();
            }
            else if(dialog == MessageBoxResult.No)
            {
                return;
            }

            static string GetMyId(string dsa)
            {
                string s = "";
                for (int i = 0; i < dsa.Length; i++)
                {
                    if (char.IsDigit(dsa[i]))
                        s += dsa[i];
                    else if (dsa[i] == ',')
                        break;
                }
                return s;
            }
        }
    }
}
