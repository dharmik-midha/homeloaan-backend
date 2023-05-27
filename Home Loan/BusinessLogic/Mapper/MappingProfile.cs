using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Text;


namespace BusinessLogic.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() : base("MappingProfile")
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<LoanDTO, Loan>().ReverseMap();
            CreateMap<PromotionDTO, Promotion>().ReverseMap();
            CreateMap<CollateralDTO, Collateral>().ReverseMap();
            CreateMap<LoanStateDTO, LoanState>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<City, CityDTO>().ReverseMap();

        }
    }
}
