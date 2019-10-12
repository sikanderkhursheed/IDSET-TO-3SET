using System;

namespace _3SAT
{
	public class Program
	{
		public static void Main ( )
		{
			var problem = new Problem3Sat ( 4, 5 );

			problem.AddClause ( new[] {1, -2, 3} );
			problem.AddClause ( new[] {-1, -4, 2} );
			problem.AddClause ( new[] {4, -2, -3} );
			problem.AddClause ( new[] {2, -3, 1} );
			problem.AddClause ( new[] {1, -3, 4} );

			var graph = new Graph ( problem );
			problem.Solution = graph.SolveIdset ( );
			Console.WriteLine ( problem );

			Console.ReadLine ( );
		}
	}
}