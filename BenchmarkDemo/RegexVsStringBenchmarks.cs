using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace BenchmarkDemo;

[MemoryDiagnoser]
public class RegexVsStringSearch {
	private const string SearchTerm = "BenchmarkDotNet";
	private const int RepeatCount = 1000;
	private string _inputText;
	private Regex _regex;
	private Regex _compiledRegex;

	[GlobalSetup]
	public void Setup() {
		// Build a long input string by repeating a phrase multiple times.
		// Insert the search term occasionally to ensure it’s present.
		var phrase = "Hello world! We're learning about BenchmarkDotNet. It's super duper interesting to everyone. ";
		_inputText = string.Join(' ', new string[RepeatCount].Select(_ => phrase));

		// 1) A "regular" Regex (no compilation)
		_regex = new Regex(SearchTerm, RegexOptions.CultureInvariant);

		// 2) A compiled Regex
		_compiledRegex = new Regex(SearchTerm, RegexOptions.Compiled | RegexOptions.CultureInvariant);
	}

	[Benchmark]
	public bool StringContains() {
		return _inputText.Contains(SearchTerm);
	}

	[Benchmark]
	public int StringIndexOf() {
		return _inputText.IndexOf(SearchTerm);
	}

	[Benchmark]
	public bool RegexIsMatch() {
		return _regex.IsMatch(_inputText);
	}

	[Benchmark]
	public bool CompiledRegexIsMatch() {
		return _compiledRegex.IsMatch(_inputText);
	}
}