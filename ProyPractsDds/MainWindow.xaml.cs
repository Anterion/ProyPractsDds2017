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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;


namespace proyPractsDds
{

    public partial class MainWindow : Window
    {
        private pS.PSClient _PSCli;
        private List<string[]> _Result;
        private BindingList<string> _Commands;
        private BindingList<string> _Strategies;
        private string arg; 
        private readonly BackgroundWorker[] Worker = //BACKGROUNDWORKER OBJECT POOL //prueba para github
        {
            new BackgroundWorker(),
            new BackgroundWorker(),
            new BackgroundWorker(),
            new BackgroundWorker()
        };

        public MainWindow()
        {
            InitializeComponent();
            InitializeBGW(); //SUSCRIBE BACKGROUNDWORKERS TO EVENTS
            _PSCli = new pS.PSClient();
            _Commands = new BindingList<string>(_PSCli.AvailableCommands);
            _Strategies = new BindingList<string>(_PSCli.AvailableStrategies);
            LBCommands.ItemsSource = _Commands;
            CBstrategy.ItemsSource = _Strategies;
            CBstrategy.SelectedItem = _Strategies.First();
        }

        private void Bexecute_Click(object sender, RoutedEventArgs e)
        {
            if (!Worker[0].IsBusy)
            {
                rb1.IsChecked = true;
                rb1.Content = "1: Working";
                Worker[0].RunWorkerAsync();

            }
            else if (!Worker[1].IsBusy)
            {
                rb2.IsChecked = true;
                rb2.Content = "2: Working";
                Worker[1].RunWorkerAsync();
            }
            else if (!Worker[2].IsBusy)
            {
                rb3.IsChecked = true;
                rb3.Content = "3: Working";
                Worker[2].RunWorkerAsync();
            }
            else if (!Worker[3].IsBusy)
            {
                rb4.IsChecked = true;
                rb4.Content = "4: Working";
                Worker[3].RunWorkerAsync();
            }
        }
        
        private void UpdateBGWState()
        {
            if (!Worker[0].IsBusy)
            {
                rb1.IsChecked = false;
                rb1.Content = "1: Asleep";
            }
            if (!Worker[1].IsBusy)
            {
                rb2.IsChecked = false;
                rb2.Content = "2: Dead";
            }
            if (!Worker[2].IsBusy)
            {
                rb3.IsChecked = false;
                rb3.Content = "3: Dead";
            }
            if (!Worker[3].IsBusy)
            {
                rb4.IsChecked = false;
                rb4.Content = "4: Dead";
            }
        }

        private void InitializeBGW() //SUSCRIBE TO EVENTS
        {
            for(int i= 0; i < Worker.Length; i++)
            {
                Worker[i].DoWork += Worker_DoWork;
                Worker[i].RunWorkerCompleted += Worker_RunWorkerCompleted;
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            _Result = _PSCli.ExecuteCommand( arg);
        }

        private void PrintResult()
        {
            foreach (string[] stringarray in _Result)
            {
                TBResult.AppendText(String.Join(", ", stringarray) + "\n");
            }
        }
     
        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateBGWState();
            PrintResult();
        }

        private void Bclear_Click(object sender, RoutedEventArgs e)
        {
            TBResult.Clear();
        }

        private void LBCommands_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TBargument.Clear();
            _PSCli.Command = LBCommands.SelectedItem.ToString();
            string arg = _PSCli.ArgumentType;
            if (arg == "None")
            {
                LabelArgument.Content = " ";
                TBargument.IsEnabled = false;
            }
            else
            {
                LabelArgument.Content = arg;
                TBargument.IsEnabled = true;
            }
        }

        private void TBargument_TextChanged(object sender, TextChangedEventArgs e)
        {
            arg = TBargument.Text;
        }

        private void CBstrategy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _PSCli.SetInvokerStrategy(CBstrategy.SelectedItem.ToString());
        }
    }
}
