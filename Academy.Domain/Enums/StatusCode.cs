namespace Academy.Domain.Enums;

 public enum StatusCode
 {
     StudentsNotFound = 1,
     GroupsNotFound = 2,
     CoursesNotFound = 3,
     NotEmptyCourseRemoval = 4,
     NotEmptyGroupRemoval = 5,
     InputDataError = 6,
     Ok = 200,
     InternalServerError = 500,
 }