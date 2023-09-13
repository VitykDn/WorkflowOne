using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using NUnit.Framework;
using FakeXrmEasy;
using WorkflowOne;

namespace TestWorkflowOne
{
    [TestFixture]
    public class ContactSearchTests
    {
        [Test]
        public void OneContactReturned_StatusCodeShouldBe1()
        {
            var context = new XrmFakedContext();
            var inputs = CreateInputParameters();

            var contact = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["emailaddress1"] = "test@test.com",
                ["firstname"] = "Test"
            };

            var orgService = context.GetOrganizationService();

            context.Initialize(contact);

            var outputParms = context.ExecuteCodeActivity<ContactSearch>(inputs);

            Assert.AreEqual(1, outputParms["Status"]);
            Assert.AreEqual(contact.Id.ToString(), outputParms["ContactLink"]);
        }
        [Test]
        public void NoContactReturned_StatusCodeShouldBe2()
        {
            var context = new XrmFakedContext();
            var inputs = CreateInputParameters();


            var outputParms = context.ExecuteCodeActivity<ContactSearch>(inputs);

            Assert.AreEqual(2, outputParms["Status"]);
        }
        [Test]
        public void MoreThanOneIdenticalContactReturned_StatusCodeShouldBe3()
        {
            var context = new XrmFakedContext();
            var inputs = CreateInputParameters();

            var contact1 = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["emailaddress1"] = "test@test.com",
                ["firstname"] = "Test"
            };

            var contact2 = new Entity("contact")
            {
                Id = Guid.NewGuid(),
                ["emailaddress1"] = "test@test.com",
                ["firstname"] = "Test"
            };

            context.Initialize(new List<Entity> { contact1, contact2 });

            var outputParms = context.ExecuteCodeActivity<ContactSearch>(inputs);

            Assert.AreEqual(3, outputParms["Status"]);
        }


        private Dictionary<string, object> CreateInputParameters()
        {
            return new Dictionary<string, object>
             {
                 { "Parameter1", "Test" },
                 { "FieldName1", "firstname" },
                 { "Parameter2", "test@test.com" },
                 { "FieldName2", "emailaddress1" }
             };
        }


    }
}
