using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class PromotionAppServicesTests
    {
        private IMapper _mapper;
        private ILogger _logger;
        private IRepositoryManager _repository;
        private PromotionAppServices _promotionAppServices;

        [SetUp]
        public void Setup()
        {
            _mapper = Substitute.For<IMapper>();
            _logger = Substitute.For<ILogger>();
            _repository = Substitute.For<IRepositoryManager>();
            _promotionAppServices = new PromotionAppServices(_mapper, _logger, _repository);
        }

        [Test]
        public void AddPromotion_WithActivePromotionExists_ShouldDeactivateActivePromotionAndAddNewPromotion()
        {
            // Arrange
            var activePromotion = new Promotion
            {
                Active = true,
            };
            var pmodel = new PromotionDTO()
            {
                Active = true,
                PromotionId = Guid.NewGuid().ToString(),
            };
            var newPromotion = new Promotion
            {
                Active = true,
                End_date = DateTime.Now.AddDays(1),
                Message = "abc",
                PromotionId = Guid.NewGuid().ToString(),
                Start_date = DateTime.Now,
                Type = 0
            };
            var promotionList = new List<Promotion> { activePromotion }.AsQueryable();
            _repository.Promotion.FindByCondition(Arg.Any<Expression<Func<Promotion, bool>>>(), false)
                .Returns(promotionList);

            _mapper.Map<PromotionDTO, Promotion>(pmodel).Returns(newPromotion);

            // Act
            var result = _promotionAppServices.AddPromotion(pmodel);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.Message, Is.EqualTo("Bank Promotion Added"));
            Assert.That(activePromotion.Active, Is.False);
            _repository.Promotion.Received().Update(activePromotion);
            _repository.Promotion.Received().Create(Arg.Any<Promotion>());
            _repository.Received().Save();
            _logger.Received().LogInfo(Arg.Any<string>());
        }

        [Test]
        public void AddPromotion_WithoutActivePromotion_ShouldAddNewPromotion()
        {
            // Arrange
            var newPromotion = new Promotion
            {
                Active = true,
                End_date = DateTime.Now.AddDays(1),
                Message = "abc",
                PromotionId = Guid.NewGuid().ToString(),
                Start_date = DateTime.Now,
                Type = 0
            };
            _repository.Promotion.FindByCondition(Arg.Any<Expression<Func<Promotion, bool>>>(), false)
                .Returns(new Promotion[0].AsQueryable());
            var pmodel = new PromotionDTO();
            _mapper.Map<PromotionDTO, Promotion>(pmodel).Returns(newPromotion);

            // Act
            var result = _promotionAppServices.AddPromotion(pmodel);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo(201));
            Assert.That(result.Message, Is.EqualTo("Bank Promotion Added"));
            _repository.Promotion.Received().Create(Arg.Any<Promotion>());
            _repository.Received().Save();
            _logger.Received().LogInfo(Arg.Any<string>());
        }

        [Test]
        public void GetPromotion_WithActivePromotionExists_ShouldReturnActivePromotion()
        {
            // Arrange
            var activePromotion = new Promotion
            {
                Active = true,
                End_date = DateTime.Now.AddDays(1),
                Message = "abc",
                PromotionId = Guid.NewGuid().ToString(),
                Start_date = DateTime.Now,
                Type = 0
            };

            var promotionList = new List<Promotion> { activePromotion }.AsQueryable();
            var promotionDTO = new PromotionDTO();
            _repository.Promotion.FindByCondition(Arg.Any<Expression<Func<Promotion, bool>>>(), false).Returns(promotionList);
            _mapper.Map<Promotion, PromotionDTO>(activePromotion).Returns(promotionDTO);

            // Act
            var result = _promotionAppServices.GetPromotion();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Message, Is.Empty);
            Assert.That(result.Data, Is.SameAs(promotionDTO));
            _logger.Received().LogInfo(Arg.Any<string>());
        }

        [Test]
        public void GetPromotion_NoActivePromotion_ReturnsErrorResult()
        {
            // Arrange

            var promotions = new List<Promotion>() {
                new Promotion() { Active = false },
                new Promotion() { Active = false },
                new Promotion() { Active = false }
            };
            _repository.Promotion.FindByCondition(Arg.Any<Expression<Func<Promotion, bool>>>(), Arg.Any<bool>())
                .Returns(promotions.AsQueryable());

            // Act
            var result = _promotionAppServices.GetPromotion();

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("No Active Bank Promotion", result.Message);
            Assert.IsNull(result.Data);

            _logger.Received(1).LogInfo(Arg.Any<string>());
        }
    }
}
