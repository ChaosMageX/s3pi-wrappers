﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using s3piwrappers.Granny2;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    /// Interaction logic for BoneSelectDialog.xaml
    /// </summary>
    public partial class BoneSelectDialog : Window
    {
        public BoneSelectDialog(IEnumerable<Bone> bones, String title):this()
        {
            Title = title;
            DataContext = bones;
            mBoneComboBox.SelectedIndex = 0;
        }
        public BoneSelectDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
        public Bone SelectedBone { get { return mBoneComboBox.SelectedItem as Bone; } }
    }
}
