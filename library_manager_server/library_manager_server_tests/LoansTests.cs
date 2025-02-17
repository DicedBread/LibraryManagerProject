using System.Net;
using System.Runtime.InteropServices.JavaScript;
using System.Security.Claims;
using library_manager_server;
using library_manager_server.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework.Internal;
using Book = library_manager_server.Model.Book;

namespace library_manager_server_tests;

[TestFixture]
public class LoansTests
{
    private const long TestUserId = 1;
    private const string TestSessionId = "1";
    private const string TestBookISBN = "12ab"; 

    private Mock<ISessionHandler> _sessionHandlerMock;
    private Mock<ILibraryManager> _libraryManagerMock;
    private ControllerContext _controllerContext;
    private List<library_manager_server.Model.Loan> _loans = new List<library_manager_server.Model.Loan>();
    private library_manager_server.Model.Loan _testLoan;
    
    [SetUp]
    public void Setup()
    {
        _loans.Clear();
        for (int i = 0; i < 10; i++)
        {
            _loans.Add(new library_manager_server.Model.Loan()
            {
                LoanId = i,
                UserId = TestUserId,
                Book = new library_manager_server.Model.Book
                {
                    Isbn = "a" + i.ToString(),
                    Title = "text book",
                    Authour = "bob",
                    Publisher = "testBooks",
                    ImgUrl = "test.com",
                },
                Date = DateOnly.FromDateTime(new DateTime(2017, 1, i + 1)),
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
        
        _libraryManagerMock.Setup(m => m.CreateLoan(TestBookISBN, TestUserId, It.IsAny<DateOnly>())).Returns(_testLoan);
    }
    
    [Test]
    public async Task GetLoan_Valid()
    {
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<library_manager_server.Model.Loan> ret = await controller.GetLoan(1);
        
        Assert.IsNotNull(ret.Value);
        Assert.That(_testLoan, Is.EqualTo(ret.Value)); 
    }

    [Test]
    public async Task GetLoan_NoSession()
    {
        _sessionHandlerMock.Setup(m => m.GetSession(It.IsAny<HttpContext>())).Returns((string?) null); // no session in http context
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<library_manager_server.Model.Loan> ret = await controller.GetLoan(1);
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task GetLoan_DeadSession()
    {
        _sessionHandlerMock.Setup(m => m.IsActiveSession(It.IsAny<string>())).Returns(false); // dead session 
        _sessionHandlerMock.Setup(m => m.GetUserId(It.IsAny<string>())).Returns((long?) null); // dead session
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<library_manager_server.Model.Loan> ret = await controller.GetLoan(1);
        
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
        ActionResult<library_manager_server.Model.Loan> ret = await controller.GetLoan(1);
        
        ForbidResult? returnValue = ret.Result as ForbidResult; // forbid res does not contain status code 
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.GetType(), Is.EqualTo(typeof(ForbidResult))); 
    }
    
    [Test]
    public async Task GetLoan_LoanDoesNotExist(){
        _libraryManagerMock.Setup(m => m.GetLoan(1)).Returns((Func<library_manager_server.Model.Loan?>)null); // no loan
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<library_manager_server.Model.Loan> ret = await controller.GetLoan(1);

        BadRequestResult? returnValue = ret.Result as BadRequestResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task GetLoans_Valid()
    {
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<List<library_manager_server.Model.Loan>> ret = await controller.GetLoans();
        
        Assert.IsNotNull(ret.Value);
        Assert.That(_loans, Is.EqualTo(ret.Value)); 
    }

    [Test]
    public async Task GetLoans_DeadSession()
    {
        _sessionHandlerMock.Setup(m => m.IsActiveSession(It.IsAny<string>())).Returns(false);
        _sessionHandlerMock.Setup(m => m.GetUserId(It.IsAny<string>())).Returns((long?) null);
        
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        ActionResult<List<library_manager_server.Model.Loan>> ret = await controller.GetLoans();
        
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
        ActionResult<List<library_manager_server.Model.Loan>> ret = await controller.GetLoans();
        
        UnauthorizedResult? returnValue = ret.Result as UnauthorizedResult;
        Assert.IsNotNull(returnValue);
        Assert.That(returnValue.StatusCode, Is.EqualTo((int) HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task CreateLoan_Valid()
    {
        Loans controller = new Loans(_libraryManagerMock.Object, NullLogger<Loans>.Instance, _sessionHandlerMock.Object);
        controller.ControllerContext = _controllerContext;
        HttpContext httpContext = new DefaultHttpContext();
        httpContext.Request.Method = "POST";
        httpContext.Request.Scheme = "https";
        httpContext.Request.ContentType = "application/json";
        httpContext.Request.Host = new HostString("localhost");
        httpContext.Request.Path = $"/api/loans/loan/{TestBookISBN}";
        controller.ControllerContext.HttpContext = httpContext;
        
        IActionResult ret = await controller.CreateLoan(TestBookISBN);
        CreatedResult? Result = ret as CreatedResult;    
        
        Console.WriteLine(Result.ToString());
        Assert.IsNotNull(Result);
        Assert.That(Result.StatusCode, Is.EqualTo((int) HttpStatusCode.Created));
    }
}