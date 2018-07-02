namespace game
{
    public class CtorTest
    {
        public CtorTest(CtorTest other)
        {
            this.A = other.A;
            this.B = other.B;
        }

        public CtorTest()
        {
        }

        public int A { get; set; } = 100;
        public int B { get; set; } = 200;
    }
}