using Academy.Domain.Entities;
using Academy.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Academy.Controllers;

public class GroupController : Controller
{
    private readonly IHierarchyService<Group> _groupService;
    private readonly IHierarchyService<Course> _courseService;
    private readonly IHierarchyService<Student> _studentService;
    private readonly CourseController _courseController;

    public GroupController(IHierarchyService<Group> groupService, IHierarchyService<Course> courseService, CourseController courseController, IHierarchyService<Student> studentService)
    {
        _groupService = groupService;
        _courseService = courseService;
        _courseController = courseController;
        _studentService = studentService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var courseResponse = await _courseService.GetAll();
        var groupResponse = await _groupService.GetAll();

        if (courseResponse.Data != null && groupResponse.Data != null && courseResponse.StatusCode == Domain.Enums.StatusCode.Ok && groupResponse.StatusCode == Domain.Enums.StatusCode.Ok)
        {
            ViewData["Courses"] = courseResponse.Data.ToList();
            ViewData["Groups"] = groupResponse.Data.ToList();

            return View();
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpGet]
    public async Task<IActionResult> Select(int groupId)
    {
        var studentResponse = await _studentService.GetAll();
        var groupResponse = await _groupService.GetById(groupId);

        if (studentResponse.Data != null && groupResponse.Data != null && groupResponse.StatusCode == Domain.Enums.StatusCode.Ok && studentResponse.StatusCode == Domain.Enums.StatusCode.Ok && groupId > 0)
        {
            ViewData["Group"] = groupResponse.Data;
            ViewData["Students"] = studentResponse.Data.Where(s => s.GroupId == groupId).ToList();

            return View();
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpGet]
    public async Task<IActionResult> Create(int groupId)
    {
        ViewData["CoursesList"] = await _courseController.GetCourseSelectList();

        if (groupId == 0 && ViewData["CoursesList"] != null)
        {
            return await Task.FromResult(View(new Group()
            {
                Id = groupId,
            }));
        }

        var groupResponse = await _groupService.GetById(groupId);

        if (groupResponse.StatusCode == Domain.Enums.StatusCode.Ok && ViewData["CoursesList"] != null)
        {
            return View(groupResponse.Data);
        }

        return RedirectToAction("Error", "Error");
    }

    [HttpPost]
    public async Task<IActionResult> Create(Group model)
    {
        if (ModelState.IsValid)
        {
            if (model.Id == 0)
            {
                await _groupService.Create(model);

                TempData["GroupSuccessAlertMessage"] = $"{model.Name} group created successfully.";

                return RedirectToAction("Select", "Course", new { courseId = model.CourseId });
            }

            await _groupService.Update(model.Id, model);

            TempData["GroupSuccessAlertMessage"] = $"{model.Name} group updated.";

            return RedirectToAction("Select", "Course", new { courseId = model.CourseId });
        }

        ViewData["CoursesList"] = await _courseController.GetCourseSelectList();

        return await Task.FromResult(View(model));
    }

    public async Task<IActionResult> Delete(int groupId)
    {
        var groupResponse = await _groupService.Delete(groupId);

        if (groupResponse.StatusCode == Domain.Enums.StatusCode.NotEmptyGroupRemoval && groupResponse.Data != null)
        {
            TempData["GroupErrorAlertMessage"] = $"{groupResponse.Data.Name} group contains some students. Delete all students inside {groupResponse.Data.Name} group first.";

            return RedirectToAction("Select", "Course", new { courseId = groupResponse.Data.CourseId });
        }

        if (groupResponse.StatusCode == Domain.Enums.StatusCode.Ok && groupResponse.Data != null)
        {
            TempData["GroupSuccessAlertMessage"] = $"{groupResponse.Data.Name} group deleted successfully.";

            return RedirectToAction("Select", "Course", new { courseId = groupResponse.Data.CourseId });
        }

        return RedirectToAction("Error", "Error");
    }
}