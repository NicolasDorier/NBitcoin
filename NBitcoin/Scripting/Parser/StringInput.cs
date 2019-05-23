using System;
using System.Collections;
using System.Collections.Generic;

namespace NBitcoin.Scripting.Parser
{
	internal class StringInput : IInput<char>
	{
		public StringInput(string source) : this(source, 0) { }
		public string Source { get; }
		public int Position { get; }

		internal StringInput(string source, int position)
		{
			if (source == null)
				throw new System.ArgumentNullException(nameof(source));
			Source = source;
			Position = position;
			Memos = new Dictionary<object, object>();
		}

		public bool AtEnd { get { return Position == Source.Length; } }
		public char GetCurrent() => Source[Position];

		public IInput<char> Advance()
		{
			if (AtEnd)
				throw new InvalidOperationException("The input is already at the end of the source");
			return new StringInput(Source, Position + 1);
		}

		public IEnumerator<char> GetEnumerator()
		{
			return this.Source.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.Source.GetEnumerator();
		}

		public IDictionary<object, object> Memos { get; }
	}
}