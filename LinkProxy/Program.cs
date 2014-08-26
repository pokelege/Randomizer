using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkProxy
{
	class Program
	{
		static void Main( string[] args )
		{
			
			if(args.Length > 0)
			{
				ProcessStartInfo info = new ProcessStartInfo( args[0] );
				info.CreateNoWindow = true;
				Process whatever = Process.Start( info );
			}
		}
	}
}
