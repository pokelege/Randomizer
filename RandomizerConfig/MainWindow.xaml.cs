using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Linq;
using Randomizer;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
namespace RandomizerConfig
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private ProgramInfoArray programInfo;
		public MainWindow()
		{
			InitializeComponent();
			try
			{
				if ( File.Exists( "settings.bin" ) )
				{
					IFormatter formatter = new BinaryFormatter();
					Stream stream = new FileStream( "settings.bin", FileMode.Open, FileAccess.Read, FileShare.Read );
					programInfo = ( ProgramInfoArray )formatter.Deserialize( stream );
				}
				else
				{
					programInfo = new ProgramInfoArray();
				}
			}
			catch(Exception e)
			{
				programInfo = new ProgramInfoArray();
			}
			ExeCode.ItemsSource = from n in programInfo.Programs select n.Code;
			ExeCode.SelectedIndex = 0;
			Executable.Text = ( from n in programInfo.Programs where n.Code == ( string )ExeCode.SelectedItem select n.Executable ).FirstOrDefault();
			GamePath.Text = ( from n in programInfo.Programs where n.Code == ( string )ExeCode.SelectedItem select n.GamePath ).FirstOrDefault();
			Extension.Text = ( from n in programInfo.Programs where n.Code == ( string )ExeCode.SelectedItem select n.Extensions ).FirstOrDefault();
			LeftArgs.Text = ( from n in programInfo.Programs where n.Code == ( string )ExeCode.SelectedItem select n.ParamsLeft ).FirstOrDefault();
			RightArgs.Text = ( from n in programInfo.Programs where n.Code == ( string )ExeCode.SelectedItem select n.ParamsRight ).FirstOrDefault();
		}

		private void BrowseExe( object sender, RoutedEventArgs e )
		{
			OpenFileDialog Browse = new OpenFileDialog();
			Browse.ShowDialog();
			Executable.Text = Browse.FileName;
		}

		private void BrowseGame( object sender, RoutedEventArgs e )
		{
			FolderBrowserDialog Browse = new FolderBrowserDialog();
			Browse.ShowDialog();
			GamePath.Text = Browse.SelectedPath;
		}

		private void Delete(object sender, RoutedEventArgs e )
		{
			programInfo.Programs.Remove( (from n in programInfo.Programs where n.Code == (string)ExeCode.SelectedItem select n).FirstOrDefault() );
			ExeCode.ItemsSource = from n in programInfo.Programs select n.Code;
		}

		private void Save( object sender, RoutedEventArgs e )
		{
			try
			{
				string[] files = Directory.GetFiles( Path.GetDirectoryName( System.Windows.Application.ResourceAssembly.Location ), "*.dat", System.IO.SearchOption.AllDirectories );
				foreach ( string j in files )
				{
					File.Delete( j );
				}


				ProgramInfo i = ( from n in programInfo.Programs where n.Code == (string)ExeCode.SelectedItem select n ).FirstOrDefault();
				i.Executable = Executable.Text;
				i.GamePath = GamePath.Text;
				i.Extensions = Extension.Text;
				i.ParamsLeft = LeftArgs.Text;
				i.ParamsRight = RightArgs.Text;

				for ( int j = 0; j < programInfo.Programs.Count;j++ )
				{
					if ( !programInfo.Programs[j].GamePath.EndsWith( "" + Path.DirectorySeparatorChar ) )
					{
						programInfo.Programs[j].GamePath += Path.DirectorySeparatorChar;
					}

					List<string> batchGames = new List<string>();

					if(string.IsNullOrEmpty(programInfo.Programs[j].Extensions))
					{
						batchGames.AddRange(Directory.GetFiles( programInfo.Programs[j].GamePath, "*.*", System.IO.SearchOption.AllDirectories ).Where( n => !n.StartsWith( "[" ) ).Select( n => n ));
					}
					else
					{
						string[] extensions = programInfo.Programs[j].Extensions.Split( ",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries );
						foreach(string l in extensions)
						{
							batchGames.AddRange( Directory.GetFiles( programInfo.Programs[j].GamePath, "*." + l, System.IO.SearchOption.AllDirectories ).Where( n => !n.StartsWith( "[" ) ).Select( n => n ) );
						}
					}
					File.WriteAllLines( programInfo.Programs[j].Code + ".dat", batchGames );
				}

				IFormatter formatter = new BinaryFormatter();
				Stream stream = new FileStream( "settings.bin", FileMode.Create, FileAccess.Write, FileShare.None );
				formatter.Serialize( stream, programInfo );
				stream.Close();

				System.Windows.MessageBox.Show( "Saved", "Success", MessageBoxButton.OK, MessageBoxImage.None );
			}
			catch (Exception ex)
			{
				System.Windows.MessageBox.Show( "Problem Saving\n" + ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error );
			}
		}

		private void Add( object sender, RoutedEventArgs e )
		{
			string toProcess = (string)ExeCode.Text;
			ProgramInfo i = ( from n in programInfo.Programs where n.Code == toProcess select n ).FirstOrDefault();
			if ( i == null )
			{
				i = new ProgramInfo();
				i.Code = toProcess;
				i.Executable = Executable.Text;
				i.GamePath = GamePath.Text;
				i.Extensions = Extension.Text;
				i.ParamsLeft = LeftArgs.Text;
				i.ParamsRight = RightArgs.Text;
				programInfo.Programs.Add( i );
			}
			else
			{
				i.Executable = Executable.Text;
				i.GamePath = GamePath.Text;
				i.Extensions = Extension.Text;
				i.ParamsLeft = LeftArgs.Text;
				i.ParamsRight = RightArgs.Text;
			}
			ExeCode.ItemsSource = from n in programInfo.Programs select n.Code;
			ExeCode.SelectedValue = toProcess;
		}

		private void ExeCode_SelectionChanged( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
		{
			if(e.AddedItems.Count > 0)
			{
				ProgramInfo i = ( from n in programInfo.Programs where n.Code == ( string )e.AddedItems[0] select n ).FirstOrDefault();
				if ( i != null )
				{
					Executable.Text = i.Executable;
					GamePath.Text = i.GamePath;
					Extension.Text = i.Extensions;
					LeftArgs.Text = i.ParamsLeft;
					RightArgs.Text = i.ParamsRight;
				}
			}
		}
	}
}
