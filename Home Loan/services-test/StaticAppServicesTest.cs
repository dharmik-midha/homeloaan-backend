using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using DataAccess.Contracts;
using DataAccess.DBContext;
using DataAccess.Entities;
using Logging.NLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    [TestFixture]
    public class StaticAppServiceTests
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger _logger;
        private IStaticAppService _appService;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<IRepositoryManager>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger>();
            _appService = new StaticAppService(_repository, _mapper, _logger);
        }

        //[Test]
        //public void AddCountry_AlreadyExists_ReturnsCountryId()
        //{
        //    // Arrange
        //    var countryModel = new CountryDTO
        //    {
        //        CountryCode = "US",
        //        CountryName = "United States"
        //    };

        //    var existingCountry = new Country
        //    {
        //        CountryId = "1",
        //        CountryCode = "US",
        //        CountryName = "United States"
        //    };

        //    _repository.Country.FindByCondition(Arg.Any<Expression<Func<Country, bool>>>(), false).Returns((new List<Country> { existingCountry }).AsQueryable());

        //    // Act
        //    var result = _appService.AddCountry(countryModel);

        //    // Assert
        //    Assert.AreEqual(existingCountry.CountryId, result);
        //    _logger.Received(1).LogInfo(Arg.Any<string>());
        //    _repository.Country.DidNotReceive().Create(Arg.Any<Country>());
        //}

        //[Test]
        //public void AddState_AlreadyExists_ReturnsStateId()
        //{
        //    // Arrange
        //    var stateModel = new StateDTO
        //    {
        //        StateName = "Uttarakhand",
        //        StateCode = "UK",
        //    };

        //    var existingState = new State
        //    {
        //        StateId = "1",
        //        StateName = "Uttarakhand",
        //        StateCode = "UK",
        //    };

        //    _repository.State.FindByCondition(Arg.Any<Expression<Func<State, bool>>>(), false).Returns((new List<State> { existingState }).AsQueryable());

        //    // Act
        //    var result = _appService.AddState(stateModel);

        //    // Assert
        //    Assert.AreEqual(existingState.StateId, result);
        //    _logger.Received(1).LogInfo(Arg.Any<string>());
        //    _repository.State.DidNotReceive().Create(Arg.Any<State>());

        //}

        //[Test]
        //public void AddCity_AlreadyExists_ReturnsCityId()
        //{
        //    // Arrange
        //    var cityModel = new CityDTO
        //    {
        //        CityName = "Haridwar",
        //        CityCode = "HW",
        //    };

        //    var existingCity = new City
        //    {
        //        CityId = "1",
        //        CityName = "Haridwar",
        //        CityCode = "HW",
        //    };
        //    _repository.City.FindByCondition(Arg.Any<Expression<Func<City, bool>>>(), false).Returns((new List<City> { existingCity }).AsQueryable());

        //    // Act
        //    var result = _appService.AddCity(cityModel);

        //    // Assert
        //    Assert.AreEqual(existingCity.CityId, result);
        //    _repository.City.DidNotReceive().Create(Arg.Any<City>());

        //}


        //[Test]
        //public void AddLocation_ShouldReturnSuccess_WhenLocationDoesNotExist()
        //{
        //    // Arrange
        //    var locationDTO = new CountryStateCityDTO
        //    {
        //        CityName = "Haridwar",
        //        CityCode = "HW",
        //        CountryName = "India",
        //        CountryCode = "IN",
        //        StateName = "Uttarakhand",
        //        StateCode = "UK",
        //    };

        //    // Act
        //    var result = _appService.AddLocation(locationDTO);

        //    // Assert
        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(result.StatusCode, 200);
        //    Assert.AreEqual(result.Message, "");
        //}
    }
}