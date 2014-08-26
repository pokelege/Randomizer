using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VJoyDisabler
{
	class Program
	{
		static void Main( string[] args )
		{
			if(args.Length > 1)
			{
				ProcessStartInfo info = new ProcessStartInfo( args[0] );
				info.WorkingDirectory = Path.GetDirectoryName( args[0] );
				info.Arguments = "-enable 0";
				info.CreateNoWindow = true;
				Process.Start(info);

				ProcessStartInfo info2 = new ProcessStartInfo( args[1] );
				info.WorkingDirectory = Path.GetDirectoryName( args[1] );
				info.CreateNoWindow = true;
				Process.Start( info2 );
			}
		}
	}
}