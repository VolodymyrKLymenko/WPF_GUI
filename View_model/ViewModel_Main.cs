using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;
using System.IO;

namespace Task4
{
    public class ViewModel_Main : INotifyPropertyChanged
    {
        public ObservableCollection<Dish> SelectedDishes { get; set; }

        public ObservableCollection<Dish> Dishes { get; set; }

        private float totalPrice;
        public float TotalPrice
        {
            get
            {
                return totalPrice;
            }
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
            }
        }

        private RelayCommand selectDishCmd;
        public RelayCommand SelectDishCmd
        {
            get
            {
                return selectDishCmd ??
                    (
                        selectDishCmd = new RelayCommand(
                        (obj) => {
                            Dish dish = new Dish((Dishes[(int)obj].Name), (Dishes[(int)obj].Price), (Dishes[(int)obj].Detail));

                            TotalPrice += dish.Price;

                            SelectedDishes.Add(dish);
                        },

                        (obj) =>
                        {
                            if (obj == null || (int)obj < 0 || (int)obj >= Dishes.Count)
                                return false;
                            return true;
                        })
                    );
            }
        }

        private RelayCommand resetDishCmd;
        public RelayCommand ResetDishCmd
        {
            get
            {
                return resetDishCmd ??
                    (
                        resetDishCmd = new RelayCommand((obj) =>
                        {
                            while (SelectedDishes.Count != 0)
                            {
                                SelectedDishes.RemoveAt(0);
                            }
                            TotalPrice = 0;
                        },
                        (obj) => { return SelectedDishes.Count > 0; }

                    ));
            }
        }

        private RelayCommand makeOrderCommand;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public RelayCommand MakeOrderCommand
        {
            get
            {
                return makeOrderCommand ??
                    (
                        makeOrderCommand = new RelayCommand(
                            (obj) =>
                            {
                                Dish.WriteOrder(SelectedDishes);

                                MessageBox.Show("Thank`s for order !\n Your order is written in file for cooker)");

                                while (SelectedDishes.Count != 0)
                                {
                                    SelectedDishes.RemoveAt(0);
                                }
                                TotalPrice = 0;
                            },
                            (obj) => { return SelectedDishes.Count > 0; }
                    ));
            }
        }

        public ViewModel_Main()
        {
            Dishes = Dish.ReadDishes();
            SelectedDishes = new ObservableCollection<Dish>();
            TotalPrice = 0;

        }

    }
}
