using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
//using System.Windows.Shapes;
//using System.Management;


namespace OESK
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MD5 md5Hash = MD5.Create();
        private SHA1 sha1Hash = SHA1.Create();
        private SHA256 sha256Hash = SHA256.Create();
        private MySQLiteDbContext dbConnection = new MySQLiteDbContext();
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                //using (var conn = new SQLiteConnection("Data Source=db.db; Version=3; New=False; Compress=True;"))
                var conn = new MySQLiteDbContext();
                var a = conn.TableAlgorithm.ToList();
                MessageBox.Show(a.Count().ToString());
                /*
                var A = new TableAlgorithm();
                A.Name = "SHA512";
                conn.TableAlgorithm.Add(A);
                conn.SaveChanges();*/
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                MessageBox.Show("Error: " + e.InnerException);
            }
        }



        private string buildHashString(byte[] data)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            { sBuilder.Append(data[i].ToString("x2")); }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        private string GetMd5Hash(string input, out TimeSpan timeSpan)
        {
            #region set priority
            //use the first Core/Processor for the test
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);

            //prevent "Normal" Processes from interrupting Threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            #endregion

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            timeSpan = stopWatch.Elapsed;
            return buildHashString(data);
        }
        private string GetSHA1Hash(string input, out TimeSpan timeSpan)
        {
            #region set priority
            //use the first Core/Processor for the test
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);

            //prevent "Normal" Processes from interrupting Threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            #endregion

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha1Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            timeSpan = stopWatch.Elapsed;
            return buildHashString(data);
        }
        private string GetSHA256Hash(string input, out TimeSpan timeSpan)
        {
            #region set priority
            //use the first Core/Processor for the test
            Process.GetCurrentProcess().ProcessorAffinity = new IntPtr(1);

            //prevent "Normal" Processes from interrupting Threads
            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            //prevent "Normal" Threads from interrupting this thread
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
            #endregion

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            timeSpan = stopWatch.Elapsed;
            return buildHashString(data);
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan time;
            string hash = GetMd5Hash(UserTxtBox.Text, out time);
            MD5TxtBlockHash.Text = hash;
            MD5TxtBlockTime.Text = String.Format("{0:mm\\:ss\\:fffffff}", time);

            hash = GetSHA1Hash(UserTxtBox.Text, out time);
            SHA1TxtBlockHash.Text = hash;
            SHA1TxtBlockTime.Text = String.Format("{0:mm\\:ss\\:fffffff}", time);

            hash = GetSHA256Hash(UserTxtBox.Text, out time);
            SHA256TxtBlockHash.Text = hash;
            SHA256TxtBlockTime.Text = String.Format("{0:mm\\:ss\\:fffffff}", time);
        }
    }
}
