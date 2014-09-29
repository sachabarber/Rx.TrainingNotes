using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Asynchrony
{
    [TestFixture]
    public class LegacyConstructsEPM
    {

        [Test]
        public void Read_file_with_BackgroundWorker_EPM()
        {
            var filePath = @"LargeTextFile.txt";

            Console.WriteLine("Initial thread : {0}", Thread.CurrentThread.ManagedThreadId);

            var bgWorker = new BackgroundWorker();
            bgWorker.DoWork += Work;
            bgWorker.RunWorkerCompleted += Completed;
            bgWorker.RunWorkerAsync(filePath);


            //Spin-block to stop test runner exiting.
            var waitStart = DateTime.Now;
            while (DateTime.Now - waitStart < TimeSpan.FromSeconds(2) && bgWorker.IsBusy)
            {}

            bgWorker.DoWork -= Work;
            bgWorker.RunWorkerCompleted -= Completed;
            bgWorker.Dispose();
        }

        private void Work(object sender, DoWorkEventArgs e)
        {
            var caller = (BackgroundWorker)sender;
            var filePath = (string)e.Argument;

            Console.WriteLine("Worker thread : {0}", Thread.CurrentThread.ManagedThreadId);
            e.Result = File.ReadAllBytes(filePath).Length;	//Yeah silly. Could just use FileInfo. -LC
        }

        private void Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("Completion thread : {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("File length : {0}", e.Result);
        }
    }
}
