using _202306231339_ProgressBar.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

//【WPF】コマンドの使い方（ICommandインターフェース）
//https://marunaka-blog.com/wpf-icommand/5279/

namespace _202306231339_ProgressBar.ViewModel
{

    public class MainWindowViewModel3 : ViewModelBase
    {
        private CancellationTokenSource CancellationTokenSource;
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                    ButtonCommand.DelegateCanExecute();
                    CancelCommand.DelegateCanExecute();
                }
            }
        }

        private int prgValue;

        public int PrgValue
        {
            get { return prgValue; }
            set { prgValue = value; OnPropertyChanged(nameof(PrgValue)); }
        }

        private string message = "";

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged(nameof(Message)); }
        }

        public DelegateCommand ButtonCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }

        public MainWindowViewModel3()
        {
            CancellationTokenSource = new();
            ButtonCommand = new(async () => await ExecuteAsync(), CanExecute);
            CancelCommand = new(() => CancellationTokenSource.Cancel(), CanCancel);
        }

        private async Task ExecuteAsync()
        {
            IsBusy = true;
            await WorkTask(CancellationTokenSource.Token);
            CancellationTokenSource = new();
            IsBusy = false;
        }

      
        private async Task WorkTask(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                Message = "処理中";
                for (PrgValue = 0; PrgValue < 100; PrgValue++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Message = "処理中止";
                        return;
                    }
                    //Thread.Sleep(100);
                    await Task.Delay(10);
                }
                Message = "処理完了";
            }, token);
        }

        private bool CanExecute() => !IsBusy;
        private bool CanCancel() => IsBusy;

    }

    /// <summary>
    /// 足し算引き算、ラジオボタン
    /// </summary>
    public class MainWindowViewModel2 : ViewModelBase
    {

        private double firstValue;
        public double FirstValue
        {
            get => firstValue; set
            {
                firstValue = value;
                PerformOperation(null);
            }
        }

        private double secondValue;
        public double SecondValue { get => secondValue; set { secondValue = value; PerformOperation(null); } }

        private double resultValue;
        public double ResultValue { get => resultValue; set { resultValue = value; OnPropertyChanged(nameof(ResultValue)); } }
        //public double ResultValue { get => resultValue; set { resultValue = value; SetProperty(ref resultValue, value); } }

        private string operation = "+";
        public string Operation { get => operation; set { operation = value; OnPropertyChanged(nameof(Operation)); } }

        private string operationGiven = "Add";


        public DelegateCommand<string> OperationCommand { get; }

        public MainWindowViewModel2()
        {
            OperationCommand = new DelegateCommand<string>(x => PerformOperation(x));
        }

        private void PerformOperation(string? param)
        {
            if (param != null) { operationGiven = param; }
            switch (operationGiven)
            {
                case "Add": Add(); break;
                case "Minus": Substract(); break;
            }
        }

        private void Add()
        {
            Operation = "+";
            ResultValue = firstValue + secondValue;
        }

        private void Substract()
        {
            Operation = "-";
            ResultValue = firstValue - secondValue;
        }

    }


    public class MainWindowViewModel1
    {
        private string showText = "";
        public string ShowText
        {
            get { return showText; }
            set
            {
                showText = value;
                ShowCommand.DelegateCanExecute();
            }
        }

        public DelegateCommand ShowCommand { get; }
        public MainWindowViewModel1()
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
