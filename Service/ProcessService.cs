using PcHealthHub.Model;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace PcHealthHub.Service
{
    public static class ProcessService
    {
        public static ObservableCollection<ProcessItem> GetProcessList()
        {
            ObservableCollection<ProcessItem> items = [];

            var processes = Process.GetProcesses()
                .OrderByDescending(p =>
                {
                    try
                    {
                        return p.WorkingSet64;
                    }
                    catch
                    {
                        return 0;
                    }
                })
                .Take(10);

            foreach (Process proc in processes)
            {
                try
                {
                    items.Add(new ProcessItem(proc));
                }
                catch
                {
                    return items;
                }
            }

            return items;
        }

        public static ObservableCollection<ProcessItem> GetTopProcesses(int count = 10)
        {
            ObservableCollection<ProcessItem> items = [];

            Process[] processes = Process.GetProcesses();

            var topProcesses = processes
                .Select(proc =>
                {
                    try
                    {
                        return new
                        {
                            Process = proc,
                            Memory = proc.WorkingSet64
                        };
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(x => x != null)
                .OrderByDescending(x => x!.Memory)
                .Take(count);

            foreach (var item in topProcesses)
            {
                try
                {
                    items.Add(new ProcessItem(item!.Process));
                }
                catch
                {
                    continue;
                }
            }

            return items;
        }

        public static void UpdateProcesses(
            ObservableCollection<ProcessItem> processItems)
        {
            foreach (ProcessItem item in processItems)
            {
                try
                {
                    using Process proc = Process.GetProcessById(item.ProcessId);

                    item.MemoryUsage =
                        Math.Round(proc.WorkingSet64 / (1024.0 * 1024.0), 1);

                    item.CpuUsage = Math.Round(CpuUsage(proc, item), 1);
                }
                catch
                {
                    return;
                }
            }
        }

        public static double CpuUsage(
    Process proc,
    ProcessItem item)
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan currentProcessorTime = proc.TotalProcessorTime;

            double elapsedCpu =
                (currentProcessorTime - item.PreviousProcessorTime)
                .TotalMilliseconds;

            double elapsedTime =
                (currentTime - item.PreviousSampleTime)
                .TotalMilliseconds;

            if (elapsedTime <= 0)
                return 0;

            double cpuUsage =
                elapsedCpu /
                elapsedTime /
                Environment.ProcessorCount *
                100.0;

            item.PreviousProcessorTime = currentProcessorTime;
            item.PreviousSampleTime = currentTime;

            return cpuUsage;
        }

        public static void UpdateCpuUsage(ProcessItem item)
        {
            try
            {
                using Process proc =
                    Process.GetProcessById(item.ProcessId);

                DateTime currentTime = DateTime.Now;
                TimeSpan currentProcessorTime = proc.TotalProcessorTime;

                double elapsedCpu =
                    (currentProcessorTime - item.PreviousProcessorTime)
                    .TotalMilliseconds;

                double elapsedTime =
                    (currentTime - item.PreviousSampleTime)
                    .TotalMilliseconds;

                if (elapsedTime <= 0)
                    return;

                double cpuUsage =
                    elapsedCpu /
                    elapsedTime /
                    Environment.ProcessorCount *
                    100.0;

                item.CpuUsage = Math.Round(cpuUsage, 1);

                item.PreviousProcessorTime =
                    currentProcessorTime;

                item.PreviousSampleTime =
                    currentTime;
            }
            catch
            {
                // Process may have exited.
            }
        }
    }
}
