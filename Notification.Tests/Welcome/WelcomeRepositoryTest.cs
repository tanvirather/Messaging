using Zuhid.Notification.Entities;
using Zuhid.Notification.Welcome;

namespace Zuhid.Notification.Tests.Welcome
{
    public class WelcomeRepositoryTest
    {
        private readonly WelcomeRepository repository;

        public WelcomeRepositoryTest()
        {
            var dataLoader = new DataLoader();
            var customers = DataLoader.LoadCsv<CustomerEntity>("CustomerEntity.csv");
            dataLoader.SetupDbSet(customers, c => c.Customer);
            repository = new WelcomeRepository(dataLoader.Context);
        }

        [Fact]
        public async Task Get_WhenUserExists_ShouldReturnWelcomeModel()
        {
            // Arrange
            // Act
            var result = await repository.Get(new Guid("3a4b5c6d-7e8f-9a0b-1c2d-3e4f5a6b7c8d"));
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Jane", result.Customer?.FirstName);
            Assert.Equal("Smith", result.Customer?.LastName);
            Assert.Equal("jane.smith@example.com", result.Customer?.Email);
        }

        [Fact]
        public async Task Get_WhenUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            // Act
            var result = await repository.Get(new Guid());
            // Assert
            Assert.Null(result);
        }
    }
}
