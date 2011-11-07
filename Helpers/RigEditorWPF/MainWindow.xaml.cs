﻿using System.Diagnostics;
using System.Windows;
using s3piwrappers.RigEditor.Diagnostics;
using s3piwrappers.RigEditor.ViewModels;

namespace s3piwrappers.RigEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BindingListener mListener;
        public MainWindow(RigEditorViewModel editorViewModel):this()
        {
            DataContext = editorViewModel;
        }
        public MainWindow()
        {
            InitializeComponent();
            //this.mListener = new BindingListener(TraceOptions.Callstack | TraceOptions.DateTime |
            //    TraceOptions.LogicalOperationStack | TraceOptions.ProcessId | TraceOptions.ThreadId |
            //    TraceOptions.Timestamp);
        }

        private void BoneTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var bone = mBoneTreeView.SelectedItem as BoneViewModel;
            mBonePanel.Visibility = bone != null? System.Windows.Visibility.Visible:System.Windows.Visibility.Hidden;
        }
    }
}
