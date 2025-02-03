using Infrastructure.Mappers;
using Tests.Builders;
using Xunit;
using FluentAssertions;

namespace Tests.InfrastructureTests
{
    public class ItemMapperEntityToDomainTests
    {
        [Fact]
        public void Given_ItemEntity_When_MapToDomainIsCalled_Then_ReturnItem()
        {
            // Given
            var itemEntity = ItemEntityBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();
            var expectedItem = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();

            // When 
            var result = ItemMapperEntityToDomain.MapToDomain(itemEntity);

            // Then
            result.Should().BeEquivalentTo(expectedItem);
        }
    }
}
