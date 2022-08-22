using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Domain.Enums;
using Academy.Domain.Responses;
using Academy.Service.Implementations;
using Academy.Service.Interfaces;
using Moq;
using NUnit.Framework;

namespace Academy.Service.Tests;

public class CourseServiceTests
{
    private Mock<IBaseRepository<Course>> _courseRepository;
    private Mock<IBaseRepository<Group>> _groupRepository;
    private IHierarchyService<Course> _service;

    [SetUp]
    public void Setup()
    {
        _courseRepository = new Mock<IBaseRepository<Course>>();
        _groupRepository = new Mock<IBaseRepository<Group>>();
        _service = new CourseService(_courseRepository.Object, _groupRepository.Object);

        _groupRepository.Setup(g => g.GetAll()).ReturnsAsync(new List<Group>());
        _courseRepository.Setup(c => c.GetAll()).ReturnsAsync(GetSomeCourses);
        _courseRepository.Setup(c => c.GetById(1)).ReturnsAsync(new Course { Id = 1, Name = "Name", Description = "Description" });
    }

    [Test]
    public async Task Create_CorrectData()
    {
        var model = new Course
        {
            Id = 1,
            Name = "Test course",
            Description = null
        };

        var expectedResponse = new BaseResponse<Course>
        {
            Data = model,
            StatusCode = StatusCode.Ok,
        };

        var actualResponse = await _service.Create(model);

        Assert.That(actualResponse.Data!.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.Description, Is.EqualTo(expectedResponse.Data.Description));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Create_IncorrectData()
    {
        var model1 = new Course
        {
            Name = string.Empty,
            Description = null
        };

        var model2 = new Course();

        var expectedResponse = new BaseResponse<Course>
        {
            Description = "[CourseService.Create] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var actualResponse1 = await _service.Create(model1);
        var actualResponse2 = await _service.Create(model2);

        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
    }

    [Test]
    public async Task Update_CorrectData()
    {
        var model = new Course
        {
            Id = 1,
            Name = "Test course updated",
            Description = "Description"
        };

        var expectedResponse = new BaseResponse<Course>
        {
            Data = model,
            StatusCode = StatusCode.Ok,
        };

        var actualResponse = await _service.Update(1, model);

        Assert.That(actualResponse.Data!.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.Description, Is.EqualTo(expectedResponse.Data.Description));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Update_IncorrectData()
    {
        var model1 = new Course();

        var model2 = new Course
        {
            Id = 1,
            Name = string.Empty,
            Description = "Description"
        };

        var model3 = new Course
        {
            Id = 1,
            Name = "Name",
            Description = "Description"
        };

        var expectedResponse = new BaseResponse<Course>
        {
            Description = "[CourseService.Update] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var actualResponse1 = await _service.Update(1, model1);
        var actualResponse2 = await _service.Update(1, model2);
        var actualResponse3 = await _service.Update(100, model3);


        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse3.Description, Is.EqualTo("[CourseService.Update] : Wrong course ID. There are no courses with this ID."));
        Assert.That(actualResponse3.StatusCode, Is.EqualTo(StatusCode.CoursesNotFound));
    }

    [Test]
    public async Task Delete_CorrectData()
    {
        var expectedResponse = new BaseResponse<Course>
        {
            Data = new Course{ Id = 1, Name = "Name", Description = "Description" },
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Delete(1);

        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse.Data!.Id, Is.EqualTo(expectedResponse.Data.Id));
        Assert.That(actualResponse.Data.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.Description, Is.EqualTo(expectedResponse.Data.Description));
    }

    [Test]
    public async Task Delete_NotEmptyCourse_Error()
    {
        var expectedResponse = new BaseResponse<Course>
        {
            Data = new Course { Id = 1, Name = "Name", Description = "Description" },
            StatusCode = StatusCode.NotEmptyCourseRemoval,
        };

        _groupRepository.Setup(g => g.GetAll()).ReturnsAsync(new List<Group>{new Group{Id = 1, Name = "Name", CourseId = 1}});

        var actualResponse = await _service.Delete(1);

        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse.Data!.Id, Is.EqualTo(expectedResponse.Data.Id));
        Assert.That(actualResponse.Data.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.Description, Is.EqualTo(expectedResponse.Data.Description));
    }

    [Test]
    public async Task Delete_IncorrectId()
    {
        var expectedResponse1 = new BaseResponse<Course>
        {
            StatusCode = StatusCode.InputDataError,
            Description = "[CourseService.Delete] : Invalid input data."
        };

        var expectedResponse2 = new BaseResponse<Course>
        {
            StatusCode = StatusCode.CoursesNotFound,
            Description = "[CourseService.Delete] : Wrong course ID. There are no courses with this ID."
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

        Assert.That(actualResponse.Data, Has.Count.EqualTo(GetSomeCourses().Count));
        Assert.That(actualResponse.Description, Is.EqualTo(null));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.Ok));
    }

    [Test]
    public async Task GetById_CorrectData()
    {
        var expectedCourse = new Course
        {
            Id = 1,
            Name = "Name",
            Description = "Description"
        };

        var actualResponse = await _service.GetById(1);

        Assert.That(actualResponse.Data!.Id, Is.EqualTo(expectedCourse.Id));
        Assert.That(actualResponse.Data.Name, Is.EqualTo(expectedCourse.Name));
        Assert.That(actualResponse.Data.Description, Is.EqualTo(expectedCourse.Description));

        Assert.That(actualResponse.Description, Is.EqualTo(null));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.Ok));
    }

    [Test]
    public async Task GetById_IncorrectData()
    {
        var actualResponse = await _service.GetById(200);

        Assert.That(actualResponse.Description, Is.EqualTo("[CourseService.GetById] : Wrong course ID. There are no courses with this ID."));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.CoursesNotFound));
    }

    private static List<Course> GetSomeCourses()
    {
        return new List<Course>
        {
            new Course
            {
                Id = 1,
                Name = "First Course",
                Description = null
            },
            new Course
            {
                Id = 2,
                Name = "First Course",
                Description = null
            },
            new Course
            {
                Id = 3,
                Name = "Third Course",
                Description = null
            },
        };
    }
}