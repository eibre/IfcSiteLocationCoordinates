using Microsoft.Win32;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IfcSiteLocationCoordinates
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void browseBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                textBox.Text = "Opening " + openFileDialog.FileName + "..."; 
                
                IfcLocationData data = new IfcLocationData(openFileDialog.FileName);
                StringBuilder sb = new StringBuilder();
                if (data.SchemaIsSupported)
                {
 
                    sb.Append("File Path: " + openFileDialog.FileName);
                    sb.Append("\nSchema: " + data.Schema);
                    sb.Append("\nAuthoring program: " + data.AuthoringTool);
                    sb.Append("\nLength unit: " + data.LengthUnit);
                    sb.Append("\n\nInfo from IfcSites IfcLocalPlacement:");
                    sb.Append($"\nN/S: {data.NS}\nE/W: {data.EW}");
                    sb.Append("\nElevation: " + data.elevation.ToString());
                    sb.Append("\nAngle to True North: " + data.Orientation.ToString());

                    sb.Append("\n\nIfcSite.RefElevation is: " + data.refElevation.ToString());
                    sb.Append("\nRevit will add this value to the shared elevation on link/import, but most other software ignores this.");
                }
                else
                {
                    sb.Append("File Path: " + openFileDialog.FileName);
                    sb.Append("\nSchema: " + data.Schema);
                    sb.Append("\nThis Schema is not supported yet");
                }


                this.textBox.Text = sb.ToString();
            }
        }
    }
}
