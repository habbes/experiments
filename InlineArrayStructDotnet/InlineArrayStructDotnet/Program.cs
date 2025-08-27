using BenchmarkDotNet.Running;
using InlineArrayStructDotnet;
using System.Diagnostics;

//var opens = new char[100];
//var closes = new char[100];
//Array.Fill(opens, '(');
//Array.Fill(closes, ')');
//var input = new string(opens) + new string(closes);
//Console.WriteLine($"Input {input}");
//int iters = 1000_000;

//var std = new StandardCharStack();

//Console.WriteLine("Waiting to start next. Press Enter...");
//Console.ReadLine();


//RunTest(input, () => new HybridCharStack(), "Hybrid", iters);

//Console.WriteLine("Waiting to start next. Press Enter...");
//Console.ReadLine();

//RunTest(input, () => new StandardCharStack(200), "Standard", iters);



//static void RunTest<TStack>(string input, Func<TStack> createStack, string name, int iters) where TStack : ICharStack, new()
//{
//    BracketsMatcher.AreBracketsBalanced(input, createStack()); // warmup
//    var sw = Stopwatch.StartNew();
//    bool res = false;
//    for (int i = 0; i < iters; i++)
//    {
//        res = BracketsMatcher.AreBracketsBalanced(input, createStack());
//    }
//    Console.WriteLine($"{name}: " + sw.ElapsedMilliseconds);
//}


BenchmarkRunner.Run<Benchmarks>();
