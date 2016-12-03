using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using DevDaysSpeakers.Model;
using DevDaysSpeakers.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace DevDaysSpeakers.ViewModel
{
	public class SpeakersViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<Speaker> Speakers { get; set; }
		public SpeakersViewModel() 
		{
			Speakers = new ObservableCollection<Speaker>();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		bool busy;
		public bool IsBusy
		{ 
			get { return busy; }
			set {
				busy = value;
				OnPropertyChanged();
			}
		}

	}
}
