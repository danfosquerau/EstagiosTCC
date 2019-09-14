using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstagiosTCC.Models
{
    public class Course : FirebaseModel
    {
        public string Uid { get; set; }
        public string CourseName { get; set; }
        public string LastUpdate { get; set; }

        public Course()
        {
            Uid = string.Empty;
            CourseName = string.Empty;
            LastUpdate = string.Empty;
        }

        public async Task<bool> Insert()
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO CURSO
                var postCourse = await Database.Child("Course").PostAsync(this);
                //PEGA A KEY DO REGISTRO DO FIREBASE E SETA COMO ID DO CURSO
                Uid = postCourse.Key;
                //DEFINE A ULTIMA VEZ EM QUE FOI ATUALIZADO
                LastUpdate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //PUT(UPDATE) NAS INFORMAÇÕES
                await Database.Child("Course").Child(postCourse.Key).PutAsync(this);
                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        public async Task<bool> Updade()
        {
            try
            {
                //DEFINE A ULTIMA VEZ EM QUE FOI ATUALIZADO
                LastUpdate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //PUT(UPDATE) NAS INFORMAÇÕES
                await Database.Child("Course").Child(Uid).PutAsync(this);
                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        public async Task<bool> Delete()
        {
            try
            {
                //DELETA O REGISTRO DO CURSO
                await Database.Child("Course").Child(Uid).DeleteAsync();
                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        public async Task<List<Course>> Select()
        {
            try
            {
                return (await Database.Child("Course").OnceAsync<Course>())
                       .Select(item => new Course
                       {
                           Uid = item.Object.Uid,
                           CourseName = item.Object.CourseName,
                           LastUpdate = item.Object.LastUpdate
                       }).ToList();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        public async Task<Course> SelectByUid(string uid)
        {
            try
            {
                var allCourses = await Select();
                return allCourses.Where(a => a.Uid == uid).FirstOrDefault();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }
    }
}