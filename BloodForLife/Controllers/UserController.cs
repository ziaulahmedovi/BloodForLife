using BloodForLifeEntity;
using BloodForLifeService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodForLife.Controllers
{
    public class UserController : Controller
    {
        private IService<User> service = new Service<User>();
        private IUserService user = new UserService();


        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registration(User u)
        {
            if (ModelState.IsValid)
            {
                this.service.Insert(u);
                return RedirectToAction("../User/Login");
            }

            else
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {

            User usr = this.user.LoginValidation(user);

            if (usr != null)
            {
                Session["Id"] = usr.Id.ToString();
                Session["userName"] = usr.userName.ToString();
                return RedirectToAction("Profile");
            }
            else
            {
                //ModelState.AddModelError("", "Username or password is wrong.");
                return RedirectToAction("../Home/Index");
            }

        }

        [HttpGet]
        public ActionResult Profile()
        {
            if (Session["Id"] != null)
            {
                int Id = Convert.ToInt32(Session["Id"]);
                return View(this.service.Get(Id));
            }
            else
            {
                return RedirectToAction("Registration");
            }

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (Session["Id"] != null)
            {

                User user = this.service.Get(id);
          

            return View(user);
            }
            else return RedirectToAction("../User/Login");
        }


        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                this.service.Update(user);
                return RedirectToAction("Profile");
            }
            else
            {
                return View(user);
            }

        }


        [HttpGet]
        public ActionResult Search()
        {
            if (Session["Id"] != null)
            {

                return View();
            }
            else return RedirectToAction("../User/Login");
        }
        List<User> bloodList = new List<User>();
        [HttpPost]
        public ActionResult Search(User user)
        {
            List<User> Users = this.service.GetAll();

            foreach (User u in Users)
            {

                if (u.bloodGroup == user.bloodGroup && u.division == user.division)
                {

                    bloodList.Add(u);

                }

            }

            ViewBag.Users = bloodList;
            return View("BloodDonerList");
        }


        [HttpGet]
        public ActionResult Password()
        {
            if (Session["Id"] != null)
            {
                //String Id = Session["Id"];

                User user = this.service.Get(Convert.ToInt32(Session["Id"]));

            return View();
            }
            else return RedirectToAction("../User/Login");
        }

        [HttpPost]
        public ActionResult Password(User user)
        {

            if (ModelState.IsValid)
            {
                this.service.Update(user);
                return RedirectToAction("Profile");
            }
            else
            {
                return RedirectToAction("Profile");
            }
        }


        public ActionResult Logout()
        {


            //return View();
            Session["Id"] = null;
            Session["userName"] = null;
            // return RedirectToAction("Profile");
            return RedirectToAction("../Home/Index");
        }

    }


}
