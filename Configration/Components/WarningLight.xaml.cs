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
    /// WarningLight.xaml 的交互逻辑
    /// </summary>
    public partial class WarningLight : UserControl
    {
        public LightState State
        {
            get { return (LightState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register("State",
                typeof(LightState),
                typeof(WarningLight),
                new PropertyMetadata(LightState.None, new PropertyChangedCallback(OnStateChanged)));

        private static void OnStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var light = (WarningLight)d;

            if (light.State == LightState.None)
            {
                light.borFault.Visibility = Visibility.Hidden;
                light.borWarning.Visibility = Visibility.Hidden;
                light.borRun.Visibility = Visibility.Hidden;
            }
            else if (light.State == LightState.Fault)
            {
                light.borFault.Visibility = Visibility.Visible;
                light.borWarning.Visibility = Visibility.Hidden;
                light.borRun.Visibility = Visibility.Hidden;
            }
            else if (light.State == LightState.Warning)
            {
                light.borFault.Visibility = Visibility.Hidden;
                light.borWarning.Visibility = Visibility.Visible;
                light.borRun.Visibility = Visibility.Hidden;
            }
            else if (light.State == LightState.Run)
            {
                light.borFault.Visibility = Visibility.Hidden;
                light.borWarning.Visibility = Visibility.Hidden;
                light.borRun.Visibility = Visibility.Visible;
            }
        }
        public WarningLight()
        {
            InitializeComponent();
        }
    }
    public enum LightState
    {
        None, Fault, Warning, Run
    }
}
