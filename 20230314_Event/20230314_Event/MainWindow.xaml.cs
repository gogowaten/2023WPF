using System.Windows;


//ボタンクリックで値変更、値変更時にイベント発生
namespace _20230314_Event
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Sample2 MySample2;        
        public MainWindow()
        {
            InitializeComponent();
            MySample2= new Sample2();
            MySample2.TestEvent += MySample2_TestEvent;
        }

        private void MySample2_TestEvent(object arg1, int arg2)
        {
            var neko = arg2;   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MySample2.X = 20;
        }

    }
}
