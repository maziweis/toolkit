using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResourceToolkit
{
    public partial class SplashScreen : Form
    {
        /// <summary>
        /// 启动画面本身
        /// </summary>
        static SplashScreen instance;

        /// <summary>
        /// 显示图片
        /// </summary>
        Bitmap bitmap;
        public static SplashScreen Instance
        {
            get { return instance; }
            set { instance = value; }
        }

        public SplashScreen()
        {
            InitializeComponent();

            //const string showInfo = "正在加载程序，请稍后...";
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.CenterScreen;
            ShowInTaskbar = false;
            //bitmap = new Bitmap(@"package/startImg.png",false);
            //ClientSize = bitmap.Size;
            /*
            using (Font font = new Font("Consoles", 10))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawString(showInfo, font, Brushes.White, 200, 330);
                }
            }
            */
            //BackgroundImage = bitmap; 
        }

        public static void ShowSplashScreen()
        {
            instance = new SplashScreen();
            instance.Show();
        }  
    }

}
