using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Domain.Enums;
using Academy.Domain.Responses;
using Academy.Service.Implementations;
using Academy.Service.Interfaces;
using Moq;
using NUnit.Framework;

namespace Academy.Service.Tests;

public class StudentServiceTests
{
    private Mock<IBaseRepository<Student>> _studentRepository;
    private IHierarchyService<Student> _service;

    [SetUp]
    public void Setup()
    {
        _studentRepository = new Mock<IBaseRepository<Student>>();
        _service = new StudentService(_studentRepository.Object);

        _studentRepository.Setup(s => s.GetAll()).ReturnsAsync(GetSomeStudents);
        _studentRepository.Setup(s => s.GetById(1)).ReturnsAsync(new Student {Id = 1, FirstName = "First", LastName = "Last", CourseId = 1, GroupId = 1});
    }

    [Test]
    public async Task Create_CorrectData()
    {
        var model = new Student
        {
            Id = 1,
            FirstName = "First",
            LastName = "Last",
            CourseId = 1,
            GroupId = 1
        };

        var expectedResponse = new BaseResponse<Student>
        {
            Data = model,
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Create(model);

        Assert.That(actualResponse.Data!.FirstName, Is.EqualTo(expectedResponse.Data.FirstName));
        Assert.That(actualResponse.Data.LastName, Is.EqualTo(expectedResponse.Data.LastName));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));
        Assert.That(actualResponse.Data.GroupId, Is.EqualTo(expectedResponse.Data.GroupId));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Create_IncorrectData()
    {
        var model1 = new Student
        {
            FirstName = String.Empty,
            LastName = "Last",
            CourseId = 1,
            GroupId = 1
        };

        var model2 = new Student
        {
            FirstName = "First",
            LastName = String.Empty,
            CourseId = 1,
            GroupId = 1
        };
        var model3 = new Student
        {
            FirstName = "First",
            LastName = "Last",
            GroupId = 1
        };
        var model4 = new Student
        {
            FirstName = "First",
            LastName = "Last",
            CourseId = 1,
        };
        var model5 = new Student();

        var expectedResponse = new BaseResponse<Group>
        {
            Description = "[StudentService.Create] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var actualResponse1 = await _service.Create(model1);
        var actualResponse2 = await _service.Create(model2);
        var actualResponse3 = await _service.Create(model3);
        var actualResponse4 = await _service.Create(model4);
        var actualResponse5 = await _service.Create(model5);

        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse3.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse3.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse4.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse4.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse5.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse5.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

    }

    [Test]
    public async Task Update_CorrectData()
    {
        var model = new Student
        {
            FirstName = "First Updated",
            LastName = "Last Updated",
            CourseId = 2,
            GroupId = 2
        };

        var expectedResponse = new BaseResponse<Student>
        {
            Data = model,
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Update(1, model);

        Assert.That(actualResponse.Data!.FirstName, Is.EqualTo(expectedResponse.Data.FirstName));
        Assert.That(actualResponse.Data.LastName, Is.EqualTo(expectedResponse.Data.LastName));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));
        Assert.That(actualResponse.Data.GroupId, Is.EqualTo(expectedResponse.Data.GroupId));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Update_IncorrectData()
    {
        var model1 = new Student
        {
            FirstName = String.Empty,
            LastName = "Last",
            CourseId = 1,
            GroupId = 1
        };

        var model2 = new Student
        {
            FirstName = "First",
            LastName = String.Empty,
            CourseId = 1,
            GroupId = 1
        };
        var model3 = new Student
        {
            FirstName = "First",
            LastName = "Last",
            GroupId = 1
        };
        var model4 = new Student
        {
            FirstName = "First",
            LastName = "Last",
            CourseId = 1,
        };
        var model5 = new Student();

        var model6 = new Student
        {
            Id = 1,
            FirstName = "First",
            LastName = "Last",
            CourseId = 1,
            GroupId = 1
        };

        var expectedResponse = new BaseResponse<Group>
        {
            Description = "[StudentService.Update] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var actualResponse1 = await _service.Update(1, model1);
        var actualResponse2 = await _service.Update(1, model2);
        var actualResponse3 = await _service.Update(1, model3);
        var actualResponse4 = await _service.Update(1, model4);
        var actualResponse5 = await _service.Update(1, model5);
        var actualResponse6 = await _service.Update(100, model6);

        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse3.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse3.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse4.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse4.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse5.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse5.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse6.Description, Is.EqualTo("[StudentService.Update] : Wrong student ID. There are no students with this ID."));
        Assert.That(actualResponse6.StatusCode, Is.EqualTo(StatusCode.StudentsNotFound));
    }

    [Test]
    public async Task Delete_CorrectData()
    {
        var expectedResponse = new BaseResponse<Student>
        {
            Data = new Student{ Id = 1, FirstName = "First", LastName = "Last", CourseId = 1, GroupId = 1 },
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Delete(1);

        Assert.That(actualResponse.Data!.FirstName, Is.EqualTo(expectedResponse.Data.FirstName));
        Assert.That(actualResponse.Data.LastName, Is.EqualTo(expectedResponse.Data.LastName));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));
        Assert.That(actualResponse.Data.GroupId, Is.EqualTo(expectedResponse.Data.GroupId));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Delete_IncorrectId()
    {
        var expectedResponse1 = new BaseResponse<Student>
        {
            StatusCode = StatusCode.InputDataError,
            Description = "[StudentService.Delete] : Invalid input data."
        };

        var expectedResponse2 = new BaseResponse<Student>
        {
            StatusCode = StatusCode.StudentsNotFound,
            Description = "[StudentService.Delete] : Wrong student ID. There are no students with this ID."
        };

        var actualResponse1 = await _service.Delete(-1);
        var actualResponse2 = await _service.Delete(200);

        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse1.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse1.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse2.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse2.StatusCode));
    }

    [Test]
    public async Task GetAll_Equals()
    {
        var actualResponse = await _service.GetAll();

        Assert.That(actualResponse.Data, Has.Count.EqualTo(GetSomeStudents().Count));
        Assert.That(actualResponse.Description, Is.EqualTo(null));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.Ok));
    }

    [Test]
    public async Task GetById_CorrectData()
    {
        var expectedStudent = new Student
        {
            Id = 1,
            FirstName = "First",
            LastName = "Last",
            CourseId = 1,
            GroupId = 1
        };

        var actualResponse = await _service.GetById(1);

        Assert.That(actualResponse.Data!.FirstName, Is.EqualTo(expectedStudent.FirstName));
        Assert.That(actualResponse.Data.LastName, Is.EqualTo(expectedStudent.LastName));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedStudent.CourseId));
        Assert.That(actualResponse.Data.GroupId, Is.EqualTo(expectedStudent.GroupId));

        Assert.That(actualResponse.Description, Is.EqualTo(null));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.Ok));
    }

    [Test]
    public async Task GetById_IncorrectData()
    {
        var actualResponse = await _service.GetById(200);

        Assert.That(actualResponse.Description, Is.EqualTo("[StudentService.GetById] : Wrong student ID. There are no students with this ID."));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.StudentsNotFound));

    }

    private static List<Student> GetSomeStudents()
    {
        return new List<Student>
        {
            new Student
            {
                Id = 1,
                FirstName = "First",
                LastName = "Student",
                GroupId = 1,
                CourseId = 1,
            },
            new Student
            {
                Id = 2,
                FirstName = "Second",
                LastName = "Student",
                GroupId = 2,
                CourseId = 2,
            },
            new Student
            {
                Id = 3,
                FirstName = "Third",
                LastName = "Student",
                GroupId = 3,
                CourseId = 3,
            }
        };
    }
}