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
        var sw = System.Diagnostics.Stopwatch.StartNew();
        T result = func();
        sw.Stop();
        return new TimingResult<T>
        {
            Milliseconds = sw.ElapsedMilliseconds,
            Result = result
        };
    }
}