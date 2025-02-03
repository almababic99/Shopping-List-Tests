using Infrastructure.Mappers;
using Tests.Builders;
using Xunit;
using FluentAssertions;

namespace Tests.InfrastructureTests
{
    public class ItemMapperDomainToEntityTests
    {
        [Fact]
        public void Given_Item_When_MapToEntityIsCalled_Then_ReturnItemEntity()
        {
            // Given
            var item = ItemBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();
            var expectedItemEntity = ItemEntityBuilder.WithDefaults().WithId(1).WithName("Milk").WithQuantity(1).Build();

            // When 
            var result = ItemMapperDomainToEntity.MapToEntity(item);

            // Then
            result.Should().BeEquivalentTo(expectedItemEntity);
        }
    }
}
