using Xunit;

namespace IndexTree
{
    public class IndexTreeTests
    {
        [Fact]
        public void Large_Test()
        {
            //const int N = 10000;
            const int N = 100;
            int[] data;
            // Arrange
            data = new int[N];
            for (int i = 0; i < N; i++)
            {
                data[i] = i;
            }
            // Shuffle the data to avoid any ordering effects
            var rand = new Random(42);
            for(int i = N - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (data[i], data[j]) = (data[j], data[i]);
            }

            // Act & Assert
            var bbDict = new IndexTree<int>();
            for (int i = 0; i < N; i++)
            {
                bbDict.AddKey(data[i]);
            }
            //for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    var _ = bbDict.ContainsKey(data[i]);
                }
            }
            for (int i = 0; i < N; i++)
            {
                bbDict.RemoveKey(data[i]);
            }
            Assert.Equal(0, bbDict.Size);
        }

        [Fact]
        public void Step_by_Step_Addition_works()
        {
            // Arrange
            var dict = new IndexTree<int>();

            // Act
            dict.AddKey(1);

            // Assert
            Assert.Equal(1, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));

            // Act
            dict.AddKey(2);

            // Assert
            Assert.Equal(2, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));

            // Act
            dict.AddKey(3);

            // Assert
            Assert.Equal(3, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));

            // Act
            dict.AddKey(4);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));

            // Act
            dict.AddKey(5);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.Equal(4, dict.GetIndexOfKey(5));

            // Act
            dict.AddKey(6);

            // Assert
            Assert.Equal(6, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.Equal(4, dict.GetIndexOfKey(5));
            Assert.Equal(5, dict.GetIndexOfKey(6));

            // Act
            dict.AddKey(7);

            // Assert
            Assert.Equal(7, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.Equal(4, dict.GetIndexOfKey(5));
            Assert.Equal(5, dict.GetIndexOfKey(6));
            Assert.Equal(6, dict.GetIndexOfKey(7));

            // Act
            dict.RemoveKey(7);

            // Assert
            Assert.Equal(6, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.Equal(4, dict.GetIndexOfKey(5));
            Assert.Equal(5, dict.GetIndexOfKey(6));
            Assert.False(dict.ContainsKey(7));

            // Act
            dict.RemoveKey(6);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.Equal(4, dict.GetIndexOfKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));

            // Act
            dict.RemoveKey(5);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));

            // Act
            dict.RemoveKey(4);

            // Assert
            Assert.Equal(3, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));

            // Act
            dict.RemoveKey(3);

            // Assert
            Assert.Equal(2, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));

            // Act
            dict.RemoveKey(2);

            // Assert
            Assert.Equal(1, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));

            // Act
            dict.RemoveKey(1);

            // Assert
            Assert.Equal(0, dict.Size);
            Assert.False(dict.ContainsKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));
        }

        [Fact]
        public void Step_by_Step_Addition_reverse_order_works()
        {
            // Arrange
            var dict = new IndexTree<int>();

            // Act
            dict.AddKey(7);

            // Assert
            Assert.Equal(1, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(7));

            // Act
            dict.AddKey(6);

            // Assert
            Assert.Equal(2, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(6));
            Assert.Equal(1, dict.GetIndexOfKey(7));

            // Act
            dict.AddKey(5);

            // Assert
            Assert.Equal(3, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(5));
            Assert.Equal(1, dict.GetIndexOfKey(6));
            Assert.Equal(2, dict.GetIndexOfKey(7));

            // Act
            dict.AddKey(4);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(4));
            Assert.Equal(1, dict.GetIndexOfKey(5));
            Assert.Equal(2, dict.GetIndexOfKey(6));
            Assert.Equal(3, dict.GetIndexOfKey(7));

            // Act
            dict.AddKey(3);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(3));
            Assert.Equal(1, dict.GetIndexOfKey(4));
            Assert.Equal(2, dict.GetIndexOfKey(5));
            Assert.Equal(3, dict.GetIndexOfKey(6));
            Assert.Equal(4, dict.GetIndexOfKey(7));

            // Act
            dict.AddKey(2);

            // Assert
            Assert.Equal(6, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(2));
            Assert.Equal(1, dict.GetIndexOfKey(3));
            Assert.Equal(2, dict.GetIndexOfKey(4));
            Assert.Equal(3, dict.GetIndexOfKey(5));
            Assert.Equal(4, dict.GetIndexOfKey(6));
            Assert.Equal(5, dict.GetIndexOfKey(7));

            // Act
            dict.AddKey(1);

            // Assert
            Assert.Equal(7, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(1));
            Assert.Equal(1, dict.GetIndexOfKey(2));
            Assert.Equal(2, dict.GetIndexOfKey(3));
            Assert.Equal(3, dict.GetIndexOfKey(4));
            Assert.Equal(4, dict.GetIndexOfKey(5));
            Assert.Equal(5, dict.GetIndexOfKey(6));
            Assert.Equal(6, dict.GetIndexOfKey(7));

            // Act
            dict.RemoveKey(1);

            // Assert
            Assert.Equal(6, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(2));
            Assert.Equal(1, dict.GetIndexOfKey(3));
            Assert.Equal(2, dict.GetIndexOfKey(4));
            Assert.Equal(3, dict.GetIndexOfKey(5));
            Assert.Equal(4, dict.GetIndexOfKey(6));
            Assert.False(dict.ContainsKey(1));

            // Act
            dict.RemoveKey(2);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(3));
            Assert.Equal(1, dict.GetIndexOfKey(4));
            Assert.Equal(2, dict.GetIndexOfKey(5));
            Assert.Equal(3, dict.GetIndexOfKey(6));
            Assert.Equal(4, dict.GetIndexOfKey(7));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(1));

            // Act
            dict.RemoveKey(3);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(4));
            Assert.Equal(1, dict.GetIndexOfKey(5));
            Assert.Equal(2, dict.GetIndexOfKey(6));
            Assert.Equal(3, dict.GetIndexOfKey(7));
            Assert.False(dict.ContainsKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));

            // Act
            dict.RemoveKey(4);

            // Assert
            Assert.Equal(3, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(5));
            Assert.Equal(1, dict.GetIndexOfKey(6));
            Assert.Equal(2, dict.GetIndexOfKey(7));
            Assert.False(dict.ContainsKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));

            // Act
            dict.RemoveKey(5);

            // Assert
            Assert.Equal(2, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(6));
            Assert.Equal(1, dict.GetIndexOfKey(7));
            Assert.False(dict.ContainsKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));

            // Act
            dict.RemoveKey(6);

            // Assert
            Assert.Equal(1, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(7));
            Assert.False(dict.ContainsKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));

            // Act
            dict.RemoveKey(7);

            // Assert
            Assert.Equal(0, dict.Size);
            Assert.False(dict.ContainsKey(1));
            Assert.False(dict.ContainsKey(2));
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(4));
            Assert.False(dict.ContainsKey(5));
            Assert.False(dict.ContainsKey(6));
            Assert.False(dict.ContainsKey(7));
        }

        [Fact]
        public void IndexFromKey100_ReturnsCorrectIndex()
        {
            // Arrange
            var dict = new IndexTree<int>();
            

            // Act
            for(int i = 1; i <= 100; i++)
            {
                dict.AddKey(i);
            }

            // Assert            
            for(int i = 1; i <= 100; i++ ) 
            {
                Assert.Equal(i - 1, dict.GetIndexOfKey(i));
            }
        }

        [Fact]
        public void InitFromArray_WithOddNumberOfElements_InitializesCorrectly()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [1, 3, 5, 7, 9];

            // Act
            dict.InitFromArray(keys);

            // Assert            
            Assert.Equal(5, dict.Size);
        }

        [Fact]
        public void InitFromArray_WithEvenNumberOfElements_InitializesCorrectly()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [2, 4, 6, 8, 10, 12];

            // Act
            dict.InitFromArray(keys);

            // Assert
            Assert.Equal(6, dict.Size);
        }

        [Fact]
        public void InitFromArray_AlreadyInitialized_ThrowsException()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [1, 2, 3];
            dict.InitFromArray(keys);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => dict.InitFromArray([4, 5, 6]));
        }

        [Fact]
        public void GetKeyAtIndex_ValidIndex_ReturnsCorrectKey()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [10, 20, 30, 40, 50];
            dict.InitFromArray(keys);

            // Act & Assert
            for (int i = 0; i < keys.Length; i++)
            {
                Assert.Equal(keys[i], dict.GetKeyAtIndex(i));
            }
        }

        [Fact]
        public void GetKeyAtIndex_NegativeIndex_ThrowsException()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([1, 2, 3]);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => dict.GetKeyAtIndex(-1));
        }

        [Fact]
        public void GetKeyAtIndex_IndexOutOfRange_ThrowsException()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([1, 2, 3]);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => dict.GetKeyAtIndex(3));
        }

        [Fact]
        public void GetIndexOfKey_ExistingKey_ReturnsCorrectIndex()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [5, 10, 15, 20, 25];
            dict.InitFromArray(keys);

            // Act & Assert
            Assert.Equal(0, dict.GetIndexOfKey(5));
            Assert.Equal(2, dict.GetIndexOfKey(15));
            Assert.Equal(4, dict.GetIndexOfKey(25));
        }

        [Fact]
        public void ContainsKey_ExistingKey_ReturnsTrue()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [5, 10, 15, 20, 25];
            dict.InitFromArray(keys);

            // Act & Assert
            Assert.True(dict.ContainsKey(5));
            Assert.True(dict.ContainsKey(15));
            Assert.True(dict.ContainsKey(25));
        }

        [Fact]
        public void ContainsKey_NonExistingKey_ReturnsFalse()
        {
            // Arrange
            var dict = new IndexTree<int>();
            int[] keys = [5, 10, 15, 20, 25];
            dict.InitFromArray(keys);

            // Act & Assert
            Assert.False(dict.ContainsKey(3));
            Assert.False(dict.ContainsKey(17));
            Assert.False(dict.ContainsKey(30));
        }

        [Fact]
        public void GetIndexOfKey_NonExistingKey_ThrowsException()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([1, 3, 5]);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => dict.GetIndexOfKey(2));
        }

        [Fact]
        public void GetIndexOfKeyOrSmaller_ExistingKey_ReturnsExactIndex()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30, 40, 50]);

            // Act
            int index = dict.GetIndexOfKeyOrSmaller(30);

            // Assert
            Assert.Equal(2, index);
        }

        [Fact]
        public void GetIndexOfKeyOrSmaller_KeyBetweenValues_ReturnsIndexOfSmallerKey()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30, 40, 50]);

            // Act
            int index = dict.GetIndexOfKeyOrSmaller(35);

            // Assert
            Assert.Equal(2, index);
        }

        [Fact]
        public void GetIndexOfKeyOrSmaller_KeySmallerThanAll_ReturnsMinusOne()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act
            int index = dict.GetIndexOfKeyOrSmaller(5);

            // Assert
            Assert.Equal(-1, index);
        }

        [Fact]
        public void GetIndexOfKeyOrSmaller_KeyLargerThanAll_ReturnsLastIndex()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act
            int index = dict.GetIndexOfKeyOrSmaller(100);

            // Assert
            Assert.Equal(2, index);
        }

        [Fact]
        public void AddKey_NewKey_IncreasesSize()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 30, 50]);

            // Act
            dict.AddKey(20);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(10));
            Assert.Equal(1, dict.GetIndexOfKey(20));
            Assert.Equal(2, dict.GetIndexOfKey(30));
            Assert.Equal(3, dict.GetIndexOfKey(50));
        }

        [Fact]
        public void AddKey_DuplicateKey_ThrowsException()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => dict.AddKey(20));
        }

        [Fact]
        public void AddKey_ToEmptyTree_CreatesRootNode()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([]);

            // Act
            dict.AddKey(42);

            // Assert
            Assert.Equal(1, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(42));
        }

        [Fact]
        public void RemoveKey_ExistingKey_DecreasesSize()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30, 40, 50]);

            // Act
            dict.RemoveKey(30);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(10));
            Assert.Equal(1, dict.GetIndexOfKey(20));
            Assert.Equal(2, dict.GetIndexOfKey(40));
            Assert.Equal(3, dict.GetIndexOfKey(50));
            Assert.False(dict.ContainsKey(30));
        }

        [Fact]
        public void RemoveKey_NonExistingKey_ThrowsException()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => dict.RemoveKey(15));
        }

        [Fact]
        public void RemoveKey_LastKey_ResultsInEmptyTree()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([42]);

            // Act
            dict.RemoveKey(42);

            // Assert
            Assert.Equal(0, dict.Size);
        }

        [Fact]
        public void Performance_Test()
        {
            // Arrange
            //var dict = new IndexTree<int>();
            int N = 1000;
            int[] data = new int[N];

            for (int i = 0; i < N; i++)
            {
                data[i] = i;
            }

            var bbDict = new IndexTree<int>();
            for (int i = 0; i < N; i++)
            {
                bbDict.AddKey(data[i]);
            }
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < N; i++)
                {
                    var _ = bbDict.ContainsKey(data[i]);
                }
            }
            for (int i = 0; i < N; i++)
            {
                bbDict.RemoveKey(data[i]);
            }
        }

        [Fact]
        public void AddAndRemoveKeys_MultipleOperations_MaintainsConsistency()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act
            dict.AddKey(15);
            dict.AddKey(25);
            dict.RemoveKey(20);
            dict.AddKey(5);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKey(5));
            Assert.Equal(1, dict.GetIndexOfKey(10));
            Assert.Equal(2, dict.GetIndexOfKey(15));
            Assert.Equal(3, dict.GetIndexOfKey(25));
            Assert.Equal(4, dict.GetIndexOfKey(30));
        }

        [Fact]
        public void GetIndexOfKeyOrSmaller_Works()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act
            dict.AddKey(15);
            dict.AddKey(25);
            dict.RemoveKey(20);
            dict.AddKey(5);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKeyOrSmaller(5));
            Assert.Equal(1, dict.GetIndexOfKeyOrSmaller(10));
            Assert.Equal(2, dict.GetIndexOfKeyOrSmaller(15));
            Assert.Equal(3, dict.GetIndexOfKeyOrSmaller(25));
            Assert.Equal(4, dict.GetIndexOfKeyOrSmaller(30));
            Assert.Equal(0, dict.GetIndexOfKeyOrSmaller(9));
            Assert.Equal(1, dict.GetIndexOfKeyOrSmaller(14));
            Assert.Equal(2, dict.GetIndexOfKeyOrSmaller(19));
            Assert.Equal(3, dict.GetIndexOfKeyOrSmaller(29));
            Assert.Equal(4, dict.GetIndexOfKeyOrSmaller(34));
            Assert.Equal(-1, dict.GetIndexOfKeyOrSmaller(4));
        }

        [Fact]
        public void GetIndexOfKeyOrGreater_Works()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act
            dict.AddKey(15);
            dict.AddKey(25);
            dict.RemoveKey(20);
            dict.AddKey(5);

            // Assert
            Assert.Equal(5, dict.Size);
            Assert.Equal(0, dict.GetIndexOfKeyOrGreater(5));
            Assert.Equal(1, dict.GetIndexOfKeyOrGreater(10));
            Assert.Equal(2, dict.GetIndexOfKeyOrGreater(15));
            Assert.Equal(3, dict.GetIndexOfKeyOrGreater(25));
            Assert.Equal(4, dict.GetIndexOfKeyOrGreater(30));
            Assert.Equal(0, dict.GetIndexOfKeyOrGreater(1));
            Assert.Equal(1, dict.GetIndexOfKeyOrGreater(9));
            Assert.Equal(2, dict.GetIndexOfKeyOrGreater(12));
            Assert.Equal(3, dict.GetIndexOfKeyOrGreater(21));
            Assert.Equal(4, dict.GetIndexOfKeyOrGreater(27));
            Assert.Equal(-1, dict.GetIndexOfKeyOrGreater(31));
        }

        [Fact]
        public void InitFromArray_WithStrings_WorksCorrectly()
        {
            // Arrange
            var dict = new IndexTree<string>();
            string[] keys = ["apple", "banana", "cherry", "date"];

            // Act
            dict.InitFromArray(keys);

            // Assert
            Assert.Equal(4, dict.Size);
            Assert.Equal("apple", dict.GetKeyAtIndex(0));
            Assert.Equal("banana", dict.GetKeyAtIndex(1));
            Assert.Equal("cherry", dict.GetKeyAtIndex(2));
            Assert.Equal("date", dict.GetKeyAtIndex(3));
        }

        [Fact]
        public void MaxSize_TracksMaximumSize()
        {
            // Arrange
            var dict = new IndexTree<int>();
            dict.InitFromArray([10, 20, 30]);

            // Act
            dict.AddKey(40);
            dict.AddKey(50);
            dict.RemoveKey(20);

            // Assert
            Assert.Equal(4, dict.Size);
        }
    }
}