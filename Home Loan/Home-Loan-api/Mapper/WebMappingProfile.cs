using AutoMapper;
using BusinessLogic.DTO;
using DataAccess.Entities;
using Home_Loan_api.Controllers;
using Home_Loan_api.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Mapper
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile() : base("WebMappingProfile")
        {
            CreateMap<LoanDTO, NewLoanModal>().ReverseMap();
            CreateMap<RegisterModal, UserDTO>().ReverseMap();
            CreateMap<LoanStateChangeModal, LoanStateDTO>().ReverseMap();
            CreateMap<PromotionModal, PromotionDTO>().ReverseMap();
            CreateMap<LoanState, LoanStateDTO>().ReverseMap();
            CreateMap<AddAddressModal, CountryStateCityDTO>().ReverseMap();
            CreateMap<ModifyLoanModal, LoanDTO>().ReverseMap();
            CreateMap<NewCollateralModal, CollateralDTO>().ReverseMap();
            CreateMap<CountryModal, CountryDTO>().ReverseMap();
            CreateMap<StateModal, StateDTO>().ReverseMap();
            CreateMap<CityModal, CityDTO>().ReverseMap();

        }
    }
}
