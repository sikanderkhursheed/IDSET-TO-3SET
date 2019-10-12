using System.Collections.Generic;
using System.Linq;

namespace _3SAT
{
	public class Problem3Sat
	{
		public int M { get; }
		public int K { get; }

		public List<HashSet<int>> Clauses { get; }
		public List<int> Solution { get; set; }

		public Problem3Sat ( int m, int k )
		{
			M = m;
			K = k;

			Clauses = new List<HashSet<int>> ( M );
		}

		public void AddClause ( IEnumerable<int> variables )
		{
			Clauses.Add ( new HashSet<int> ( variables ) );
		}

		private static string AsString ( int i ) =>
			i < 0 ? string.Format ( "~X{0}", -i ) : string.Format ( "X{0}", i );

		private static string AsString ( IEnumerable<int> enumerable, string separator ) =>
			string.Join ( separator, enumerable.Select ( AsString ).ToArray ( ) );

		private string AsString ( IEnumerable<int> enumerable ) =>
			string.Format (
				"({0})", string.Join ( "v", enumerable.Select ( x => Solution.Contains ( x ) ? "1" : "0" ).ToArray ( ) ) );

		public override string ToString ( )
		{
			var result = string.Empty;

			result += "Function: ";
			result += string.Join ( "^", Clauses.Select ( x => string.Format ( "({0})", AsString ( x, "v" ) ) ).ToArray ( ) );
			result += "\n\n";

			if ( Solution == null )
				return result + "No solution.";

			result += string.Format ( "Solution: {0}\n\n", AsString ( Solution, "^" ) );
			result += string.Join ( "^", Clauses.Select ( AsString ).ToArray ( ) );

			return result;
		}
	}
}