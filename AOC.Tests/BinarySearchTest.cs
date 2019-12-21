using NUnit.Framework;

namespace AOC.Tests
{
    public class BinarySearchTest
    {
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(1)]
        [TestCase(20)]
        [TestCase(2342)]
        [TestCase(2543252345)]
        [TestCase(5234535425234)]
        [TestCase(25262452435423)]
        public void BinarySearchMin(long value)
        {
            var result = BinarySearch.GetMin(i => i >= value);
            Assert.AreEqual(value, result);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(1)]
        [TestCase(20)]
        [TestCase(2342)]
        [TestCase(2543252345)]
        [TestCase(5234535425234)]
        [TestCase(25262452435423)]
        public void BinarySearchMax(long value)
        {
            var result = BinarySearch.GetMax(i => i <= value);
            Assert.AreEqual(value, result);
        }
    }
}