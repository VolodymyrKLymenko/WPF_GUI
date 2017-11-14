using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.IO;
using System.Collections.ObjectModel;

namespace Task4
{
    public class Dish : INotifyPropertyChanged
    {
        private string detail;
        private string name;
        private float price;

        private static FileInfo _file = new FileInfo(Directory.GetCurrentDirectory() + @"\Orders.txt");

        private static int CountOrders = 0;


        public string Detail
        {
            get { return detail; }
            set
            {
                detail = value;
                OnPropertyChanged("Detail");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public float Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }


        public Dish()
        {
        }
        public Dish(string name, float price, string detail)
        {
            Name = name;
            Price = price;
            Detail = detail;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        public static ObservableCollection<Dish> ReadDishes(string _file = "None")
        {
            if (_file == "None")
                _file = (Directory.GetCurrentDirectory() + @"\Data.txt");

            ObservableCollection< Dish> res = new ObservableCollection<Dish>();

            string[] dishes = File.ReadAllLines(_file);

            foreach (string data_dish in dishes)
            {
                string[] cur_dish_data = data_dish.Split(' ');

                Dish curent_dish = new Dish();
                curent_dish.Name = cur_dish_data[0];
                curent_dish.Price = float.Parse(cur_dish_data[1]);

                for (int i = 2; i < cur_dish_data.Length; i++)
                {
                    if(i >=5 && (i % 10) == 0)
                        curent_dish.Detail += '\n';

                    curent_dish.Detail += (cur_dish_data[i] + " ");
                }

                res.Add(curent_dish);
            }

            return res;
        }

        public static void WriteDishes(ObservableCollection<Dish> dishes, string _file = "None")
        {
            if(_file == "None")
                _file = (Directory.GetCurrentDirectory() + @"\Data.txt");

            string[] str_dishes = new string[dishes.Count];

            for (int i = 0; i < dishes.Count; i++)
            {
                str_dishes[i] += (dishes[i].Name + " ");
                str_dishes[i] += (dishes[i].Price.ToString() + " ");
                str_dishes[i] += (dishes[i].Detail);
            }

            File.WriteAllLines(_file, str_dishes);
        }

        public static void WriteOrder(ObservableCollection<Dish> dishes)
        {
            ++CountOrders;

            string[] str_dishes = new string[dishes.Count];

            for (int i = 0; i < dishes.Count; i++)
            {

                str_dishes[i] += ("* " + dishes[i].Name + "(");
                str_dishes[i] += (dishes[i].Price.ToString() + ")");
            }

            string lastText;
            using (StreamReader reader = _file.OpenText())
            {
                lastText = reader.ReadToEnd();
            }

            using (StreamWriter writer = _file.CreateText())
            {
                writer.Write(lastText);

                writer.Write(CountOrders.ToString() + ")\n");

                foreach (string item in str_dishes)
                {
                    writer.WriteLine(item);
                }
            }

        }

    }

}
