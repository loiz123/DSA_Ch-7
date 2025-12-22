using System;

public static class TimeUtils
{
    public class TimingResult<T>
    {
        public long Milliseconds { get; set; }
        public T Result { get; set; }
    }

    public static TimingResult<T> MeasureTime<T>(Func<T> func)
    {
        const int ITERATIONS = 1000;
        var sw = new System.Diagnostics.Stopwatch();

        T result = default(T);
        long totalMilliseconds = 0;

        // JIT warm-up
        func();

        for (int i = 0; i < ITERATIONS; i++)
        {
            // Clear GC để giảm nhiễu
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            sw.Restart();
            result = func();
            sw.Stop();

            totalMilliseconds += sw.ElapsedMilliseconds;
        }

        return new TimingResult<T>
        {
            Milliseconds = totalMilliseconds / ITERATIONS,
            Result = result
        };
    }
}
