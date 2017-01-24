using Form_Post_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;


namespace Form_Post_MVC.Controllers
{
    public class HomeController : Controller
    {


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserModel user)
        {
            int userId = user.UserId;
            string name = user.Name;
            string gender = user.Gender;
            string city = user.City;

            TempData["username"] = name;


            // Bind to the domain that this user is currently connected to.
            DirectoryEntry dom = new DirectoryEntry("LDAP://adtest", "Administrator", "Testtest01");

            // Find the container (in this case, the Consulting organizational unit) that you 
            // wish to add the new group to.
            DirectoryEntry ou = dom.Children.Find("OU=OU1");

            // Add the new group Practice Managers.
            DirectoryEntry group = ou.Children.Add("CN=Practice Managers2", "group");

            // Set the samAccountName for the new group.
            group.Properties["samAccountName"].Value = "pracmans2";

            // Commit the new group to the directory.
            group.CommitChanges();

            ////////////////////////////////////////////////////////////////////////////



            //DirectoryEntry SampleOU = new DirectoryEntry("LDAP://OU=OU1, DC=local");

            //DirectoryEntry groupEnt = SampleOU.Children.Add("CN=TestGroup", "Group");

            //using (groupEnt)
            //{
            //    groupEnt.Properties["sAmAccountName"].Value = "TestGroup";
            //    groupEnt.CommitChanges();
            //}
            ////////////////////////////////////////////////////////////////////////////


            //DirectoryEntry ent = new DirectoryEntry("WinNT://adtest.my.domain", "Administrator", "Testtest01");
            ////DirectoryEntry ent = new DirectoryEntry();
            ////DirectoryEntry ent = new DirectoryEntry("WinNT://" + Environment.MachineName + ",computer");
            //DirectoryEntry ou = ent.Children.Find("OU=OU1");


            ////// Use the Add method to add a user to an organizational unit.
            //DirectoryEntry usr = ou.Children.Add("TestUser1", "user");
            ////// Set the samAccountName, then commit changes to the directory.
            //usr.Properties["samAccountName"].Value = "newuser";


            ////usr.Invoke("SetPassword", new object[] { "#12345Abc" });
            ////usr.Invoke("Put", new object[] { "Description", "Test User from .NET" });

            //usr.CommitChanges();



            //{
            //    directoryentry ad = new directoryentry("winnt://" +
            //                        environment.machinename + ",computer");
            //    directoryentry newuser = ad.children.add("cn=testuser1", "user");

            //    newuser.properties["samaccountname"].value = "tt1";

            //    newuser.commitchanges();

            //}



            return RedirectToAction("SimpleMethod", "Home");
        }

        public string SimpleMethod()
        {
            string username = TempData["username"] as String;


            return username+": You have clicked submit." + "..";
        }


    }
}