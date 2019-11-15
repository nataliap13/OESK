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
        private MySQLiteDbContext conn = new MySQLiteDbContext();
        public MainWindow()
        {
            InitializeComponent();
            /*
            try
            {
                //using (var conn = new SQLiteConnection("Data Source=db.db; Version=3; New=False; Compress=True;"))
                //var conn = new MySQLiteDbContext();
                var a = conn.TableAlgorithm.ToList();
                MessageBox.Show(a.Count().ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
                MessageBox.Show("Error: " + e.InnerException);
            }*/
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
            var begin = DateTime.Now;
            TimeSpan timeOfCalculation;
            var text = UserTxtBox.Text;
            string hash = GetMd5Hash(text, out timeOfCalculation);
            MD5TxtBlockHash.Text = hash;
            MD5TxtBlockTime.Text = ((int)timeOfCalculation.TotalSeconds).ToString() + ","
                + String.Format("{0:fffffff}", timeOfCalculation); ;
            //MD5TxtBlockTime.Text = String.Format("{0:mm\\:ss\\:fffffff}", timeOfCalculation);
            SaveTestToDatabase(text, timeOfCalculation, "MD5");

            hash = GetSHA1Hash(text, out timeOfCalculation);
            SHA1TxtBlockHash.Text = hash;
            SHA1TxtBlockTime.Text = ((int)timeOfCalculation.TotalSeconds).ToString() + ","
                + String.Format("{0:fffffff}", timeOfCalculation);
            //SHA1TxtBlockTime.Text = String.Format("{0:mm\\:ss\\:fffffff}", timeOfCalculation);
            SaveTestToDatabase(text, timeOfCalculation, "SHA1");

            hash = GetSHA256Hash(text, out timeOfCalculation);
            SHA256TxtBlockHash.Text = hash;
            SHA256TxtBlockTime.Text = ((int)timeOfCalculation.TotalSeconds).ToString() + ","
                + String.Format("{0:fffffff}", timeOfCalculation);
            //SHA256TxtBlockTime.Text = String.Format("{0:mm\\:ss\\:fffffff}", timeOfCalculation);
            SaveTestToDatabase(text, timeOfCalculation, "SHA256");
            TxtBlockFullTime.Text = (DateTime.Now - begin).ToString();
        }

        private void SaveTestToDatabase(string sourceText, TimeSpan timeOfCalculation, string AlgorithmName)
        {
            var listOfTexts = conn.TableText.Where(x => x.Text == sourceText).ToList();
            var IDText = 0;
            if (listOfTexts.Count() == 0)
            {
                var entryText = new TableText();
                entryText.Text = sourceText;
                entryText = conn.TableText.Add(entryText);
                IDText = entryText.IDText;
            }
            else { IDText = listOfTexts.First().IDText; }

            var entryTestResult = new TableTestResult();
            entryTestResult.IDAlgorithm = conn.TableAlgorithm.Where(x => x.Name == AlgorithmName).First().IDAlgorithm;
            entryTestResult.IDText = IDText;
            entryTestResult.CalculationTime = ((int)timeOfCalculation.TotalSeconds).ToString() + ","
                + String.Format("{0:fffffff}", timeOfCalculation);
            entryTestResult = conn.TableTestResult.Add(entryTestResult);
            try
            { conn.SaveChanges(); }
            catch (Exception e)
            { MessageBox.Show(e.Message); }
        }

        private void BtnStartStandardTest_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan timeOfCalculation;
            var text = "A";
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    for (int j = 0; j < 10; j++)
                    {
                        GetMd5Hash(text, out timeOfCalculation);
                        SaveTestToDatabase(text, timeOfCalculation, "MD5");

                        GetSHA1Hash(text, out timeOfCalculation);
                        SaveTestToDatabase(text, timeOfCalculation, "SHA1");

                        GetSHA256Hash(text, out timeOfCalculation);
                        SaveTestToDatabase(text, timeOfCalculation, "SHA256");
                    }
                    text = new String('A', 1000 * (i + 1));
                }
                MessageBox.Show("Test zakończony poprawnie.");
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}
