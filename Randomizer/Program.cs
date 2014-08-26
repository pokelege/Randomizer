using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer
{
	class Program
	{
		static void Main( string[] args )
		{
			IFormatter formatter = new BinaryFormatter();
			Stream stream = new FileStream( "settings.bin", FileMode.Open, FileAccess.Read, FileShare.Read );
			ProgramInfoArray theRandom = ( ProgramInfoArray )formatter.Deserialize( stream );
			if(args.Length > 0)
			{
				theRandom.random(args[0]);
			}
			else
			{
				theRandom.random();
			}
		}
	}
}
