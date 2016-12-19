﻿using NUnit.Framework;
using Phaxio.Tests.Helpers;
using System.Linq;
using System.Threading;

namespace Phaxio.Tests.IntegrationTests
{
    [TestFixture, Explicit]
    public class NumbersTests
    {
        [Test]
        public void IntegrationTests_Numbers_GetAreaCodes()
        {
            var config = new KeyManager();

            var phaxio = new PhaxioClient(config["api_key"], config["api_secret"]);

            var areaCodes = phaxio.ListAreaCodes(state:"HI");

            Assert.Greater(areaCodes.Count(), 0, "There should be some area codes");
        }

        [Test]
        public void IntegrationTests_Numbers_BasicScenario()
        {
            var config = new KeyManager();

            var phaxio = new PhaxioClient(config["api_key"], config["api_secret"]);

            // Find area codes to provision a number in
            var areaCodes = phaxio.ListAreaCodes(state: "DE");

            Assert.Greater(areaCodes.Count(), 0, "There should be some area codes");

            var areaCode = areaCodes.First();

            // Provision a number
            var provisionedNumber = phaxio.ProvisionNumber(areaCode.Key);

            // Check to see if the number's listed on the account
            var accountNumbers = phaxio.ListNumbers();

            if (!accountNumbers.Any(n => n.Number == provisionedNumber.Number))
            {
                throw new AssertionException("ListNumbers should return newly provisioned number.");
            }

            Thread.Sleep(1000);

            // Release the number
            phaxio.ReleaseNumber(provisionedNumber.Number);

            Thread.Sleep(1000);

            // Check to see if the number's still listed on the account
            accountNumbers = phaxio.ListNumbers();

            if (accountNumbers.Any(n => n.Number == provisionedNumber.Number))
            {
                throw new AssertionException("ListNumbers should not return released number.");
            }
        }
    }
}
