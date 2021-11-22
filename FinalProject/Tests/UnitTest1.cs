using FinalProject;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Arrange
            var scores = new[] { 1, 2, 3, 4, 5};
            var expectedSum = 15;

            //Act
            var actualSum = new ScoreCalculator().AddScores(scores);

            //Assert
            Assert.AreEqual(expectedSum, actualSum);
        }
    }
}