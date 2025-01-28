using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

[MemoryDiagnoser]
public class Program
{
    public static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}

//[SimpleJob(RuntimeMoniker.Net472, baseline: true)]
//[SimpleJob(RuntimeMoniker.NetCoreApp30)]
//[SimpleJob(RuntimeMoniker.NativeAot80)]
//[SimpleJob(RuntimeMoniker.Mono)]
//[RPlotExporter]
//public class Md5VsSha256
//{
//    private SHA256 sha256 = SHA256.Create();
//    private MD5 md5 = MD5.Create();
//    private byte[] data;

//    [Params(1000, 10000)]
//    public int N;

//    [GlobalSetup]
//    public void Setup()
//    {
//        data = new byte[N];
//        new Random(42).NextBytes(data);
//    }

//    [Benchmark]
//    public byte[] Sha256() => sha256.ComputeHash(data);

//    [Benchmark]
//    public byte[] Md5() => md5.ComputeHash(data);
//}

public class IntroMemoryRandomization
{
    [Params(512 * 4)]
    public int Size;

    private int[] array;
    private int[] destination;

    [GlobalSetup]
    public void Setup()
    {
        array = new int[Size];
        destination = new int[Size];
    }

    [Benchmark]
    [MemoryRandomization(false)]
    public void Array_RandomizationDisabled() => Array.Copy(array, destination, Size);

    [Benchmark]
    [MemoryRandomization(true)]
    [MaxIterationCount(40)] // the benchmark becomes multimodal and need a lower limit of max iterations than the default
    public void Array_RandomizationEnabled() => Array.Copy(array, destination, Size);
}