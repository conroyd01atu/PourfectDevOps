using Microsoft.VisualStudio.TestTools.UnitTesting;
using PourfectApp.Core.Models;
using System;
using System.Collections.Generic;

namespace PourfectApp.Tests.Models
{
    [TestClass]
    public class BrewTests
    {
        [TestMethod]
        public void Ratio_ValidWeights_ShouldCalculateCorrectly()
        {
            // Arrange
            var brew = new Brew
            {
                CoffeeWeight = 15,
                WaterWeight = 250
            };

            // Act
            string ratio = brew.Ratio;

            // Assert
            Assert.AreEqual("1:16.7", ratio, "Ratio should be calculated correctly");
        }

        [TestMethod]
        public void Ratio_ZeroCoffeeWeight_ShouldReturnNA()
        {
            // Arrange
            var brew = new Brew
            {
                CoffeeWeight = 0,
                WaterWeight = 250
            };

            // Act
            string ratio = brew.Ratio;

            // Assert
            Assert.AreEqual("N/A", ratio, "Ratio should be N/A when coffee weight is 0");
        }

        [TestMethod]
        public void Brew_DefaultValues_ShouldBeInitialized()
        {
            // Arrange & Act
            var brew = new Brew();

            // Assert
            Assert.AreEqual(0, brew.Id);
            Assert.IsNull(brew.CoffeeName);
            Assert.AreEqual(0, brew.CoffeeWeight);
            Assert.AreEqual(0, brew.WaterWeight);
            Assert.AreEqual(0, brew.Rating);
        }

        [TestMethod]
        public void Brew_SetProperties_ShouldRetainValues()
        {
            // Arrange
            var brewDate = DateTime.Now;
            var roastDate = DateTime.Now.AddDays(-7);

            // Act
            var brew = new Brew
            {
                CoffeeName = "Ethiopian Yirgacheffe",
                Roaster = "Local Roaster",
                BrewDate = brewDate,
                RoastDate = roastDate,
                CoffeeWeight = 18,
                WaterWeight = 300,
                GrindSize = "Medium",
                WaterTemperature = 93,
                BrewTime = "3:00",
                Dripper = "V60",
                Notes = "Floral and bright",
                Rating = 4.5,
                Username = "testuser"
            };

            // Assert
            Assert.AreEqual("Ethiopian Yirgacheffe", brew.CoffeeName);
            Assert.AreEqual("Local Roaster", brew.Roaster);
            Assert.AreEqual(brewDate, brew.BrewDate);
            Assert.AreEqual(roastDate, brew.RoastDate);
            Assert.AreEqual(18, brew.CoffeeWeight);
            Assert.AreEqual(300, brew.WaterWeight);
            Assert.AreEqual("Medium", brew.GrindSize);
            Assert.AreEqual(93, brew.WaterTemperature);
            Assert.AreEqual("3:00", brew.BrewTime);
            Assert.AreEqual("V60", brew.Dripper);
            Assert.AreEqual("Floral and bright", brew.Notes);
            Assert.AreEqual(4.5, brew.Rating);
            Assert.AreEqual("testuser", brew.Username);
        }
    }
}
