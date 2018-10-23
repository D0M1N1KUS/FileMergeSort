using NUnit.Framework;
using SequentialFileIO;

namespace SequentialFileIO_Test
{
    [TestFixture]
    public class FibonacciSequenceGeneratorTest
    {
        [Test]
        public void getFirstTenFibonacciNumbersWithGetNext()
        {
            var expectedNumbers = new int[] {1, 1, 2, 3, 5, 8, 13, 21, 34, 55};
            var actualNumbers1 = new int[10];
            var generator = new FibonacciSequenceGenerator();

            for (var i = 0; i < 10; i++)
                actualNumbers1[i] = generator.GetNext();
           
            CollectionAssert.AreEqual(expectedNumbers, actualNumbers1);
        }
        
        [Test]
        public void getFirstTenFibonacciNumbersWithGetRangeFromTo()
        {
            var expectedNumbers = new int[] {1, 1, 2, 3, 5, 8, 13, 21, 34, 55};
            var generator = new FibonacciSequenceGenerator();

            var actualNumbers = generator.GetRange(1, 55);
            
            CollectionAssert.AreEqual(expectedNumbers, actualNumbers);
        }
        
        [Test]
        public void getFirstTenFibonacciNumbersWithGetRangeN()
        {
            var expectedNumbers = new int[] {1, 1, 2, 3, 5, 8, 13, 21, 34, 55};
            var generator = new FibonacciSequenceGenerator();

            var actualNumbers = generator.GetRange(55);
            
            CollectionAssert.AreEqual(expectedNumbers, actualNumbers);
        }

        [Test]
        public void getRangeOfN_FibonacciNumbers()
        {
            var expectedNumbers = new int[] {5, 8, 13, 21, 34};

            var actualNumbers = new FibonacciSequenceGenerator().GetRangeOfN(5, 5);
            
            CollectionAssert.AreEqual(expectedNumbers, actualNumbers);
        }
        
        [Test]
        public void getRangeOfN_FibonacciNumbers_StartingFromZero()
        {
            var expectedNumbers = new int[] {1, 1, 2, 3, 5};

            var actualNumbers = new FibonacciSequenceGenerator().GetRangeOfN(1, 5);
            
            CollectionAssert.AreEqual(expectedNumbers, actualNumbers);
        }

        [Test]
        public void getNext_N_Numbers()
        {
            var expectedNumbers = new int[] {5, 8};
            var generator = new FibonacciSequenceGenerator();

            for (var i = 0; i < 4; i++)
            {
                generator.GetNext();
            }

            var actualNumbers = generator.GetNext(2);
            
            CollectionAssert.AreEqual(expectedNumbers, actualNumbers);

        }
    }
}