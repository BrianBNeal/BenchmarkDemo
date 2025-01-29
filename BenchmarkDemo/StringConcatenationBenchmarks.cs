using BenchmarkDotNet.Attributes;
using System.Text;

namespace BenchmarkDemo;

public class StringConcatenationBenchmarks {
	
	/* 
	 * Things to avoid:
	 * - returning void can lead to inaccurate results due to JIT skipping your code
	 * - comparing results from methods that return different types
	 * - not optimizing the build
	 * - running with debugger attached
	 */


	[Params(50,500)]
	public int Iterations;

	[Benchmark]
	public string StringConcatenation() {
		var result = string.Empty;

		for (int i = 0; i < Iterations; i++) {
			result += "Line " + i + "; ";
		}
		return result;
	}

	[Benchmark]
	public string StringBuilderConcatenation() {
		var sb = new StringBuilder();
		for (int i = 0; i < Iterations; i++) {
			sb.Append("Line ").Append(i).Append("; ");
		}
		return sb.ToString();
	}
}
