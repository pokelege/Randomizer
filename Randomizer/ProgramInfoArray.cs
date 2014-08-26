using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Randomizer
{
	[Serializable]
	public class ProgramInfoArray
	{
		public List<ProgramInfo> Programs;

		public ProgramInfoArray()
		{
			Programs = new List<ProgramInfo>();
		}
		public Process random(string name = "")
		{
			if ( Programs == null || Programs.Count == 0 ) return null;
			Process toReturn = null;
			if(string.IsNullOrEmpty(name))
			{
				Random rand = new Random( DateTime.Now.Millisecond );
				toReturn = Programs[rand.Next( 0, Programs.Count )].random();
			}
			else
			{
				ProgramInfo program = ( from n in Programs where n.Code == name select n ).FirstOrDefault();
				if ( program != null ) toReturn = program.random();
			}
			return toReturn;
		}
	}
}
