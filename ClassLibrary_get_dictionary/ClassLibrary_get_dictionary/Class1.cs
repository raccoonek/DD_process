using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary_get_dictionary
{
    public class ClassLibrary
    {
        const string UnwantedSigns = ".,?!-/*”“'\"";
        private static Dictionary<string, int> get_dictionary(string text)
        {
                string FileContents = text;
                foreach (char c in UnwantedSigns)
                {
                    FileContents = FileContents.Replace(c, ' ');
                    GC.Collect();
                }
                FileContents = FileContents.Replace(Environment.NewLine, " ");
                string[] Words = FileContents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                FileContents = string.Empty;
                GC.Collect();
                Dictionary<string, int> WordCounts = new Dictionary<string, int>();
                foreach (string w in Words.Select(x => x.ToLower()))
                    if (WordCounts.TryGetValue(w, out int c))
                        WordCounts[w] = c + 1;
                    else
                        WordCounts.Add(w, 1);
                Words = new string[1];
                GC.Collect();
                return WordCounts;
        }
        public static Dictionary<string, int> get_dictionary_assync(string text)
        {
            
            string FileContents = text;
            Task[] tasks = new Task[11];
            int i = 0;
            foreach (char c in UnwantedSigns)
            {
                tasks[i] = Task.Factory.StartNew(() => {
                    FileContents = FileContents.Replace(c, ' ');
                    
                });
                i++;
                
            }
            GC.Collect();
            Task.WaitAll(tasks);
            FileContents = FileContents.Replace(Environment.NewLine, " ");
            string[] Words = FileContents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            FileContents = string.Empty;
            GC.Collect();
            Dictionary<string, int> WordCounts = new Dictionary<string, int>();

            
            foreach (string w in Words.Select(x => x.ToLower()))
            {
                
                    if (WordCounts.TryGetValue(w, out int c))
                        WordCounts[w] = c + 1;
                    else
                        WordCounts.Add(w, 1);
                
            }
                
            Words = new string[1];
            GC.Collect();
            

            return WordCounts;
        }
        
        public static Dictionary<string, int> get_dictionary_ThreadPool(string text)
        {
            string FileContents = text;
            
            foreach (char c in UnwantedSigns)
            {
                ThreadPool.QueueUserWorkItem(a => {
                    FileContents = FileContents.Replace(c, ' ');
                    
                });
                
            }
            GC.Collect();
            FileContents = FileContents.Replace(Environment.NewLine, " ");
            string[] Words = FileContents.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            FileContents = string.Empty;
            GC.Collect();
            Dictionary<string, int> WordCounts = new Dictionary<string, int>();
            foreach (string w in Words.Select(x => x.ToLower()))
                if (WordCounts.TryGetValue(w, out int c))
                  WordCounts[w] = c + 1;
                else
                  WordCounts.Add(w, 1);
            
                
            Words = new string[1];
            GC.Collect();
            return WordCounts;
        }
    }
}
