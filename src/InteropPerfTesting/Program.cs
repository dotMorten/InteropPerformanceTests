using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace InteropPerfTesting
{
    class Program
    {
        static long freq = 0;
        static void Main(string[] args)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                Console.WriteLine("WARNING: Debugger is attached and will affect performance numbers");
#if DEBUG
            Console.WriteLine("WARNING: Test is run using a debug build");
#endif
            QueryPerformanceFrequency(out freq);
            while (true)
            {
                Console.WriteLine("\nRUNNING TESTS...");
                ITestObject mtobj = new ManagedTestObject();
                var managedDuration = RunTest(mtobj);
                Console.WriteLine($"Managed object test\n\t\t\t{Math.Round(managedDuration, 5)} seconds");

                double duration1;
                using (var tobj = new NativeTestObject())
                {
                    duration1 = RunTest(tobj);
                    Console.WriteLine($"Native object test\n\t\t\t{Math.Round(duration1, 5)} seconds. (diff: {Math.Round(duration1 / managedDuration, 2)}x)");
                }
                using (var tobj = new SuppresUnmanagedCASTestObject())
                {
                    var duration2 = RunTest(tobj);
                    Console.WriteLine($"Suppress CAS native object test\n\t\t\t{Math.Round(duration2, 5)} seconds (diff: {Math.Round(duration2 / managedDuration, 2)}x,  {Math.Round(duration1 / duration2, 2)}x)");
                }

                Console.ReadKey();
            }
        }
        public static double RunTest(ITestObject obj)
        {
            long startTime = 0;
            long stopTime = 0;
            QueryPerformanceCounter(out startTime);
            for (int i = 0; i < 10000000; i++)
            {
                obj.DoubleValue = 5;
                double v = obj.DoubleValue;
            }
            QueryPerformanceCounter(out stopTime);
            double duration = (double)(stopTime - startTime) / (double)freq;
            return duration;
        }


        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(
           out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(
            out long lpFrequency);
    }

    public interface ITestObject
    {
        double DoubleValue { get; set; }
    }

    public class ManagedTestObject : ITestObject
    {
        public double DoubleValue { get; set; }
    }

    public class NativeTestObject : ITestObject, IDisposable
    {
        private IntPtr handle;
        public NativeTestObject()
        {
            handle = TestObject_Create();
        }

        ~NativeTestObject()
        {
            TestObject_Destroy(handle);
        }

        public double DoubleValue
        {
            get { return TestObject_GetDouble(handle); }
            set { TestObject_SetDouble(handle, value); }
        }


        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr TestObject_Create();

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double TestObject_Destroy(IntPtr o);

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern double TestObject_GetDouble(IntPtr o);

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern void TestObject_SetDouble(IntPtr o, double value);

        public void Dispose()
        {
            try
            {
                TestObject_Destroy(handle);
            }
            finally
            {
                handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        internal bool IsDisposed { get { return handle == IntPtr.Zero; } }
    }

    public class SuppresUnmanagedCASTestObject : ITestObject, IDisposable
    {
        private IntPtr handle;
        public SuppresUnmanagedCASTestObject()
        {
            handle = TestObject_Create();
        }

        ~SuppresUnmanagedCASTestObject()
        {
            TestObject_Destroy(handle);
        }

        public double DoubleValue
        {
            get { return TestObject_GetDouble(handle); }
            set { TestObject_SetDouble(handle, value); }
        }

        public void Dispose()
        {
            try
            {
                TestObject_Destroy(handle);
            }
            finally
            {
                handle = IntPtr.Zero;
            }
            GC.SuppressFinalize(this);
        }

        internal bool IsDisposed { get { return handle == IntPtr.Zero; } }

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern IntPtr TestObject_Create();

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        private static extern double TestObject_Destroy([In] IntPtr o);

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [System.Security.SuppressUnmanagedCodeSecurity] // Roughly 1.6x speedup. But dangerous
        private static unsafe extern double TestObject_GetDouble([In] IntPtr o);

        [DllImport("NativeLibrary.dll", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        [System.Security.SuppressUnmanagedCodeSecurity] // Roughly 1.6x speedup. But dangerous
        private static unsafe extern void TestObject_SetDouble([In] IntPtr o, [In] double value);

    }
}
