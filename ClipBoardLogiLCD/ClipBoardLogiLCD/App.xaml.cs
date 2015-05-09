using LogiLcdClipBoardService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ClipBoardLogiLCD
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static ClipBoardViewer cbv;

        App()
        {
            InitializeComponent();        
        }

        [STAThread]
        static void Main()
        {
            App app = new App();            
            Window window = new Window();
            window.Width = 0;
            window.Height = 0;
            window.WindowStyle = WindowStyle.None;
            window.ShowInTaskbar = false;
            window.ShowActivated = false;
            
            cbv = new ClipBoardViewer(window);

           
            window.Loaded += Window_Loaded;
            window.Closing += Window_Closing;
            app.Run(window);
        }

        private static void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cbv.OnStop();
        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cbv.OnStart();
        }                     
    }


}
