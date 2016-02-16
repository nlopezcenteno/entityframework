namespace UnitTesting.Core
{
    internal class UniversityContextFactory : IUniversityContextFactory
    {
        private readonly string _nameOrConnectionString;

        public UniversityContextFactory(string nameOrConnectionString)
        {
            _nameOrConnectionString = nameOrConnectionString;
        }

        public IUniversityContext CreateContext()
        {
            return new UniversityContext(_nameOrConnectionString);
        }
    }
}
