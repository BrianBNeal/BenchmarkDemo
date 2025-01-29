using BenchmarkDotNet.Attributes;

namespace BenchmarkDemo;
[MemoryDiagnoser]
public class ToListBenchmarks {
	private List<int> _data;

	[GlobalSetup]
	public void Setup() {
		_data = Enumerable.Range(1, 1_000).ToList();
	}

	[Benchmark]
	public List<int> MultipleToListCalls() {
		// Notice the chain calls each create a new list:
		// 1) Where(...) -> List
		// 2) Select(...) -> List
		// 3) Where(...) -> List
		var result = _data
			.Where(x => x % 2 == 0)
			.ToList()
			.Select(x => x * 2)
			.ToList()
			.Where(x => x > 20)
			.ToList();

		return result;
	}

	[Benchmark]
	public List<int> SingleToListCall() {
		// Defer the creation of a list until the very end
		var result = _data
			.Where(x => x % 2 == 0)
			.Select(x => x * 2)
			.Where(x => x > 20)
			.ToList();

		return result;
	}
}
