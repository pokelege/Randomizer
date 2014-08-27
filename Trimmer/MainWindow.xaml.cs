using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace Trimmer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void BrowseFolder( object sender, RoutedEventArgs e )
		{
			FolderBrowserDialog Browse = new FolderBrowserDialog();
			Browse.ShowDialog();
			TrimDirectory.Text = Browse.SelectedPath;
		}
		private void Trim( object sender, RoutedEventArgs e )
		{
			string[] file = Directory.GetFiles( TrimDirectory.Text, "*", System.IO.SearchOption.AllDirectories );
			foreach (string i in file)
			{
				try
				{
					string fileName = Path.GetFileNameWithoutExtension( i );
					string[] theTrim = fileName.Split( "[]()".ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
					string theResult = theTrim[0];
					string thePath = Path.GetDirectoryName( i );
					if ( !thePath.EndsWith( "" + Path.DirectorySeparatorChar ) )
					{
						thePath += Path.DirectorySeparatorChar;
					}
					File.Move( i, thePath + theResult + Path.GetExtension( i ) );
				}
				catch(Exception lolz)
				{

				}
			}
		}
	}
}
