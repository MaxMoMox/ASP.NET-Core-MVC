using Academy.DAL.Interfaces;
using Academy.Domain.Entities;
using Academy.Domain.Enums;
using Academy.Domain.Responses;
using Academy.Service.Implementations;
using Academy.Service.Interfaces;
using Moq;
using NUnit.Framework;

namespace Academy.Service.Tests;

public class GroupServiceTests
{
    private Mock<IBaseRepository<Group>> _groupRepository;
    private Mock<IBaseRepository<Student>> _studentRepository;
    private IHierarchyService<Group> _service;

    [SetUp]
    public void Setup()
    {
        _groupRepository = new Mock<IBaseRepository<Group>>();
        _studentRepository = new Mock<IBaseRepository<Student>>();
        _service = new GroupService(_groupRepository.Object, _studentRepository.Object);

        _studentRepository.Setup(s => s.GetAll()).ReturnsAsync(new List<Student>());
        _groupRepository.Setup(g => g.GetAll()).ReturnsAsync(GetSomeGroups);
        _groupRepository.Setup(g => g.GetById(1)).ReturnsAsync(new Group {Id = 1, CourseId = 1, Name = "Name"});
    }

    [Test]
    public async Task Create_CorrectData()
    {
        var model = new Group
        {
            Id = 1,
            CourseId = 1,
            Name = "Name",
        };

        var expectedResponse = new BaseResponse<Group>
        {
            Data = model,
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Create(model);

        Assert.That(actualResponse.Data!.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Create_IncorrectData()
    {
        var model1 = new Group
        {
            CourseId = 1,
            Name = String.Empty,
        };

        var model2 = new Group
        {
            Name = "Name",
        };

        var model3 = new Group();

        var expectedResponse = new BaseResponse<Group>
        {
            Description = "[GroupService.Create] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var actualResponse1 = await _service.Create(model1);
        var actualResponse2 = await _service.Create(model2);
        var actualResponse3 = await _service.Create(model3);

        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse3.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse3.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
    }

    [Test]
    public async Task Update_CorrectData()
    {
        var model = new Group()
        {
            CourseId = 2,
            Name = "Test group updated",
        };

        var expectedResponse = new BaseResponse<Group>
        {
            Data = model,
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Update(1, model);

        Assert.That(actualResponse.Data!.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));

        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));
        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
    }

    [Test]
    public async Task Update_IncorrectData()
    {
        var model1 = new Group
        {
            Id = 1,
            CourseId = 1,
            Name = String.Empty,
        };

        var model2 = new Group
        {
            Id = 1,
            Name = "Name",
        };

        var model3 = new Group();

        var model4 = new Group
        {
            Id = 1,
            CourseId = 1,
            Name = "Name",
        };

        var expectedResponse = new BaseResponse<Group>
        {
            Description = "[GroupService.Update] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var actualResponse1 = await _service.Update(1, model1);
        var actualResponse2 = await _service.Update(1, model2);
        var actualResponse3 = await _service.Update(1, model3);
        var actualResponse4 = await _service.Update(100, model4);

        Assert.That(actualResponse1.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse1.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse2.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse2.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse3.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse3.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse4.Description, Is.EqualTo("[GroupService.Update] : Wrong group ID. There are no groups with this ID."));
        Assert.That(actualResponse4.StatusCode, Is.EqualTo(StatusCode.GroupsNotFound));
    }

    [Test]
    public async Task Delete_CorrectData()
    {
        var expectedResponse = new BaseResponse<Group>
        {
            Data = new Group{Id = 1, CourseId = 1, Name = "Name"},
            StatusCode = StatusCode.Ok
        };

        var actualResponse = await _service.Delete(1);

        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse.Data!.Id, Is.EqualTo(expectedResponse.Data.Id));
        Assert.That(actualResponse.Data.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));
    }

    [Test]
    public async Task Delete_NotEmptyGroup_Error()
    {
        var expectedResponse = new BaseResponse<Group>
        {
            Data = new Group { Id = 1, CourseId = 1, Name = "Name" },
            StatusCode = StatusCode.NotEmptyGroupRemoval
        };

        _studentRepository.Setup(s => s.GetAll()).ReturnsAsync(new List<Student>{new Student{Id = 1, GroupId = 1, FirstName = "Name", LastName = "Name"}});

        var actualResponse = await _service.Delete(1);

        Assert.That(actualResponse.Description, Is.EqualTo(expectedResponse.Description));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(expectedResponse.StatusCode));

        Assert.That(actualResponse.Data!.Id, Is.EqualTo(expectedResponse.Data.Id));
        Assert.That(actualResponse.Data.Name, Is.EqualTo(expectedResponse.Data.Name));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedResponse.Data.CourseId));
    }

    [Test]
    public async Task Delete_IncorrectId()
    {
        var expectedResponse1 = new BaseResponse<Group>
        {
            Description = "[GroupService.Delete] : Invalid input data.",
            StatusCode = StatusCode.InputDataError
        };

        var expectedResponse2 = new BaseResponse<Group>
        {
            Description = "[GroupService.Delete] : Wrong group ID. There are no groups with this ID.",
            StatusCode = StatusCode.GroupsNotFound
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

        Assert.That(actualResponse.Data, Has.Count.EqualTo(GetSomeGroups().Count));
        Assert.That(actualResponse.Description, Is.EqualTo(null));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.Ok));
    }

    [Test]
    public async Task GetById_CorrectData()
    {
        var expectedGroup = new Group
        {
            Id = 1,
            CourseId = 1,
            Name = "Name"
        };

        var actualResponse = await _service.GetById(1);

        Assert.That(actualResponse.Data!.Id, Is.EqualTo(expectedGroup.Id));
        Assert.That(actualResponse.Data.Name, Is.EqualTo(expectedGroup.Name));
        Assert.That(actualResponse.Data.CourseId, Is.EqualTo(expectedGroup.CourseId));

        Assert.That(actualResponse.Description, Is.EqualTo(null));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.Ok));
    }

    [Test]
    public async Task GetById_IncorrectData()
    {
        var actualResponse = await _service.GetById(200);

        Assert.That(actualResponse.Description, Is.EqualTo("[GroupService.GetById] : Wrong group ID. There are no groups with this ID."));
        Assert.That(actualResponse.StatusCode, Is.EqualTo(StatusCode.GroupsNotFound));
    }

    private static List<Group> GetSomeGroups()
    {
        return new List<Group>()
        {
            new Group()
            {
                Id = 1,
                CourseId = 1,
                Name = "First Group"
            },
            new Group()
            {
                Id = 2,
                CourseId = 2,
                Name = "Second Group"
            },
            new Group()
            {
                Id = 3,
                CourseId = 3,
                Name = "Third Group"
            }
        };
    }
}