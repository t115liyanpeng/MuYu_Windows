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


namespace MuYu.Conf
{
    /// <summary>
    /// SettingWin.xaml 的交互逻辑
    /// </summary>
    public partial class SettingWin : Window
    {
        public string DisPlay { get; set; }

        public Color DisColor { get; set; }

        private Dictionary<string,Color> _colors = new Dictionary<string, Color>()
        {
            {"红",Colors.Red},
            {"黄",Colors.Yellow},
            {"白",Colors.White},
            {"黑",Colors.Black},
        };
        public SettingWin()
        {
            InitializeComponent();
        }

        private void Setting_Click(object sender, RoutedEventArgs e)
        {
            if (txt.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请填写需要显示的文字");
                return;
            }
            if (color.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请选择需要显示的文字的颜色");
                return;
            }

            DisColor = _colors[color.Text];
            DisPlay=txt.Text;
            this.DialogResult = true;
        }
    }
}
