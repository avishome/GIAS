﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ui
{
    /// <summary>
    /// Interaction logic for listOfReports.xaml
    /// </summary>
    public partial class listOfReports : UserControl
    {
        public listOfReports()
        {
            InitializeComponent();
            for (int i = 0; i < item.Items.Count; i++) {

                ContentPresenter c = (ContentPresenter)item.ItemContainerGenerator.ContainerFromItem(item.Items[i]);
                ToggleButton tb = c.ContentTemplate.FindName("mybtn", c) as ToggleButton;
                if (!tb.IsChecked.Value) { MessageBox.Show("f"); }
            }
        }
    }
}
