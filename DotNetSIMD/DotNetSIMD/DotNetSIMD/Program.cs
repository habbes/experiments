using BenchmarkDotNet.Running;
using DotNetSIMD;

//BenchmarkSwitcher.FromAssembly(typeof(Benchmarks).Assembly).Run(args);


var benchmark = new Benchmarks();
benchmark.dataSize = 0x1000;

benchmark.Setup();
benchmark.MemberWiseSumSIMD();
