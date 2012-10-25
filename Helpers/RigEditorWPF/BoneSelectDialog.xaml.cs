using System;
using System.Collections.Generic;
using System.Windows;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    ///   Interaction logic for BoneSelectDialog.xaml
    /// </summary>
    public partial class BoneSelectDialog : Window
    {
        public BoneSelectDialog(IEnumerable<RigResource.RigResource.Bone> bones, String title) : this()
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
            Close();
        }

        public RigResource.RigResource.Bone SelectedBone
        {
            get { return mBoneComboBox.SelectedItem as RigResource.RigResource.Bone; }
        }
    }
}
