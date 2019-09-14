using EstagiosTCC.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace EstagiosTCC.ViewModels
{
    public class CourseViewModel : BaseViewModel
    {
        public ICommand SaveCommand { get; set; }
        public ICommand CleanCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ObservableCollection<Course> ListCourses { get; set; }

        private Course _selectedCourse;
        public Course SelectedCourse
        {
            get { return _selectedCourse; }
            set { SetProperty(ref _selectedCourse, value); }
        }

        public CourseViewModel()
        {
            Title = "Cursos";
            LoadCourses();
            SaveCommand = new Command(() => OnSave());
            CleanCommand = new Command(() => OnClean());
            DeleteCommand = new Command(() => OnDelete());
        }

        private async void LoadCourses()
        {
            ListCourses = new ObservableCollection<Course>();
            SelectedCourse = new Course();
            var response = await SelectedCourse.Select();
            foreach (Course item in response)
                ListCourses.Add(item);
        }

        private async Task UpdateOnView()
        {
            ListCourses.Clear();
            SelectedCourse = new Course();
            var response = await SelectedCourse.Select();
            foreach (Course item in response)
                ListCourses.Add(item);
        }

        private async void OnSave()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrEmpty(SelectedCourse.CourseName))
                    DisplayDeniedPrompt();
                else
                {
                    bool response = false;

                    if (string.IsNullOrEmpty(SelectedCourse.Uid))
                        response = await SelectedCourse.Insert();
                    else
                        response = await SelectedCourse.Updade();

                    if (response)
                        DisplaySuccessPrompt();
                    else
                        DisplayFailedPrompt();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                await UpdateOnView();
                IsBusy = false;
            }
        }

        public async void OnClean()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                await UpdateOnView();
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

        public async void OnDelete()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            try
            {
                if (string.IsNullOrEmpty(SelectedCourse.Uid))
                    DisplayDeniedPrompt();
                else
                {
                    bool resultado = await SelectedCourse.Delete();

                    if (resultado)
                        DisplaySuccessPrompt();
                    else
                        DisplayFailedPrompt();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                await UpdateOnView();
                IsBusy = false;
            }
        }
    }
}