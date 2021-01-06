using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace blockingcollection_overview
{
    class Program
    {
        static void Main(string[] args)
        {
            // A bounded collection. It can hold no more
            // than 100 times at once.
            BlockingCollection<Image> dataItems = new BlockingCollection<Image>(100);

            // A Simple blocking consumer with no cancellation.
            Task.Run(() =>
            {
                while (!dataItems.IsCompleted)
                {
                    Data data = null;
                    try
                    {
                        data = dataItems.Take();
                    }
                    catch (IndexOutOfRangeException) { }
                    if (data != null)
                    {
                        Process(data);
                    }
                }
                Console.WriteLine("\r\nNoe more items to take.");
            });

            // A Simple blocking producer with no cancellation.
            Task.Run(() =>
            {
                while (moreItemsToAdd)
                {
                    Data data = GetData();
                    // Blocks if number.Count == dataItems.BoundedCapacity
                    dataItems.Add(data);
                }
                // Let consumer know we are done.
                dataItems.CompleteAdding();
            });

        }
    }
}
