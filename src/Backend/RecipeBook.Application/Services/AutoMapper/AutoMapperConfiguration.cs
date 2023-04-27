using AutoMapper;
using RecipeBook.Comunication.Payloads;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Application.Services.AutoMapper;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<SignUpUserRequestDto, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }
}