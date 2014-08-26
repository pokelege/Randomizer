using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Randomizer
{
	[Serializable]
	public class ProgramInfo
	{
		public string Code;
		public string Executable;
		public string GamePath;
		public string Extensions;
		public string ParamsLeft, ParamsRight;

		public Process random()
		{
			Process theProcess = null;
			try
			{
				if ( File.Exists( Code + ".dat" ) )
				{
					string[] games = File.ReadAllLines(Code + ".dat");
					Random rand = new Random( DateTime.Now.Millisecond );
					ProcessStartInfo info = null;
					if(string.IsNullOrEmpty(Executable))
					{
						info = new ProcessStartInfo( games[rand.Next(0, games.Length)] );
						info.Arguments = ParamsLeft + ParamsRight;
					}
					else
					{
						info = new ProcessStartInfo( Executable );
						info.WorkingDirectory = Path.GetDirectoryName(Executable);
						info.Arguments = ParamsLeft + "\"" + games[rand.Next( 0, games.Length )] + "\"" + ParamsRight;
					}

					
					info.CreateNoWindow = true;
					
					theProcess = Process.Start( info );
				}
			}
			catch (Exception e)
			{

			}
			return theProcess;
		}
	}
}
