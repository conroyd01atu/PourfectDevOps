using Microsoft.VisualStudio.TestTools.UnitTesting;
using PourfectApp.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace PourfectApp.Tests.Models
{
    [TestClass]
    public class RecipeTests
    {
        [TestMethod]
        public void FavoriteIcon_WhenFavorite_ShouldReturnStar()
        {
            // Arrange
            var recipe = new Recipe { IsFavorite = true };

            // Act
            string icon = recipe.FavoriteIcon;

            // Assert
            Assert.AreEqual("⭐", icon);
        }

        [TestMethod]
        public void FavoriteIcon_WhenNotFavorite_ShouldReturnEmptyStar()
        {
            // Arrange
            var recipe = new Recipe { IsFavorite = false };

            // Act
            string icon = recipe.FavoriteIcon;

            // Assert
            Assert.AreEqual("☆", icon);
        }

        [TestMethod]
        public void Steps_JsonSerialization_ShouldWorkCorrectly()
        {
            // Arrange
            var steps = new List<RecipeStep>
            {
                new RecipeStep
                {
                    StepNumber = 1,
                    TimeRange = "0:00-0:30",
                    Description = "Bloom",
                    WaterAmount = 30
                },
                new RecipeStep
                {
                    StepNumber = 2,
                    TimeRange = "0:30-1:00",
                    Description = "First pour",
                    WaterAmount = 70
                }
            };

            var recipe = new Recipe();

            // Act
            recipe.Steps = steps;
            var retrievedSteps = recipe.Steps;

            // Assert
            Assert.AreEqual(2, retrievedSteps.Count);
            Assert.AreEqual("Bloom", retrievedSteps[0].Description);
            Assert.AreEqual(70, retrievedSteps[1].WaterAmount);
        }

        [TestMethod]
        public void Parameters_JsonSerialization_ShouldWorkCorrectly()
        {
            // Arrange
            var parameters = new RecipeParameters
            {
                BloomTime = "0:30",
                BloomWater = 30,
                PourTechnique = "Continuous",
                NumberOfPours = 3,
                FilterType = "V60 tabbed",
                Notes = "Keep water temperature stable"
            };

            var recipe = new Recipe();

            // Act
            recipe.Parameters = parameters;
            var retrievedParams = recipe.Parameters;

            // Assert
            Assert.AreEqual("0:30", retrievedParams.BloomTime);
            Assert.AreEqual(30, retrievedParams.BloomWater);
            Assert.AreEqual("Continuous", retrievedParams.PourTechnique);
            Assert.AreEqual(3, retrievedParams.NumberOfPours);
            Assert.AreEqual("V60 tabbed", retrievedParams.FilterType);
            Assert.AreEqual("Keep water temperature stable", retrievedParams.Notes);
        }

        [TestMethod]
        public void Ratio_ShouldCalculateCorrectly()
        {
            // Arrange
            var recipe = new Recipe
            {
                CoffeeWeight = 20,
                WaterWeight = 340
            };

            // Act
            string ratio = recipe.Ratio;

            // Assert
            Assert.AreEqual("1:17.0", ratio);
        }
    }
}