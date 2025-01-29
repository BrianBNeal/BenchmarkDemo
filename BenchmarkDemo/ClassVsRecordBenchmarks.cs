using BenchmarkDotNet.Attributes;

namespace BenchmarkDemo;

public class PersonClass {
	public PersonClass(string first, string last) {
		FirstName = first;
		LastName = last;
	}

	public string FirstName { get; }
		public string LastName { get; }
	}

// Reference-type Record
public record PersonRecord(string FirstName, string LastName);

[MemoryDiagnoser]
public class ClassVsRecordBenchmarks {
	private const int N = 1_000;
	private Dictionary<PersonClass, int> _classDictionary;
	private Dictionary<PersonRecord, int> _recordDictionary;
	private List<PersonClass> _classList;
	private List<PersonRecord> _recordList;

	[GlobalSetup]
	public void Setup() {
		_classDictionary = new Dictionary<PersonClass, int>();
		_recordDictionary = new Dictionary<PersonRecord, int>();

		for (int i = 0; i < N; i++) {
			var pc = new PersonClass($"First{i}", $"Last{i}");
			_classDictionary[pc] = i;

			var pr = new PersonRecord($"First{i}", $"Last{i}");
			_recordDictionary[pr] = i;
		}
	}

	[Benchmark]
	public PersonClass CreateManyPersonClasses() {
		List<PersonClass> list = new();
		for (int i = 0; i < N; i++) {
			var person = new PersonClass($"First{i}", $"Last{i}");
			list.Add(person);
		}
		return list.Last();
	}

	[Benchmark]
	public PersonRecord CreateManyPersonRecords() {
		PersonRecord last = null;
		for (int i = 0; i < N; i++) {
			last = new PersonRecord($"First{i}", $"Last{i}");
		}
		return last;
	}

	[Benchmark]
	public bool EqualityCheckClass() {
		var p1 = new PersonClass("John", "Doe");
		var p2 = new PersonClass("John", "Doe");
		return p1.Equals(p2);
	}

	[Benchmark]
	public bool EqualityCheckRecord() {
		var p1 = new PersonRecord("John", "Doe");
		var p2 = new PersonRecord("John", "Doe");
		return p1.Equals(p2);
	}

	[Benchmark]
	public int DictionaryLookupsClass() {
		int foundCount = 0;
		for (int i = 0; i < N; i++) {
			// Re-create the same key for each iteration
			var key = new PersonClass($"First{i}", $"Last{i}");
			if (_classDictionary.TryGetValue(key, out _)) {
				foundCount++;
			}
		}
		return foundCount;
	}

	[Benchmark]
	public int DictionaryLookupsRecord() {
		int foundCount = 0;
		for (int i = 0; i < N; i++) {
			var key = new PersonRecord($"First{i}", $"Last{i}");
			if (_recordDictionary.TryGetValue(key, out _)) {
				foundCount++;
			}
		}
		return foundCount;
	}
}