using System;
using System.Collections.Generic;
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

namespace CarsOwnersEF
{
    /// <summary>
    /// Interaction logic for CarsDialog.xaml
    /// </summary>
    public partial class CarsDialog : Window
    {
        public List<Owner> owners = new List<Owner>();
        public List<Car> cars = new List<Car>();
        
        CarsOwnersDatabaseContext ctx = new CarsOwnersDatabaseContext();
        public int OwnerId;
        public CarsDialog(int id)
        {
            InitializeComponent();
            OwnerId = id;
            LoadData();
        }
        public void LoadData()
        {
            //Car car = ctx.Cars.Where(c => c.OwnerId == OwnerId).FirstOrDefault<Car>();
            cars = ctx.Cars.Where(c => c.OwnerId == OwnerId).ToList<Car>();
           
            lvCars.ItemsSource = cars;
            Owner owner = ctx.Owners.Where(w => w.OwnerId == OwnerId).FirstOrDefault<Owner>();
            lblId.Content = owner.OwnerId;
            lblName.Content = owner.Name;
        }

        private void lvCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = true;
            btnUpdate.IsEnabled = true;

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtMakeModel.Text == string.Empty)
            {
                MessageBox.Show("Make and Model can not be empty", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Car car = new Car
            {
                MakeModel = txtMakeModel.Text,
                OwnerId = OwnerId
            };
            cars.Add(car);
            ctx.Cars.Add(car);
            ctx.SaveChanges();
            lvCars.Items.Refresh();
            txtMakeModel.Clear();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvCars.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Car carToBeDeleted = (Car)lvCars.SelectedItem;
            if (carToBeDeleted != null)
            {
                cars.Remove(carToBeDeleted);
                ctx.Cars.Remove(carToBeDeleted);
                ctx.SaveChanges();
                lvCars.Items.Refresh();
            }
            
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void lvCars_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvCars.UnselectAll();
            lvCars.SelectedIndex = -1;
            btnUpdate.IsEnabled = false;
            btnDelete.IsEnabled = false;
            
        }
    }
}
