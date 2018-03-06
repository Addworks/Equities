using EquitiesIntegration.Models;
using EquitiesIntegration.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace EquitiesIntegration.Controllers
{
    //[Authorize]
   
    public class ServicesController : Controller
    {
        private EquitiesIntegrationContext db = new EquitiesIntegrationContext();

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        
        [AllowCrossSiteJson]
        public JsonResult GetCitizenships()
        {
            return Json(db.Citizenships.ToList(), JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        public JsonResult GetCustomers()
        {
            var customers = db.Customers.ToList()
                .Select(n => new 
                {
                    n.Citizenship,
                    n.CustomerId,
                    DateOfBirth = n.DateOfBirth.ToShortDateString(),
                    Gender = Enum.GetName(typeof(Gender), n.Gender),
                    n.IDNumber
                }
                ).ToList();
            return Json(customers, JsonRequestBehavior.AllowGet);
        }

        [AllowCrossSiteJson]
        // GET: api
        public JsonResult SubmitCustomer(string idNumber, string dob, string gender, string citizenship)
        {
            var response = new SubmitCustomerResponse();

            response.Code = (int)IDValidationCodes.NotCompleted;
            response.Messages = new List<string>();

            var customer = new Customer();

            try
            {
                if(ValidateGender(gender, ref response))
                {
                    if (ValidateCitizenship(citizenship, ref response))
                    {
                        if (ValidateDOB(dob, ref response))
                        {
                            if (ValidateIDNumber(idNumber, ref response, dob, citizenship, gender))
                            {
                                if (db.Customers.Any(c => c.IDNumber == idNumber))
                                {
                                    response.Code = (int)IDValidationCodes.AlreadyAdded;
                                    response.Messages.Add("Customer has already been added");
                                }
                                else
                                {
                                    customer.Citizenship = db.Citizenships.FirstOrDefault(c => c.Name == citizenship);
                                    customer.DateOfBirth = DateTime.Parse(dob);
                                    customer.IDNumber = idNumber;
                                    customer.Gender = (Gender)Enum.Parse(typeof(Gender), gender);

                                    db.Customers.Add(customer);
                                    db.SaveChanges();

                                    response.Code = (int)IDValidationCodes.Success;
                                    response.Messages.Add("Successfully submitted");
                                }
                            }
                        }
                    }
                }
              
            }
            catch(Exception e)
            {
                response.Code = (int)IDValidationCodes.ExceptionEncountered;
                response.Messages.Add(e.Message);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        private bool ValidateGender(string gender, ref SubmitCustomerResponse response)
        {
            var genderOk = new List<string>() { "Male", "Female", "Other" }.Contains(gender);

            if(genderOk)
            {
                response.Code = 58;
                return true;
            }
            else
            {
                return false;
                response.Code = (int)IDValidationCodes.InvalidGender;
                response.Messages.Add("Invalid gender entered - " + gender);
            }
        }
        
        private bool ValidateDOB(string dob, ref SubmitCustomerResponse response)
        {
            var res = new DateTime();
            var dobOk = DateTime.TryParse(dob, out res);

            if (dobOk)
            {
                return true;
            }
            else
            {
                response.Code = (int)IDValidationCodes.InvalidDOB;
                response.Messages.Add("Invalid DOB entered - " + dob);
                return false;
            }
        }

        private bool ValidateCitizenship(string citizenship, ref SubmitCustomerResponse response)
        {
            var cits =  db.Citizenships.Any(c => c.Name == citizenship);
            if (cits)
            {
                return true;
            }
            else
            {
                response.Code = (int)IDValidationCodes.InvalidCitizenship;
                response.Messages.Add("Invalid citizenship entered - " + citizenship);
                return false;
            }
        }

        public bool ValidateIDNumber(string idNumber, ref SubmitCustomerResponse response, string dob, string citizenship, string gender)
        {

            string IdExpression = @"(?<Year>[0-9][0-9])(?<Month>([0][1-9])|([1][0-2]))(?<Day>([0-2][0-9])|([3][0-1]))(?<Gender>[0-9])(?<Series>[0-9]{3})(?<Citizenship>[0-9])(?<Uniform>[0-9])(?<Control>[0-9])";
            var match = Regex.Match(idNumber, IdExpression);
            if(match.Success)
            {
                //check gender flag 
                if(gender.ToLower() == "female")
                {
                    if(int.Parse(idNumber.Substring(6, 4)) > 4999)
                    {
                        response.Code = (int)IDValidationCodes.InvalidID;
                        response.Messages.Add("Invalid ID Number entered (incorrect gender flag for female customer) - " + idNumber);
                        return false;
                    }
                }
                else if(gender.ToLower() == "male")
                {
                    if (int.Parse(idNumber.Substring(6, 4)) < 4999)
                    {
                        response.Code = (int)IDValidationCodes.InvalidID;
                        response.Messages.Add("Invalid ID Number entered (incorrect gender flag for male customer) - " + idNumber);
                        return false;
                    }
                }

                //check nationality flag
                if (citizenship.ToLower() == "south african")
                {
                    if (int.Parse(idNumber.Substring(10, 1)) != 0)
                    {
                        response.Code = (int)IDValidationCodes.InvalidID;
                        response.Messages.Add("Invalid ID Number entered (incorrect citizenship flag for South African citizen) - " + idNumber);
                        return false;
                    }
                }
                else
                {
                    if (int.Parse(idNumber.Substring(10, 1)) != 1)
                    {
                        response.Code = (int)IDValidationCodes.InvalidID;
                        response.Messages.Add("Invalid ID Number entered (incorrect citizenship flag for permanent resident) - " + idNumber);
                        return false;
                    }
                }

                var dateOfBirth = DateTime.Parse(dob);
                
                //check DOB match
                if (idNumber.Substring(0, 6) != (dateOfBirth.Year.ToString().Substring(2,2) + dateOfBirth.Month.ToString("D2") + dateOfBirth.Day.ToString("D2")))
                {
                    response.Code = (int)IDValidationCodes.InvalidID;
                    response.Messages.Add("Invalid ID Number entered (DOB does not match first six characters of ID Number) - " + idNumber);
                    return false;
                }

                if (idNumber.Length != 13)
                {
                    response.Code = (int)IDValidationCodes.InvalidID;
                    response.Messages.Add("Invalid ID Number entered (length of ID Number is incorrect) - " + idNumber);
                    return false;
                }

                response.Code = (int)IDValidationCodes.Success;
                return true;
            }
            else
            {
                response.Code = (int)IDValidationCodes.InvalidID;
                response.Messages.Add("Invalid ID Number entered - " + idNumber);
                return false;
            }
        }
    }
}