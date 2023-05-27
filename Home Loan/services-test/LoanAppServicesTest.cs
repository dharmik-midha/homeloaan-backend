using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BusinessLogic.AppServices;
using BusinessLogic.DTO;
using Constants.Enums;
using Constants.Values;
using DataAccess.Contracts;
using DataAccess.Entities;
using Logging.NLog;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class LoanAppServicesTests
    {
        private ILogger _logger;
        private IMapper _mapper;
        private IRepositoryManager _repository;
        private ILoanStateAppService _loanStateAppService;
        private UserManager<IdentityUser> _userManager;
        private LoanAppServices _service;

        [SetUp]
        public void SetUp()
        {
            _logger = Substitute.For<ILogger>();
            _mapper = Substitute.For<IMapper>();
            _repository = Substitute.For<IRepositoryManager>();
            _loanStateAppService = Substitute.For<ILoanStateAppService>();
            _userManager = Substitute.For<UserManager<IdentityUser>>(
                Substitute.For<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);
            _service = new LoanAppServices(
                _logger, _mapper, _repository, _userManager, _loanStateAppService);
        }

        [Test]
        public void CreateNewLoan_Success_ReturnsTrue()
        {
            // Arrange
            var loanDto = new LoanDTO { /* set properties */ };
            var loan = new Loan { /* set properties */ };
            _mapper.Map<LoanDTO, Loan>(loanDto).Returns(loan);

            // Act
            var result = _service.CreateNewLoan(loanDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            Assert.IsEmpty(result.Message);
            _repository.Loan.Received(1).Create(loan);
            _repository.Received(1).Save();
            _loanStateAppService.Received(1).AddLoanState(Arg.Is<LoanStateDTO>(
                x => x.LoanID == loan.LoanId
                && x.State == LoanStates.Created));
        }

        [Test]
        public void ApplyLoan_LoanNotFound_ReturnsFalse()
        {
            // Arrange
            var email = "example@example.com";
            var loanId = Guid.NewGuid().ToString();
            _repository.Loan.GetByKey(loanId).Returns((Loan)null);

            // Act
            var result = _service.ApplyLoan(email, loanId);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(StatusCodes.Status404NotFound, result.StatusCode);
            Assert.AreEqual("Loan not found", result.Message);
            _loanStateAppService.DidNotReceive().AddLoanState(Arg.Any<LoanStateDTO>());
        }
    }
}