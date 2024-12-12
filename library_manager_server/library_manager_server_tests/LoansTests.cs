using System.Net;
using System.Security.Claims;
using library_manager_server;
using library_manager_server.model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework.Internal;

namespace library_manager_server_tests;

[TestFixture]
public class LoansTests
{
    private const double TestUserId = 1;
    private const string TestSessionId = "1";
    
    Mock<ISessionHandler> sessionHandlerMock;
    Mock<ILibraryManager> libraryManagerMock;
    ControllerContext controllerContext;
    Loan testLoan;
    
    [SetUp]
    public void Setup()
    {
        List<Loan> loans = new List<Loan>();
        for (int i = 0; i < 10; i++)
        {
            loans.Add(new Loan()
            {
                Loan_id = i,
                Isbn = "a" + i.ToString(),
                Date = new DateTime(2017, 1, i + 1),
            });
        }
        testLoan = loans[0];
        
        sessionHandlerMock = new Mock<ISessionHandler>();
        libraryManagerMock = new Mock<ILibraryManager>();
        controllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
        };
        
        sessionHandlerMock.Setup(m => m.IsActiveSession(TestSessionId)).Returns(true);
        sessionHandlerMock.Setup(m => m.GetUserId(TestSessionId)).Returns(TestUserId);
        sessionHandlerMock.Setup(m => m.GetSession(It.IsAny<HttpContext>())).Returns(TestSessionId);
        
        libraryManagerMock.Setup(m => m.GetLoan(TestUserId)).Returns(testLoan);
        libraryManagerMock.Setup(m => m.OwnsLoan(1, TestUserId)).Returns(true);
        
        libraryManagerMock.Setup(m => m.GetLoans(TestUserId)).Returns(loans);
    }
    
    [Test]
    public async Task GetLoan_Valid()
    {
        Loans controller = new Loans(libraryManagerMock.Object, NullLogger<Loans>.Instance, sessionHandlerMock.Object);
        controller.ControllerContext = controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        Assert.IsNotNull(ret.Value);
        Assert.That(testLoan, Is.EqualTo(ret.Value)); 
    }

    [Test]
    public async Task GetLoan_NoSession()
    {
        sessionHandlerMock.Setup(m => m.GetSession(It.IsAny<HttpContext>())).Returns((string?) null); // no session in http context
        
        Loans controller = new Loans(libraryManagerMock.Object, NullLogger<Loans>.Instance, sessionHandlerMock.Object);
        controller.ControllerContext = controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetLoan_DeadSession()
    {
        sessionHandlerMock.Setup(m => m.IsActiveSession(It.IsAny<string>())).Returns(false); // dead session 
        sessionHandlerMock.Setup(m => m.GetUserId(It.IsAny<string>())).Returns((double?) null); // dead session
        
        Loans controller = new Loans(libraryManagerMock.Object, NullLogger<Loans>.Instance, sessionHandlerMock.Object);
        controller.ControllerContext = controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetLoan_DoesNotOwnLoan()
    {
        libraryManagerMock.Setup(m => m.OwnsLoan(1, 1)).Returns(false); // does not own loan 
        
        Loans controller = new Loans(libraryManagerMock.Object, NullLogger<Loans>.Instance, sessionHandlerMock.Object);
        controller.ControllerContext = controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        ForbidResult? returnValue = ret.Result as ForbidResult; // forbid res does not contain status code 
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.GetType(), Is.EqualTo(typeof(ForbidResult))); 
    }
    
    [Test]
    public async Task GetLoan_LoanDoesNotExist(){
        libraryManagerMock.Setup(m => m.GetLoan(1)).Returns((Loan?) null); // no loan
        
        Loans controller = new Loans(libraryManagerMock.Object, NullLogger<Loans>.Instance, sessionHandlerMock.Object);
        controller.ControllerContext = controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);

        BadRequestResult? returnValue = ret.Result as BadRequestResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
    }


}