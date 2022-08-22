using Academy.Domain.Entities;
using Academy.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Academy.Controllers;

public class StudentController : Controller
{
    private readonly IHierarchyService<Student> _studentService;
    private readonly IHierarchyService<Group> _groupService;
    private readonly IHierarchyService<Course> _courseService;
    private readonly CourseController _courseController;

    public StudentController(IHierarchyService<Student> studentService, IHierarchyService<Course> courseService,
        GroupController groupController, IHierarchyService<Group> groupService, CourseController courseController)
    {
        _studentService = studentService;
        _groupService = groupService;
        _courseController = courseController;
        _courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courseResponse = await _courseService.GetAll();
        var groupResponse = await _groupService.GetAll();
        var studentResponse = await _studentService.GetAll();

        if (courseResponse.Data != null && groupResponse.Data != null && studentResponse.Data != null 
            && courseResponse.StatusCode == Domain.Enums.StatusCode.Ok && groupResponse.StatusCode == Domain.Enums.StatusCode.Ok && studentResponse.StatusCode == Domain.Enums.StatusCode.Ok)
        {
            ViewData["Courses"] = courseResponse.Data.ToList();
            ViewData["Groups"] = groupResponse.Data.ToList();
            ViewData["Students"] = studentResponse.Data.ToList();

            return View();
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpGet]
    public async Task<IActionResult> Create(int studentId)
    {
        ViewData["CoursesList"] = await _courseController.GetCourseSelectList();

        if (studentId == 0 && ViewData["CoursesList"] != null)
        {
            return await Task.FromResult(View(new Student()
            {
                Id = studentId,
            }));
        }

        var studentResponse = await _studentService.GetById(studentId);

        if (studentResponse.StatusCode == Domain.Enums.StatusCode.Ok && studentResponse.Data != null &&
            ViewData["CoursesList"] != null)
        {
            return View(studentResponse.Data);
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpPost]
    public async Task<IActionResult> Create(Student model)
    {
        if (model.GroupId == 0)
        {
            ModelState.AddModelError("GroupId", "Select the group.");
        }

        if (ModelState.IsValid)
        {
            if (model.Id == 0)
            {
                await _studentService.Create(model);

                TempData["StudentSuccessAlertMessage"] = $"{model.FirstName} {model.LastName} student created successfully.";

                return RedirectToAction("Select", "Group", new {groupId = model.GroupId});
            }

            await _studentService.Update(model.Id, model);

            TempData["StudentSuccessAlertMessage"] = $"{model.FirstName} {model.LastName} student updated.";

            return RedirectToAction("Select", "Group", new { groupId = model.GroupId });
        }

        ViewData["CoursesList"] = await _courseController.GetCourseSelectList();

        return await Task.FromResult(View(model));
    }

    public async Task<IActionResult> Delete(int studentId)
    {
        var studentResponse = await _studentService.Delete(studentId);

        if (studentResponse.StatusCode == Domain.Enums.StatusCode.Ok && studentResponse.Data != null) 
        {
            TempData["StudentSuccessAlertMessage"] = $"{studentResponse.Data.FirstName} {studentResponse.Data.LastName} student deleted successfully.";

            return RedirectToAction("Select", "Group", new {groupId = studentResponse.Data.GroupId});
        }

        return RedirectToAction("Error", "Error");
    }

    public async Task<JsonResult> GetGroupSelectList(int courseId)
    {
        var groupResponse = await _groupService.GetAll();

        if (groupResponse.Data != null)
        {
            var groupSelectList = groupResponse.Data.Where(g => g.CourseId == courseId).ToList();

            groupSelectList.Insert(0, new Group{Id = 0, Name = "Select group"});

            return Json(new SelectList(groupSelectList, "Id", "Name"));
        }

        return Json(new SelectList(string.Empty).ToList());
    }

    private async Task<SelectList?> GetCourseSelectList()
    {
        var courseResponse = await _courseService.GetAll();
        if (courseResponse.StatusCode is not (Domain.Enums.StatusCode.Ok or Domain.Enums.StatusCode.CoursesNotFound))
        {
            return null;
        }

        return new SelectList(courseResponse.Data, "Id", "Name");
    }
}