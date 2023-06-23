using _202306231339_ProgressBar.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

//【WPF】コマンドの使い方（ICommandインターフェース）
//https://marunaka-blog.com/wpf-icommand/5279/

namespace _202306231339_ProgressBar.ViewModel
{

    public class MainWindowViwModel1
    {
        private string showText = "";
        public string ShowText {
            get { return showText; }
            set
            {
                showText = value;
                ShowCommand.DelegateCanExecute();
            }
        }

        public DelegateCommand ShowCommand { get; }
        public MainWindowViwModel1()
        {
            ShowCommand = new DelegateCommand(execute, canExecute);
        }

        private void execute()
        {
            MessageBox.Show($"入力内容は\"{ShowText}\"です");
        }

        private bool canExecute()
        {
            return !string.IsNullOrEmpty(ShowText);
        }
    }

    public class MainWindowViewModel
    {
        private bool flag = true;
        public bool Flag
        {
            get { return flag; }
            set
            {
                flag = value;
                ButtonCommand.DelegateCanExecute();
            }
        }

        public DelegateCommand ButtonCommand { get; }

        public MainWindowViewModel()
        {
            //ButtonCommand = new DelegateCommand(async () =>
            //{
            //    Flag = false;
            //    await Task.Delay(5000);
            //    Flag = true;
            //}, canExecuteCommand);

            ButtonCommand = new DelegateCommand(AAA, canExecuteCommand);
        }
        private async void AAA()
        {
            Flag = false;
            await Task.Delay(5000);
            Flag = true;
        }

        private bool canExecuteCommand()
        {
            return Flag;
        }
    }
}
