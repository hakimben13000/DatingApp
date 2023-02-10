namespace DatingAppTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Theory]
        [InlineData("hakim")]
        [InlineData("bendi")]
        public void Test2(string p)
        {
             Assert.NotEqual(0, p.Length);
        }
    }
}