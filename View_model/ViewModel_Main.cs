using System.Collections.ObjectModel;
using System.Windows;


namespace Task4
{
    public class ViewModel_Main : DependencyObject
    {
        public ObservableCollection<Dish> SelectedDishes { get; set; }

        public ObservableCollection<Dish> Dishes
        {
            get { return ((ObservableCollection<Dish>)GetValue(DishesCollectionProperty)); }
            set { SetValue(DishesCollectionProperty, value); }
        }
        public static readonly DependencyProperty DishesCollectionProperty =
            DependencyProperty.Register("Dishes", typeof(ObservableCollection<Dish>), typeof(ViewModel_Main));


        public float TotalPrice
        {
            get { return (float)GetValue(TotalPriceProperty); }
            set { SetValue(TotalPriceProperty, value); }
        }
        public static readonly DependencyProperty TotalPriceProperty =
            DependencyProperty.Register("TotalPrice", typeof(float), typeof(ViewModel_Main));


        private RelayCommand selectDishCmd;
        public RelayCommand SelectDishCmd
        {
            get
            {
                return selectDishCmd ??
                    (
                        selectDishCmd = new RelayCommand(
                        (obj) =>{
                            Dish dish = new Dish((Dishes[(int)obj].Name), (Dishes[(int)obj].Price), (Dishes[(int)obj].Detail));

                            TotalPrice += dish.Price;

                            SelectedDishes.Add(dish);
                        },
                    
                        (obj)=> 
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
                            while(SelectedDishes.Count != 0)
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
