using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EquitiesIntegration.Controllers;
using System.Collections.Generic;

namespace EquitiesIntegration.Test
{
    [TestClass]
    public class IDNumberTest
    {
        ServicesController servicesController = new ServicesController();


        [TestMethod]
        public void ShouldFailLengthTooShort()
        {
            var idNum = "8501011234";
            var dob = new DateTime(1985, 1,1);
            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);

        }

        [TestMethod]
        public void ShouldFailLengthTooLong()
        {
            var idNum = "8501011232655922254";
            var dob = new DateTime(1985, 1, 1);           

            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);
        }

        [TestMethod]
        public void ShouldFailFirstSixDigitsDontMatchDOB()
        {
            var idNum = "8501011234567";
            var dob = new DateTime(1986, 12, 12);           

            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);
        }


        [TestMethod]
        public void ShouldFailRegexMismatch()
        {
            var idNum = "8585850101234";
            var dob = new DateTime(1985, 12, 12);            

            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);
        }


        [TestMethod]
        public void ShouldFailGenderFlagIncorrectMale()
        {
            var idNum = "8501013088188";
            var dob = new DateTime(1985, 1, 1);
            
            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);
        }


        [TestMethod]
        public void ShouldFailGenderFlagIncorrectFemale()
        {
            var idNum = "8501015088188";
            var dob = new DateTime(1985, 1, 1);
            
            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);
        }

        [TestMethod]
        public void ShouldFailNationalityFlagIncorrect()
        {
            var idNum = "8501016088188";
            var dob = new DateTime(1985, 1, 1);
            
            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.InvalidID);
        }


        [TestMethod]
        public void ShouldPass()
        {
            var idNum = "8501014000088";
            var dob = new DateTime(1985, 1, 1);            

            string citizenship = "South African";
            string gender = "Female";
            var scr = new SubmitCustomerResponse() { Code = (int)IDValidationCodes.NotCompleted, Messages = new List<string>() };

            var outcome = servicesController.ValidateIDNumber(idNum, ref scr, dob.ToString(), citizenship, gender);

            Assert.AreEqual(scr.Code, (int)IDValidationCodes.Success);
        }
    }
}
