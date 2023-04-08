using System.Windows;
using System.Windows.Controls;

//方法: ContextMenuOpening イベントを処理する - WPF .NET Framework | Microsoft Learn
//https://learn.microsoft.com/ja-jp/dotnet/desktop/wpf/advanced/how-to-handle-the-contextmenuopening-event


//右クリックメニュー(ContextMenu)の表示、非表示テスト
namespace _20230408_ContextMemu
{
    public partial class MainWindow : Window
    {
        private ContextMenu MyContextMenu1;
        private ContextMenu MyContextMenu2;
        public MainWindow()
        {
            InitializeComponent();

            Top = 300;
            Left = 300;
            MyContextMenu1 = new();            
            MyContextMenu1.Items.Add(new MenuItem() { Header = "ランボー怒りの" });
            MyContextMenu1.Items.Add(new MenuItem() { Header = "右クリック" });

            MyContextMenu2 = new();
            MyContextMenu2.Items.Add(new MenuItem() { Header = "Rect2専用" });
            MyContextMenu2.Items.Add(new MenuItem() { Header = "暮らし安心" });
            MyContextMenu2.Items.Add(new MenuItem() { Header = "ガウシアン" });

            //右クリックメニュー開くときの動作
            //これじゃない
            //ContextMenuOpening += MainWindow_ContextMenuOpening;

            //結果1
            ContextMenuOpening += MainWindow_ContextMenuOpening1;

            //結果2
            //ContextMenuOpening += MainWindow_ContextMenuOpening2;
        }

        //MyRectの1か2以外を右クリックした直後だと
        //MyRectの1か2を右クリックしてもメニューが表示されない
        //もう一度右クリックで表示される
        private void MainWindow_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (e.Source == MyRect1 || e.Source == MyRect2)
            {
                ContextMenu = MyContextMenu1;
            }
            else
            {
                ContextMenu = null;
            }
        }

        //これなら一回で表示される
        private void MainWindow_ContextMenuOpening1(object sender, ContextMenuEventArgs e)
        {
            if (e.Source == MyRect1 || e.Source == MyRect2)
            {
                //nullだった場合は表示したいメニューを指定後に強制表示
                if (ContextMenu == null)
                {
                    ContextMenu = MyContextMenu1;
                    ContextMenu.IsOpen = true;
                }
            }
            else ContextMenu = null;
        }

        //MainWindow_ContextMenuOpening1を改変
        //MyRect1か2なら、それぞれの右クリックメニューを表示する、それ以外は表示しない
        private void MainWindow_ContextMenuOpening2(object sender, ContextMenuEventArgs e)
        {
            bool flag = false;
            if (ContextMenu == null) { flag = true; }
            if (e.Source == MyRect1)
            {
                ContextMenu = MyContextMenu1;
                if (flag) { ContextMenu.IsOpen = true; }
            }
            else if (e.Source == MyRect2)
            {
                ContextMenu = MyContextMenu2;
                if (flag) { ContextMenu.IsOpen = true; }
            }
            else
            {
                ContextMenu = null;
            }
        }


    }
}
