﻿using System;
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
		public Command GetSpeakersCommand { get; set; }

		public SpeakersViewModel() 
		{
			Speakers = new ObservableCollection<Speaker>();
			GetSpeakersCommand = new Command(
				async () => await GetSpeakers(),
				() => !IsBusy);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged([CallerMemberName] string name = null) =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

		bool busy;
		public bool IsBusy
		{ 
			get { return busy; }
			set
			{
				busy = value;
				OnPropertyChanged();
				//Update the can execute
				GetSpeakersCommand.ChangeCanExecute();
			}
		}

		// インターネットから speaker のデータをすべて取ってくる
		async Task GetSpeakers()
		{
			if (IsBusy)
				return;

			Exception error = null;
			try
			{
				IsBusy = true;

				var service = DependencyService.Get<AzureService>();
				var items = await service.GetSpeakers();

				Speakers.Clear();
				foreach (var item in items)
					Speakers.Add(item);
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Error: " + ex);
				error = ex;
			}
			finally
			{
				IsBusy = false;
			}

			if (error != null)
				await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
		}
	}
}
