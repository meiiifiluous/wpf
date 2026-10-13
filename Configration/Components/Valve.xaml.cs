using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Configration.Components
{
    /// <summary>
    /// Valve.xaml 的交互逻辑
    /// </summary>
    public partial class Valve : UserControl
    {
        //依赖属性
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen",
                typeof(bool), typeof(Valve),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenChanged)));

        private static void OnIsOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var valve = (d as Valve);
            valve.rt.Angle = valve.IsOpen ? 0 : 90;
        }
        public Valve()
        {
            InitializeComponent();
        }
    }
}
