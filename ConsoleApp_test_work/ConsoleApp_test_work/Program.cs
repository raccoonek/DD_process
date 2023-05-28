﻿using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;
using System;
using Microsoft.Win32;
using ClassLibrary_get_dictionary;
using System.Diagnostics;

namespace ConsoleApp_test_work
{
    internal class Program
    {
        static void Main()
        {
            ReflectionExample();
        }
        static void ReflectionExample()
        {
            try
            {
                Console.WriteLine("Напишите полный путь к текстовому файлу:");
                string check_path = Console.ReadLine();

                string path = check_path;
                string text = File.ReadAllText(path);

                var myClass = new ClassLibrary();
                var type = myClass.GetType();
                var methodinfo = type.GetMethod("get_dictionary",BindingFlags.Static | BindingFlags.NonPublic);

                Stopwatch stopWatch = new Stopwatch();
                

                stopWatch.Start();
                var WordCounts = methodinfo.Invoke(myClass, new object[] { text } ) as Dictionary<string, int>;
                stopWatch.Stop();

                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds,ts.Milliseconds);
                Console.WriteLine("Without async " + elapsedTime+" ms");

                List<Tuple<int, string>> WordStats = WordCounts.Select(x => new Tuple<int, string>(x.Value, x.Key)).ToList();
                WordStats.Sort((x, y) => y.Item1.CompareTo(x.Item1));
                foreach (Tuple<int, string> t in WordStats)
                    File.AppendAllText(path + ".Without async.txt", t.Item2 + " " + t.Item1 + Environment.NewLine);

                stopWatch.Restart();
                WordCounts = ClassLibrary.get_dictionary_assync(text);
                stopWatch.Stop();

                 ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds);
                Console.WriteLine("async Tasks " + elapsedTime + " ms");

                 
                stopWatch.Restart();
                WordCounts = ClassLibrary.get_dictionary_ThreadPool(text);
                stopWatch.Stop();

                ts = stopWatch.Elapsed;
                elapsedTime = String.Format("{0:00}.{1:00}", ts.Seconds, ts.Milliseconds);
                Console.WriteLine("async ThreadPool " + elapsedTime + " ms");

                 
                Console.WriteLine("Обработка прошла успешно");
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Ошибка подключения введите путь заново.");
                Main();
            }
        }

       
    }
}