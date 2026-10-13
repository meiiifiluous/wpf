using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Configration.Components
{
    /// <summary>
    /// Tank.xaml 的交互逻辑
    /// </summary>
    public partial class Tank : UserControl
    {
        public double Level
        {
            get { return (double)GetValue(LevelProperty); }
            set { SetValue(LevelProperty, value); }
        }
        //依赖属性
        public static readonly DependencyProperty LevelProperty =
                DependencyProperty.Register("Level",
                typeof(double),
                typeof(Tank),
        new PropertyMetadata(0.0, new PropertyChangedCallback(OnLevelChanged)));
        //属性变化回调
        private static void OnLevelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tank = d as Tank;
            var v = 120 - tank.Level / 100 * 120;// 算出液体该下移多少
            // 300ms 过渡到 v
            DoubleAnimation da = new DoubleAnimation(v, new TimeSpan(0, 0, 0, 0, 300));
            tank.tt.BeginAnimation(TranslateTransform.YProperty, da);
        }
        public Tank()
        {
            InitializeComponent();
        }
    }
}
