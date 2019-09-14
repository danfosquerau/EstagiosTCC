using EstagiosTCC.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModels
{
    public class MyInternshipsViewModel : BaseViewModel
    {
        public ICommand AllInternshipCommand;

        public ObservableCollection<Course> ListCourses { get; set; }
        public ObservableCollection<Internship> ListInternships { get; set; }


        public MyInternshipsViewModel()
        {
            Title = "Meus Estágios";
            LoadCourses();
            LoadInternship();
            AllInternshipCommand = new Command(() => OnAllInternship());
        }

        private async void LoadCourses()
        {
            ListCourses = new ObservableCollection<Course>() { new Course() { CourseName = "Selecione um curso" } };
            var response = await new Course().Select();
            foreach (Course item in response)
                ListCourses.Add(item);
        }

        private async void LoadInternship()
        {
            ListInternships = new ObservableCollection<Internship>();
            var response = await new Internship().Select();
            foreach (Internship item in response)
                ListInternships.Add(item);
        }

        public async void UpdateListInternship(string uid)
        {
            ListInternships.Clear();
            var response = await new Internship().SelectByCourse(uid);
            foreach (Internship item in response)
                ListInternships.Add(item);
        }

        private async void OnAllInternship()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                ListInternships.Clear();
                var responseInternship = await new Internship().Select();
                foreach (Internship item in responseInternship)
                    ListInternships.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}