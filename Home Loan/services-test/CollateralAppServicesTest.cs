using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using DataAccess.Contracts;
using DataAccess.Entities;
using Logging.NLog;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class CollateralAppServiceTest
    {
        private IRepositoryManager _repository;
        private IMapper _mapper;
        private ILogger _logger;
        private CollateralAppService _collateralAppServices;

        [SetUp]
        public void Setup()
        {
            _repository = Substitute.For<IRepositoryManager>();
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger>();
            _collateralAppServices = new CollateralAppService(_repository, _mapper, _logger);
        }

        [Test]
        public void AddCollateral_ShouldReturnSuccessResult()
        {
            // Arrange
            var collateralDto = new CollateralDTO();
            var collateral = new Collateral { Id = Guid.NewGuid().ToString() };
            _mapper.Map<CollateralDTO, Collateral>(collateralDto).Returns(collateral);

            // Act
            var result = _collateralAppServices.AddCollateral(collateralDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(201, result.StatusCode);
            Assert.AreEqual("Collateral added", result.Message);
            _repository.Collateral.Received().Create(collateral);
            _repository.Received().Save();
        }

        [Test]
        public void UpdateCollateral_WithExistingCollateral_ShouldReturnSuccessResult()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var collateralDto = new CollateralDTO { CollateralValue = 100, OwnShare = 1 };
            var existingCollateral = new Collateral { Id = id };
            _repository.Collateral.GetByKey(id).Returns(existingCollateral);

            // Act
            var result = _collateralAppServices.UpdateCollateral(id, collateralDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Updated", result.Message);
            Assert.AreEqual(collateralDto.CollateralValue, existingCollateral.CollateralValue);
            Assert.AreEqual(collateralDto.OwnShare, existingCollateral.OwnShare);
            _repository.Received().Save();
            _logger.Received().LogInfo(Arg.Is<string>(m => m.Contains($"Colateral Updated with id : {id}")));
        }

        [Test]
        public void UpdateCollateral_WithNonExistingCollateral_ShouldReturnErrorResult()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var collateralDto = new CollateralDTO();
            _repository.Collateral.GetByKey(id).Returns((Collateral)null);

            // Act
            var result = _collateralAppServices.UpdateCollateral(id, collateralDto);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Collateral Doesn't Exist", result.Message);
            _repository.DidNotReceive().Save();
            _logger.Received().LogInfo(Arg.Is<string>(m => m.Contains($"Colateral Not Found with id : {id}")));
        }

        [Test]
        public void DeleteCollateral_WithNonExistingCollateral_ShouldReturnNotFoundResult()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            _repository.Collateral.GetByKey(id).Returns((Collateral)null);

            // Act
            var result = _collateralAppServices.DeleteCollateral(id);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("Collateral Doesn't Exist", result.Message);
            _repository.DidNotReceive().Save();

        }

        [Test]
        public void DeleteCollateral_WithExistingCollateralAndNoLoan_ShouldReturnSuccessResult()
        {
            // Arrange
            var id = Guid.NewGuid().ToString();
            var collateral = new Collateral { Id = id };
            _repository.Collateral.GetByKey(id).Returns(collateral);

            // Act
            var result = _collateralAppServices.DeleteCollateral(id);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual("Deleted successfuly", result.Message);
            _repository.Received().Save();

        }
    }
}
