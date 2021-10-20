using BenchmarkDotNet.Running;
using TesteBenchmarkDataAccessSqlServer.Tests;

namespace TesteBenchmarkDataAccessSqlServer
{
    class Program
    {
        static void Main(string[] args)
        {
            new BenchmarkSwitcher(new [] { typeof(TestesDataAccess) }).Run(args);
        }
    }
}