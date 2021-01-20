using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using Image = System.Windows.Controls.Image;
using System.Drawing.Imaging;

namespace CarsOwnersEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>    
    public partial class MainWindow : Window   
        
    {
        public List<Owner> owners = new List<Owner>();
        public List<Car> cars = new List<Car>();
        CarsOwnersDatabaseContext ctx = new CarsOwnersDatabaseContext();
        byte[] ImageData;
        private static readonly ImageConverter _imageConverter = new ImageConverter();
        public MainWindow()
        {
            InitializeComponent();
            LoadDate();

        }
        public void LoadDate()
        {
            try
            {
                owners = ctx.Owners.Include("Car").ToList();
            }
            catch
            {
                owners = ctx.Owners.ToList();
            }
            
            cars = ctx.Cars.ToList<Car>();
            lvOwners.ItemsSource = owners;
        }
        public void AddOwner()
        {
            if ((txtName.Text.Trim() == string.Empty))
            {
                MessageBox.Show("Name can not be empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Owner owner = new Owner
            {
                Name = txtName.Text,
                Image = ImageData    
                
            };
            owners.Add(owner);
            ctx.Owners.Add(owner);
            ctx.SaveChanges();
            ResetValue();
        }
        public void DeleteOwner()
        {
            if (lvOwners.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Owner ownerToBeDeleted = (Owner)lvOwners.SelectedItem;
            
            if (ownerToBeDeleted != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "CONFIRMATION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    owners.Remove(ownerToBeDeleted);                    
                    ResetValue();

                    //Remove from database
                    ctx.Owners.Remove(ownerToBeDeleted);                    
                    ctx.SaveChanges();
                }
            }
        }
        public void UpdateOwner()
        {
            if (lvOwners.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((txtName.Text == ""))
            {
                MessageBox.Show("Input string error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Owner ownerToBeUpdated = (Owner)lvOwners.SelectedItem;
            ownerToBeUpdated.Name = txtName.Text;            
            ResetValue();

            //Update in datebase
            int id = ownerToBeUpdated.OwnerId;
            Owner owner=ctx.Owners.Where(o => o.OwnerId==id).FirstOrDefault<Owner>();
            if (owner != null)
            {
                owner.Name += txtName.Text;
                owner.Image = ImageData;
                ctx.SaveChanges();
            }

        }
        public void AddImage()
        {
            FileStream fs;
            BinaryReader br;


            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            if (openFileDialog.ShowDialog() == true)
            {
                Image img = new Image();
                img.Source= new BitmapImage(new Uri(openFileDialog.FileName));
                btnImage.Content = "";
                
                // Set button background
                ImageBrush brush= new ImageBrush();
                BitmapImage bi= new BitmapImage(new Uri(openFileDialog.FileName));
                brush.ImageSource = bi;
                btnImage.Background = brush;

                //Image to byte[] to save it in database
                string FileName = openFileDialog.FileName;                
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                br = new BinaryReader(fs);
                ImageData = br.ReadBytes((int)fs.Length);
                br.Close();
                fs.Close();
            }            
        }           

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOwner();
            ResetValue();

        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            AddImage();
        }
        private void ResetValue()
        {
            lvOwners.Items.Refresh();
            txtName.Clear();
            btnImage.Content = "Click to add image";
            btnImage.Background = null;
            //txtTask.Clear();
            //dpDueDate.Text = "";
            //slDifficulty.Value = 1;
            //comStatus.Text = "";
            //lvList.SelectedIndex = -1;
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteOwner();
        }

        private void lvOwners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdate.IsEnabled = true;
            btnDelete.IsEnabled = true;
            var selectedOwner = lvOwners.SelectedItem;
            if(selectedOwner is Owner)
            {
                Owner owner = (Owner)lvOwners.SelectedItem;
                txtName.Text = owner.Name;

                ImageBrush brush;
                BitmapImage bi;
                using (var ms = new MemoryStream(owner.Image))
                {
                    brush = new ImageBrush();

                    bi = new BitmapImage();
                    bi.BeginInit();
                    bi.CreateOptions = BitmapCreateOptions.None;
                    bi.CacheOption = BitmapCacheOption.OnLoad;
                    bi.StreamSource = ms;
                    bi.EndInit();
                }

                brush.ImageSource = bi;
                btnImage.Background = brush;
                btnImage.Content = string.Empty;
                ImageData = owner.Image;
                //btnImage.Content = new Image
                //{
                //    Source = new BitmapImage(GetImageFromByteArray(owner.Image), UriKind.Relative)),
                //    VerticalAlignment = VerticalAlignment.Center,
                //    Stretch = Stretch.Fill,
                //    Height = 256,
                //    Width = 256
                //};


                //Bitmap newBitmap= GetImageFromByteArray(owner.Image);

                //MemoryStream ms = new MemoryStream();
                //newBitmap.Save(ms, ImageFormat.Png);
                //ms.Position = 0;
                //BitmapImage bi = new BitmapImage();
                //bi.BeginInit();
                //bi.StreamSource = ms;
                //bi.EndInit();
                ////CurrentImage = bi;
                //btnImage.Content = bi;



                //btnImage.Content =(Image) GetImageFromByteArray(owner.Image);
            }
        }

        private void lvOwners_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvOwners.UnselectAll();
            ResetValue();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateOwner();
        }

        private void btnManageCars_Click(object sender, RoutedEventArgs e)
        {
            if (lvOwners.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select an owner", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Owner ownerToBeUpdated = (Owner)lvOwners.SelectedItem;
            //ownerToBeUpdated.Name = txtName.Text;           
                     
            int id = ownerToBeUpdated.OwnerId;

            CarsDialog carsDialon = new CarsDialog(id);
            carsDialon.Owner = this;
            carsDialon.ShowDialog();

        }

        //public static Bitmap GetImageFromByteArray(byte[] byteArray)
        //{
        //    Bitmap bm = (Bitmap)_imageConverter.ConvertFrom(byteArray);

        //    if (bm != null && (bm.HorizontalResolution != (int)bm.HorizontalResolution ||
        //                       bm.VerticalResolution != (int)bm.VerticalResolution))
        //    {
        //        // Correct a strange glitch that has been observed in the test program when converting 
        //        //  from a PNG file image created by CopyImageToByteArray() - the dpi value "drifts" 
        //        //  slightly away from the nominal integer value
        //        bm.SetResolution((int)(bm.HorizontalResolution + 0.5f),
        //                         (int)(bm.VerticalResolution + 0.5f));
        //    }

        //    return bm;
        //}
    }
}
