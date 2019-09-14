using EstagiosTCC.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModels
{
    public class InternshipDetailViewModel : BaseViewModel
    {
        public ICommand OpenWebCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public ObservableCollection<Course> ListCoursesOfInternship { get; set; }

        private Internship _selectedInternship;
        public Internship SelectedInternship
        {
            get { return _selectedInternship; }
            set { SetProperty(ref _selectedInternship, value); }
        }
        private string _urlAtachment = string.Empty;
        public string UrlAttachment
        {
            get { return _urlAtachment; }
            set { SetProperty(ref _urlAtachment, value); }
        }

        public InternshipDetailViewModel()
        {
            Title = "Detalhes do Estágio";
        }

        public InternshipDetailViewModel(Internship internship)
        {
            Title = "Detalhes do Estágio";
            SelectedInternship = internship;
            LoadResoures();
            CancelCommand = new Command(() => OnCancel());
            OpenWebCommand = new Command(() => Device.OpenUri(
                new Uri(string.IsNullOrEmpty(SelectedInternship.LinkForMoreInfo)? "https://xamarin.com/platform" : SelectedInternship.LinkForMoreInfo)));
        }

        private async void LoadResoures()
        {
            UrlAttachment = SelectedInternship.UrlAttachment;
            ListCoursesOfInternship = new ObservableCollection<Course>();
            var response = await new Course().Select();
            if (!string.IsNullOrEmpty(SelectedInternship.Uid))
            {
                for (int i = 0; i < SelectedInternship.UidCourses.Length; i++)
                {
                    foreach (Course c in response.ToList())
                    {
                        if (c.Uid == SelectedInternship.UidCourses[i])
                        {
                            ListCoursesOfInternship.Add(c);
                            break;
                        }
                    }
                }
            }
            OnPropertyChanged();
        }

        public async void OnCancel()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}