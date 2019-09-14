using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EstagiosTCC.Models
{
    public class Internship : FirebaseModel
    {
        public string Uid { get; set; }
        public string OwnerUid { get; set; }
        public string[] UidCourses { get; set; }
        public string UrlAttachment { get; set; }
        public string Company { get; set; }
        public string Sector { get; set; }
        public string Address { get; set; }
        public string DescriptionOfActivities { get; set; }
        public string WorkSchedule { get; set; }
        public string Payment { get; set; }
        public string OtherAid { get; set; }
        public string LastUpdate { get; set; }
        public string Contact { get; set; }
        public string LinkForMoreInfo { get; set; }

        public Internship()
        {
            Uid = string.Empty;
            OwnerUid = string.Empty;
            UidCourses = new string[] { };
            UrlAttachment = string.Empty;
            Company = string.Empty;
            Sector = string.Empty;
            Address = string.Empty;
            DescriptionOfActivities = string.Empty;
            WorkSchedule = string.Empty;
            Payment = string.Empty;
            OtherAid = string.Empty;
            LastUpdate = string.Empty;
            Contact = string.Empty;
        }

        public async Task<bool> Insert(Stream attachment)
        {
            try
            {
                //CADASTRA AS INFORMAÇÕES INICIAIS DO ESTÁGIO
                var postInternship = await Database.Child("Internship").PostAsync(this);
                //PEGA O ID DO REGISTRO DO FIREBASE DO ESTÁGIO
                Uid = postInternship.Key;
                if (attachment != null)
                {
                    //CADASTRA A IMAGEM DO ANEXO NO STORAGE
                    var urlAttachment = await Storage.Child("Attachment")
                        .Child(postInternship.Key + ".jpg").PutAsync(attachment);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA 
                    UrlAttachment = urlAttachment;
                }
                //DEFINE A ULTIMA VEZ EM QUE FOI ATUALIZADO
                LastUpdate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //PUT(UPDATE) NAS INFORMAÇÕES
                await Database.Child("Internship").Child(postInternship.Key).PutAsync(this);
                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        public async Task<bool> Updade(Stream attachment)
        {
            try
            {
                if (attachment != null)
                {
                    //CADASTRA A IMAGEM DO ANEXO NO STORAGE
                    var urlAttachment = await Storage.Child("Attachment")
                        .Child(Uid + ".jpg").PutAsync(attachment);
                    //PEGA O ENDEREÇO DA IMAGEM SALVA
                    UrlAttachment = urlAttachment;
                }
                //DEFINE A ULTIMA VEZ EM QUE FOI ATUALIZADO
                LastUpdate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //PUT(UPDATE) NAS INFORMAÇÕES
                await Database.Child("Internship").Child(Uid).PutAsync(this);
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
                try
                {
                    //DELETA A IMAGEM DE ANEXO DO ESTÁGIO SE TIVER
                    await Storage.Child("Attachment").Child(Uid + ".jpg").DeleteAsync();
                }
                catch { }
                //DELETA O REGISTRO DO ESTÁGIO
                await Database.Child("Internship").Child(Uid).DeleteAsync();
                return true;
            }
            catch (FirebaseException)
            {
                return false;
            }
        }

        public async Task<List<Internship>> Select()
        {
            try
            {
                return (await Database.Child("Internship").OnceAsync<Internship>())
                       .Select(item => new Internship
                       {
                           Uid = item.Object.Uid,
                           OwnerUid = item.Object.OwnerUid,
                           UidCourses = item.Object.UidCourses,
                           UrlAttachment = item.Object.UrlAttachment,
                           Company = item.Object.Company,
                           Sector = item.Object.Sector,
                           Address = item.Object.Address,
                           DescriptionOfActivities = item.Object.DescriptionOfActivities,
                           WorkSchedule = item.Object.WorkSchedule,
                           Payment = item.Object.Payment,
                           OtherAid = item.Object.OtherAid,
                           LastUpdate = item.Object.LastUpdate,
                           Contact = item.Object.Contact,
                           LinkForMoreInfo = item.Object.LinkForMoreInfo
                       }).ToList();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        public async Task<Internship> SelectByUid(string uid)
        {
            try
            {
                var allInternship = await Select();
                return allInternship.Where(a => a.Uid == uid).FirstOrDefault();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        public async Task<List<Internship>> SelectByCourse(string uidCourse)
        {
            try
            {
                var allInternship = await Select();
                return allInternship.Where(a => a.UidCourses.Contains(uidCourse)).ToList();
            }
            catch (FirebaseException)
            {
                return null;
            }
        }

        public async Task<List<Internship>> SelectByOwner(string ownerUid)
        {
            try
            {
                var allInternship = await Select();
                return allInternship.Where(a => a.OwnerUid == ownerUid).ToList(); 
            }
            catch (FirebaseException)
            {
                return null;
            }
        }
    }
}