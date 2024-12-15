using System.Net;
using System.Runtime.InteropServices.JavaScript;
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
    private const string TestBookISBN = "12ab"; 

    private Mock<ISessionHandler> _sessionHandlerMock;
    private Mock<ILibraryManager> _libraryManagerMock;
    private ControllerContext _controllerContext;
    private List<Loan> _loans = new List<Loan>();
    private Loan _testLoan;
    
    [SetUp]
    public void Setup()
    {
        _loans.Clear();
        for (int i = 0; i < 10; i++)
        {
            _loans.Add(new Loan()
            {
                LoanId = i,
                UserId = TestUserId,
                Isbn = "a" + i.ToString(),
                Date = new DateTime(2017, 1, i + 1),
            });
        }
        _testLoan = _loans[0];
        
        _sessionHandlerMock = new Mock<ISessionHandler>();
        _libraryManagerMock = new Mock<ILibraryManager>();
        _controllerContext = new ControllerContext()
        {
            HttpContext = new DefaultHttpContext()
        };
        
        _sessionHandlerMock.Setup(m => m.IsActiveSession(TestSessionId)).Returns(true);
        _sessionHandlerMock.Setup(m => m.GetUserId(TestSessionId)).Returns(TestUserId);
        _sessionHandlerMock.Setup(m => m.GetSession(It.IsAny<HttpContext>())).Returns(TestSessionId);
        
        _libraryManagerMock.Setup(m => m.GetLoan(TestUserId)).Returns(_testLoan);
        _libraryManagerMock.Setup(m => m.OwnsLoan(1, TestUserId)).Returns(true);
        
        _libraryManagerMock.Setup(m => m.GetLoans(TestUserId)).Returns(_loans);
        
        // _libraryManagerMock.Setup(m => m.CreateLoan(TestBookISBN, TestUserId, It.IsAny<DateTime>())).Returns(null);
    }
    
    [Test]
    public async Task GetLoan_Valid()
    {
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        Assert.IsNotNull(ret.Value);
        Assert.That(_testLoan, Is.EqualTo(ret.Value)); 
    }

    [Test]
    public async Task GetLoan_NoSession()
    {
        _sessionHandlerMock.Setup(m => m.GetSession(It.IsAny<HttpContext>())).Returns((string?) null); // no session in http context
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetLoan_DeadSession()
    {
        _sessionHandlerMock.Setup(m => m.IsActiveSession(It.IsAny<string>())).Returns(false); // dead session 
        _sessionHandlerMock.Setup(m => m.GetUserId(It.IsAny<string>())).Returns((double?) null); // dead session
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetLoan_DoesNotOwnLoan()
    {
        _libraryManagerMock.Setup(m => m.OwnsLoan(1, 1)).Returns(false); // does not own loan 
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);
        
        ForbidResult? returnValue = ret.Result as ForbidResult; // forbid res does not contain status code 
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.GetType(), Is.EqualTo(typeof(ForbidResult))); 
    }
    
    [Test]
    public async Task GetLoan_LoanDoesNotExist(){
        _libraryManagerMock.Setup(m => m.GetLoan(1)).Returns((Loan?) null); // no loan
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<Loan> ret = await controller.GetLoan(1);

        BadRequestResult? returnValue = ret.Result as BadRequestResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetLoans_Valid()
    {
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<List<Loan>> ret = await controller.GetLoans();
        
        Assert.IsNotNull(ret.Value);
        Assert.That(_loans, Is.EqualTo(ret.Value)); 
    }

    [Test]
    public async Task GetLoans_DeadSession()
    {
        _sessionHandlerMock.Setup(m => m.IsActiveSession(It.IsAny<string>())).Returns(false);
        _sessionHandlerMock.Setup(m => m.GetUserId(It.IsAny<string>())).Returns((double?) null);
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<List<Loan>> ret = await controller.GetLoans();
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetLoans_NoSession()
    {
        _sessionHandlerMock.Setup(m => m.GetSession(It.IsAny<HttpContext>())).Returns((string?) null);
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<List<Loan>> ret = await controller.GetLoans();
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }


}