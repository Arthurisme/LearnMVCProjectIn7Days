using ADBasic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using Microsoft.VisualBasic.FileIO;


namespace ADBasic.Controllers
{
    public class HomeController : Controller
    {

        string StrCurrentTestOU = "OU1";


        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addGroup(DirectoryModel directory)
        {
            int userId = directory.UserId;
            string name = directory.Name;
            string operatorPerson = directory.OperatorPerson;
            string CN = directory.CN;
            string samAccountName = directory.SamAccountName;
            string gender = directory.Gender;
            string city = directory.City;


            try
            {

                // Bind to the domain that this user is currently connected to.
                //DirectoryEntry dom = new DirectoryEntry("LDAP://adtest", "Administrator", "Testtest01");
                DirectoryEntry dom = new DirectoryEntry("LDAP://192.168.86.130", "Administrator", "Testtest01");

                // Find the container (in this case, the Consulting organizational unit) that you 
                // wish to add the new group to.
                DirectoryEntry ou = dom.Children.Find("OU="+ StrCurrentTestOU);

                dom.RefreshCache();

                // Add the new group Practice Managers.
                DirectoryEntry group = ou.Children.Add("CN=" + CN, "group");

                // Set the samAccountName for the new group.
                group.Properties["samAccountName"].Value = samAccountName;

                // Commit the new group to the directory.
                group.CommitChanges();

            }
            catch (System.Runtime.InteropServices.COMException COMEx)
            {
                // If a COMException is thrown, then the following code example can catch the text of the error.
                // For more information about handling COM exceptions, see Handling Errors.
                Console.WriteLine(COMEx.ErrorCode);
            }

            ////////////////////////////////////////////////////////////////////////////


            TempData["operatorPerson"] = operatorPerson;
            
            TempData["feedbackString"] = "Active Directory is commited successfully. <br>" +  
                  "You added "+ CN + " as Group in the " + StrCurrentTestOU + ".";

            return RedirectToAction("FeedbackMethod", "Home");
        }

        [HttpPost]
        public ActionResult addOU(DirectoryModel directory)
        {
            int userId = directory.UserId;
            string name = directory.Name;
            string operatorPerson = directory.OperatorPerson;
            string CN = directory.CN;
            string samAccountName = directory.SamAccountName;
            string gender = directory.Gender;
            string city = directory.City;

            string strOUForAdd = directory.OU;
            string strOUDescription = directory.OUDescription;



            try {
                // Bind to the domain that this user is currently connected to.
                //DirectoryEntry dom = new DirectoryEntry("LDAP://adtest", "Administrator", "Testtest01");
                DirectoryEntry dom = new DirectoryEntry("LDAP://192.168.86.130", "Administrator", "Testtest01");

                // Find the container (in this case, the Consulting organizational unit) that you 
                // wish to add the new group to.
                DirectoryEntry ou = dom.Children.Find("OU="+ StrCurrentTestOU);


            dom.RefreshCache();


            // Add the new group Practice Managers.
            DirectoryEntry ouForAdd = ou.Children.Add("OU=" + strOUForAdd, "OrganizationalUnit");

            // Set the samAccountName for the new group.
            ouForAdd.Properties["description"].Add(strOUDescription);

            // Commit the new group to the directory.
            ouForAdd.CommitChanges();

        }

                 catch (System.Runtime.InteropServices.COMException COMEx)
            {
                // If a COMException is thrown, then the following code example can catch the text of the error.
                // For more information about handling COM exceptions, see Handling Errors.
                Console.WriteLine(COMEx.ErrorCode);
            }

            ////////////////////////////////////////////////////////////////////////////


            TempData["operatorPerson"] = operatorPerson;

            TempData["feedbackString"] = "Active Directory is commited successfully. <br> "   
                + "You added " + strOUForAdd + " as OU in the " + StrCurrentTestOU + ".";

            return RedirectToAction("FeedbackMethod", "Home");
        }

        [HttpPost]
        public ActionResult createUser(DirectoryModel directory)
        {
            int userId = directory.UserId;
            string operatorPerson = directory.OperatorPerson;
            string name = directory.Name;
            string CN = directory.CN;
            string samAccountName = directory.SamAccountName;
            string gender = directory.Gender;
            string city = directory.City;

            string strOUForAdd = directory.OU;
            string strOUDescription = directory.OUDescription;

            string strUserForAdd = directory.UserName;



            try
            {
                // Bind to the domain that this user is currently connected to.
                //DirectoryEntry dom = new DirectoryEntry("LDAP://adtest", "Administrator", "Testtest01");
                DirectoryEntry dom = new DirectoryEntry("LDAP://192.168.86.130", "Administrator", "Testtest01");

                // Find the container (in this case, the Consulting organizational unit) that you 
                // wish to add the new group to.
                DirectoryEntry ou = dom.Children.Find("OU="+ StrCurrentTestOU);


                dom.RefreshCache();


                // Use the Add method to add a user to an organizational unit.
                DirectoryEntry usr = ou.Children.Add("CN="+ strUserForAdd, "user");
                // Set the samAccountName, then commit changes to the directory.
                usr.Properties["samAccountName"].Value = samAccountName;
                usr.CommitChanges();

                TempData["feedbackString"] = "Active Directory is commited successfully. <br> "
               + "You added " + strUserForAdd + " as User in the " + StrCurrentTestOU + ".";

            }

            catch (System.Runtime.InteropServices.COMException COMEx)
            {
                // If a COMException is thrown, then the following code example can catch the text of the error.
                // For more information about handling COM exceptions, see Handling Errors.
                Console.WriteLine(COMEx.ErrorCode);
                TempData["feedbackString"] = "There are some Error on the server or code, please contact admin.";
            }

            ////////////////////////////////////////////////////////////////////////////


            TempData["operatorPerson"] = operatorPerson;

           

            return RedirectToAction("FeedbackMethod", "Home");
        }

        [HttpPost]
        public ActionResult CreatBulkADUsersFromCSVFile(DirectoryModel directory)
        {

            string operatorPerson = directory.OperatorPerson;




            string csv_File_Path = @"C:\NewUsers.csv";
            TextFieldParser csvReader = new TextFieldParser(csv_File_Path);
            csvReader.SetDelimiters(new string[] { "," });
            csvReader.HasFieldsEnclosedInQuotes = true;

            // reading column fields 
            string[] colFields = csvReader.ReadFields();
            int index_Name = colFields.ToList().IndexOf("Name");
            int index_samaccountName = colFields.ToList().IndexOf("samAccountName");
            int index_ParentOU = colFields.ToList().IndexOf("ParentOU");
            while (!csvReader.EndOfData)
            {

             

                // reading user fields 
                string[] csvData = csvReader.ReadFields();
                //DirectoryEntry ouEntry = new DirectoryEntry("LDAP://" + csvData[index_ParentOU]);

                // Bind to the domain that this user is currently connected to.
                //DirectoryEntry dom = new DirectoryEntry("LDAP://adtest", "Administrator", "Testtest01");
                DirectoryEntry dom = new DirectoryEntry("LDAP://192.168.86.130", "Administrator", "Testtest01");

                DirectoryEntry ou = dom.Children.Find("OU=" + StrCurrentTestOU);


                try
                {

                    // Use the Add method to add a user to an organizational unit.
                    DirectoryEntry usr = ou.Children.Add("CN=" + csvData[index_Name], "user");
                    // Set the samAccountName, then commit changes to the directory.
                    usr.Properties["samAccountName"].Value = csvData[index_samaccountName];
                    usr.CommitChanges();

                    TempData["feedbackString"] = "Active Directory is commited successfully. <br> "
+ "You added bulk users in the " + StrCurrentTestOU + ".";


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    TempData["feedbackString"] = "There are some Error on the server or code, please contact admin.";
                }
            }
            csvReader.Close();

            TempData["operatorPerson"] = operatorPerson;

            return RedirectToAction("FeedbackMethod", "Home");
        }

        [HttpPost]
        public ActionResult Index(DirectoryModel directory)
        {
            int userId = directory.UserId;
            string name = directory.Name;
            string operatorPerson = directory.OperatorPerson;
            string gender = directory.Gender;
            string city = directory.City;



            // Bind to the domain that this user is currently connected to.
            DirectoryEntry dom = new DirectoryEntry("LDAP://adtest", "Administrator", "Testtest01");

            // Find the container (in this case, the Consulting organizational unit) that you 
            // wish to add the new group to.
            DirectoryEntry ou = dom.Children.Find("OU=OU1");

            // Add the new group Practice Managers.
            DirectoryEntry group = ou.Children.Add("CN=Practice Managers3", "group");

            // Set the samAccountName for the new group.
            group.Properties["samAccountName"].Value = "pracmans3";

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


            TempData["operatorPerson"] = operatorPerson;

            TempData["feedbackString"] = "Active Directory is commited successfully.";

            return RedirectToAction("FeedbackMethod", "Home");
        }

        public string FeedbackMethod()
        {
            string operatorPerson = TempData["operatorPerson"] as String;
            string feedbackString = TempData["feedbackString"] as String;


            return operatorPerson + ": You have clicked submit. <br>" +  
                  feedbackString;
        }


    }
}