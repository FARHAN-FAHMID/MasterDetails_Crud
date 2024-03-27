using MasterDetails_Crud.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace MasterDetails_Crud.Controllers
{
    public class ApplicantController : Controller
    {
        Master_Detail_CrudEntities db = new Master_Detail_CrudEntities();

        // GET: Applicant
        public ActionResult Index()
        {
            return View(db.Applicants.ToList());
        }
        public ActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.ApplicantExes.Add(new ApplicantEx
            {
                CompanyName = "",
                Designation = "",
                YearOfExperience = 0
            });

            return View(applicant);
        }
        [HttpPost]
        public ActionResult Create(Applicant applicant, string btn)
        {
            if (btn == "Add")
            {
                applicant.ApplicantExes.Add(new ApplicantEx());
            }
            if (btn == "Create")
            {
                if (applicant.Picture != null)
                {
                    string ext = Path.GetExtension(applicant.Picture.FileName);
                    if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                    {
                        applicant.TotalExprience = applicant.ApplicantExes.Sum(m => m.YearOfExperience);

                        string rootPath = Server.MapPath("~/");
                        string fileToSave = Path.Combine(rootPath, "Picture", applicant.Picture.FileName);
                        applicant.Picture.SaveAs(fileToSave);
                        applicant.PicPath = "~/Picture/" + applicant.Picture.FileName;
                        db.Applicants.Add(applicant);

                        if (db.SaveChanges() > 0)
                        {
                            return RedirectToAction("Index");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Provide A Valid Image, Such As JPG Or PNG");
                        return View(applicant);
                    }
                }
            }
            return View(applicant);
        }


        public ActionResult Edit(int id)
        {
            Applicant applicant = db.Applicants.Find(id);
            return View(applicant);
        }

        [HttpPost]
        public ActionResult Edit(Applicant applicant, string btn)
        {
            if (btn == "Add")
            {
                applicant.ApplicantExes.Add(new ApplicantEx());
            }
            if (btn == "Update")
            {
                if (applicant.Picture != null)
                {
                    string ext = Path.GetExtension(applicant.Picture.FileName);
                    if (ext == ".jpg" || ext == ".png")
                    {
                        applicant.TotalExprience = applicant.ApplicantExes.Sum(m => m.YearOfExperience);

                        string rootPath = Server.MapPath("~/");
                        string fileToSave = Path.Combine(rootPath, "Picture", applicant.Picture.FileName);
                        applicant.Picture.SaveAs(fileToSave);
                        applicant.PicPath = "~/Picture/" + applicant.Picture.FileName;

                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Provide A Valid Image, Such As JPG Or PNG");
                        return View(applicant);
                    }
                }
                else
                {
                    applicant.PicPath = applicant.PicPath;
                }

                db.Applicants.AddOrUpdate(applicant);
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(applicant);
        }

        public ActionResult Delete(int id)
        {
            var applicant = db.Applicants.Find(id);
            db.Applicants.Remove(applicant);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            Applicant applicant = db.Applicants.Find(id);

            //var Applica = db.Applicants.Include(applicant => applicant.Applicant_Exprience).ToList();
            return View(applicant);
        }


    }
}