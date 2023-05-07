using AutoMapper;
using RecipeBook.Application.Services.AutoMapper;

namespace TestHelpers.Mapper;

public class MapperBuilder
{
    public static IMapper Instance()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperConfiguration>();
        });

        return mapperConfig.CreateMapper();
    }
}