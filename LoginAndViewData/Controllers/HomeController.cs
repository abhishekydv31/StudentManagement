
using Humanizer;
using LoginAndViewData.Migrations;
using LoginAndViewData.Models;
using LoginAndViewData.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace LoginAndViewData.Controllers
{

    public class HomeController : Controller
    {
        private readonly StudentDB_2Context context;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment;
       

        public HomeController(StudentDB_2Context context, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            this.context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateStudent()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var model = new StudentViewModel();
            return View(model);
        }

        public IActionResult CommitRollback()
        {
            return View();
        }

     

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentViewModel model, IFormFile file)
        {
            
            ModelState.Remove(nameof(model.FileName));
            

            if (ModelState.IsValid)
            {
                string uniquefullname = null;
                if (file != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images1");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    uniquefullname = Guid.NewGuid().ToString("N").Substring(0, 3) + "_" + file.FileName;
                    string filepath = Path.Combine(uploadFolder, uniquefullname);
                    file.CopyTo(new FileStream(filepath, FileMode.Create));
                }
                StudentDetail stdnew = new StudentDetail
                {
                    Name = model.Name,
                    MobileNo = model.MobileNo,
                    BloodGroup = model.BloodGroup,
                    Gender = model.Gender,
                    FileName = uniquefullname,
                    Description = model.Description
                };

                try
                {
                    context.StudentDetail.Add(stdnew);
                    await context.SaveChangesAsync();
                    return RedirectToAction("Dashboard2");
                }
                catch (Exception ex)
                {
                    ViewBag.errorMessage1 = ex.InnerException;
                }
            }

            var errors_ = ModelState.Values.SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

            ViewBag.errorMessage = errors_;

            return View(model);
        }


        public IActionResult Login()
        {
           
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(StudentLogin std)
        {
            PasswordEncryption psw = new PasswordEncryption();
            var db_pass = context.StudentLogins.FirstOrDefault(item => item.Username == std.Username);
            if (db_pass==null) {
                ViewBag.ErrorMessage1 = "There is no user with {std.Username} Username";
            }
            var pass = psw.VerifyPassword(std.Password,db_pass.Password);
            if (pass)
            {
                HttpContext.Session.SetString("UserSession", db_pass.Username);
                return RedirectToAction("Dashboard");
            }
            else {
                ViewBag.ErrorMessage2 = "Wrong Credentials";
            }
            return View(std);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AddStudent()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var model = new DashboardViewModel() { };
            return View(model);
        }

        [HttpPost]
        public IActionResult AddStudent(DashboardViewModel model, IFormFile file, IFormCollection form)
        {
           
            ModelState.Remove(nameof(model.FileName));
            ModelState.Remove(nameof(model.SubjectNames));
            ModelState.Remove(nameof(model.StudentId));
            if (ModelState.IsValid)
            {
                string uniquefullname = null;
                if (file != null)
                {
                    string uploadFolder = Path.Combine(hostingEnvironment.WebRootPath, "images1");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    uniquefullname = Guid.NewGuid().ToString("N").Substring(0, 3) + "_" + file.FileName;
                    string filepath = Path.Combine(uploadFolder, uniquefullname);
                    file.CopyTo(new FileStream(filepath, FileMode.Create));
                }

                var studentdetail = new StudentDetail()
                {
                    Name = model.Name,
                    MobileNo = model.MobileNo,
                    BloodGroup = model.BloodGroup,
                    Gender = model.Gender,
                    FileName = uniquefullname,
                    Description = model.Description,
                };

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        context.StudentDetail.Add(studentdetail);
                        context.SaveChanges();


                        foreach (var item in model.MovieNames)
                        {
                            //var value = context.Movies.SingleOrDefault(s => s.MovieName == item);

                                var moviestudent = new MovieStudent()
                                {
                                    StudentId = studentdetail.StudentId,
                                    MovieId = int.Parse(item)
                                };
                                context.MovieStudents.Add(moviestudent);
                        }
                            int StudentId = studentdetail.StudentId;
                            var SelectedSubjectsIds = form["selectedSubject"].Select(int.Parse).ToList();

                            foreach (int subjectId in SelectedSubjectsIds)
                            {
                                var sub = new SubjectStudent
                                {
                                    StudentId = StudentId,
                                    SubjectId = subjectId
                                };
                                //context.Entry(sub).State = EntityState.Detached;
                                context.SubjectStudents.Add(sub);
                            }        
                        context.SaveChanges();
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.errorMessage1 = ex.ToString();
                        transaction.Rollback();
                        return View(model);
                    }
                    return RedirectToAction("Dashboard");
                }
            }
            var errors_ = ModelState.Values.SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage)
                                .ToList();

            ViewBag.modelstateError= errors_;
            return View(model);
        }
        public IActionResult SelectMovie()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }

            return View();
        }
        [HttpPost]
        public IActionResult SelectMovie([Bind(nameof(MovieViewModel.StudentId), nameof(MovieViewModel.SelectedMovies))] MovieViewModel ms)
        { 
            if (ms == null)
            {
                ViewBag.ErrorMessage3 = "No Model!!";
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var studentId = ms.StudentId;
                    var selectedMovieId = ms.SelectedMovies;

                    foreach (var movieId in selectedMovieId)
                    {
                        var sub = new MovieStudent
                        {
                            StudentId = studentId,
                            MovieId = movieId
                        };
                        context.MovieStudents.Add(sub);
                    }
                    context.SaveChanges();
                    return RedirectToAction("Dashboard2");
                }
                catch (DbUpdateException ex) {
                    ViewBag.ErrorMessage2 = ex.InnerException;
                    return View();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage3 = ex.InnerException;
                    return View();
                }
            }
            else
            {
                ViewBag.ModelStateErrors = ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage)
                   .ToList();
                return View();
            }
            
        }


        public IActionResult ListOfMovieSelected()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var movieForStudent = context.Movies.Join(context.MovieStudents, movie => movie.MovieId, moviestudent => moviestudent.MovieId, (movie, moviestudent) => new
            {
                movie.MovieName,
                moviestudent.StudentId
            }).ToList();
            return View(movieForStudent);
        }

        [HttpGet]
        public IActionResult StudentDetails()
        {
           
            return View();
        }

        public IActionResult SelectSubject()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            return View();
        }


        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }


            var viewmodellist = new List<DashboardViewModel>();

            var students = context.StudentDetail.ToList();
            var moviestudent = context.MovieStudents.ToList();
            var movie = context.Movies.ToList();
            var subjectstudent=context.SubjectStudents.ToList();
            var subjects=context.Subjects.ToList();

            foreach (var student in students) {
                var model = new DashboardViewModel {
                    StudentId=student.StudentId,
                    Name = student.Name,
                    MobileNo = student.MobileNo,
                    BloodGroup = student.BloodGroup,
                    Gender = student.Gender,
                    FileName = student.FileName,
                    Description = student.Description,
                    MovieNames = new List<String>(),
                    SubjectNames=new List<String>()
                };
                var studentMovies = moviestudent.Where(ms => ms.StudentId == student.StudentId);
                foreach (var studentMovie in studentMovies)
                {
                    var movieName = movie.FirstOrDefault(m => m.MovieId == studentMovie.MovieId)?.MovieName;
                    if (!string.IsNullOrEmpty(movieName))
                    {
                        model.MovieNames.Add(movieName);
                    }
                }

                var studentSubjects = subjectstudent.Where(ss => ss.StudentId == student.StudentId);
                foreach (var studentSubject in studentSubjects)
                {
                    var subjectName = subjects.FirstOrDefault(s => s.SubjectId == studentSubject.SubjectId)?.SubjectName;
                    if (!string.IsNullOrEmpty(subjectName))
                    {
                        model.SubjectNames.Add(subjectName);
                    }
                }
                viewmodellist.Add(model);
            }
                return View(viewmodellist);
        }
        public IActionResult ListOfSelectedSubject()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var subjectForStudent = context.Subjects.Join(context.SubjectStudents, subject => subject.SubjectId, subjectstudent => subjectstudent.SubjectId, (subject, subjectstudent) => new
            {
                subject.SubjectName,
                subjectstudent.StudentId
            }).ToList();

            return View(subjectForStudent);
        }
        [HttpPost]
        public IActionResult SelectSubject(IFormCollection form)
        {
            
            try
            {
                int StudentId = int.Parse(form["StudentId"]);
                var SelectedSubjectsIds = form["selectedSubject"].Select(int.Parse).ToList();

                foreach (int subjectId in SelectedSubjectsIds)
                {
                    var sub = new SubjectStudent
                    {
                        StudentId = StudentId,
                        SubjectId = subjectId
                    };
                    //context.Entry(sub).State = EntityState.Detached;
                    context.SubjectStudents.Add(sub);
                }
            }
            catch (SqlException ex)
            {
                ViewBag.ErrorMessage = ex.Message;
            }
            context.SaveChanges();
            return RedirectToAction("Dashboard2");
        }
        public IActionResult Dashboard2()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var stdData = context.StudentDetail.ToList();
            return View(stdData);
        }

        public IActionResult Edit(int? id)
        {
            
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            if (id == null)
            {
                return NotFound();
            }

            List<SelectListItem> BloodGroup = new()
            {
                new SelectListItem{ Value="A-", Text="A Negative"},
                new SelectListItem{ Value="A+", Text="A Positive"},
                new SelectListItem{ Value="B+",Text="B Positive"},
                new SelectListItem{ Value="B-",Text="B Negative"},
                new SelectListItem{ Value="O+",Text="O Positive"}
            };
            ViewBag.BloodGroup = BloodGroup;
            string str = context.StudentDetail.FromSqlRaw($"Select Blood_Group FROM StudentDetails").ToString();


            //List<string> movieid = (context.MovieStudents.FromSqlRaw($"SELECT MovieId From MovieStudents WHERE StudentId={id}").ToList()).ConvertAll(item=>item.ToString());

            List<string> movieIds = context.MovieStudents
                        .FromSqlRaw($"SELECT MovieId FROM MovieStudents WHERE StudentId={id}")
                        .Select(result => result.MovieId.ToString())
                        .ToList();

            //List<string> subject = (context.SubjectStudents.FromSqlRaw($"SELECT SubjectId From SubjectStudent WHERE StudentId={id}").ToList()).ConvertAll(item => item.ToString());

            List<string> subjectId = context.SubjectStudents
                        .FromSqlRaw($"SELECT SubjectId  FROM SubjectStudent WHERE StudentId={id}")
                        .Select(result => result.SubjectId.ToString())
                        .ToList();


            var stdData = context.StudentDetail.Find(id);
            if (stdData == null) { return NotFound(); }
            var dashboardviewmodel = new DashboardViewModel() {
                StudentId=stdData.StudentId,
                Name = stdData.Name,
                MobileNo = stdData.MobileNo,
                BloodGroup = stdData.BloodGroup,
                Gender = stdData.Gender,
                FileName = stdData.FileName,
                Description = stdData.Description,
                MovieNames = movieIds,
                SubjectNames=subjectId
            };
            if (stdData == null) { return NotFound(); }
            ViewBag.SelectedBloodGroup = str;
            return View(dashboardviewmodel);
        }

        [HttpPost]
        public IActionResult Edit(int? id, DashboardViewModel std,IFormCollection form)
        {
            if (id ==null)
            {
                return NotFound();
            }
            List<SelectListItem> BloodGroup = new()
            {
                new SelectListItem{ Value="A-", Text="A Negative"},
                new SelectListItem{ Value="A+", Text="A Positive"},
                new SelectListItem{ Value="B+",Text="B Positive"},
                new SelectListItem{ Value="B-",Text="B Negative"},
                new SelectListItem{ Value="O+",Text="O Positive"}
            };
            ViewBag.BloodGroup = BloodGroup;
            string str = context.StudentDetail.FromSqlRaw($"Select Blood_Group FROM StudentDetails").ToString();
            ViewBag.SelectedBloodGroup = str;

            ModelState.Remove(nameof(std.SubjectNames));


            if (ModelState.IsValid)
            {
                var studentdetail = new StudentDetail { 
                    StudentId=std.StudentId,
                  Name= std.Name,
                  MobileNo= std.MobileNo,
                  BloodGroup=std.BloodGroup,
                  Gender=std.Gender,
                  FileName=std.FileName,
                  Description=std.Description
                };
                context.StudentDetail.Update(studentdetail);
                context.SaveChanges();


                //Movie Updation

                var selectedMovieIds = std.MovieNames.Select(int.Parse).ToList();

                var existingMovieStudents = context.MovieStudents
                .Where(ms => ms.StudentId == studentdetail.StudentId)
                .ToList();

                foreach (var existingMovieStudent in existingMovieStudents)
                {
                    if (!selectedMovieIds.Contains(existingMovieStudent.MovieId))
                    {
                        context.MovieStudents.Remove(existingMovieStudent);
                    }
                }

                foreach (var selectedMovieId in selectedMovieIds)
                {
                    var existingMovieStudent = existingMovieStudents
                        .FirstOrDefault(ms => ms.MovieId == selectedMovieId);

                    if (existingMovieStudent == null)
                    {
                        var newMovieStudent = new MovieStudent()
                        {
                            StudentId = studentdetail.StudentId,
                            MovieId = selectedMovieId
                        };
                        context.MovieStudents.Add(newMovieStudent);
                    }
                }

           
                context.SaveChanges();

               
                var SelectedSubjectsIds = form["selectedSubject"].Select(int.Parse).ToList();

                var existingSubjectStudents = context.SubjectStudents
                .Where(ss => ss.StudentId == studentdetail.StudentId)
                .ToList();

                foreach (var existingSubjectStudent in existingSubjectStudents)
                {
                    if (!SelectedSubjectsIds.Contains(existingSubjectStudent.SubjectId))
                    {
                        context.SubjectStudents.Remove(existingSubjectStudent);
                    }
                }
                foreach (var selectedSubjectId in SelectedSubjectsIds)
                {
                    var existingSubjectStudent = existingSubjectStudents
                        .FirstOrDefault(ss => ss.SubjectId == selectedSubjectId);

                    if (existingSubjectStudent == null)
                    {
                        // Subject is already selected, do nothing
                        var newSubjectStudent = new SubjectStudent()
                        {
                            StudentId = studentdetail.StudentId,
                            SubjectId = selectedSubjectId
                        };
                        context.SubjectStudents.Add(newSubjectStudent);
                    } 
                }
                context.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }

            return View(std);
        }

        public IActionResult Delete(int? id)
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                ViewBag.MySession = HttpContext.Session.GetString("UserSession").ToString();
            }
            else
            {
                return RedirectToAction("Login");
            }
            var stdData = context.StudentDetail.Find(id);
            if (stdData == null) { return NotFound(); }
            List<string> movieIds = context.MovieStudents
                     .FromSqlRaw($"SELECT MovieId FROM MovieStudents WHERE StudentId={id}")
                     .Select(result => result.MovieId.ToString())
                     .ToList();

            //List<string> subject = (context.SubjectStudents.FromSqlRaw($"SELECT SubjectId From SubjectStudent WHERE StudentId={id}").ToList()).ConvertAll(item => item.ToString());

            List<string> subjectId = context.SubjectStudents
                        .FromSqlRaw($"SELECT SubjectId  FROM SubjectStudent WHERE StudentId={id}")
                        .Select(result => result.SubjectId.ToString())
                        .ToList();
            var dashboardviewmodel = new DashboardViewModel()
            {
                StudentId = stdData.StudentId,
                Name = stdData.Name,
                MobileNo = stdData.MobileNo,
                BloodGroup = stdData.BloodGroup,
                Gender = stdData.Gender,
                FileName = stdData.FileName,
                Description = stdData.Description,
                MovieNames = movieIds,
                SubjectNames = subjectId
            };
            if (stdData == null) { return NotFound(); }
            return View(dashboardviewmodel);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            using (var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
              
                try
                {
                    var stdData = context.StudentDetail.Find(id);
                    if (stdData != null)
                    {
                        //context.StudentDetails.Remove(stdData);
                        var movieStudents = context.MovieStudents.Where(ms => ms.StudentId == id).ToList();
                        context.MovieStudents.RemoveRange(movieStudents);

                        var subjectStudents = context.SubjectStudents.Where(ss => ss.StudentId == id).ToList();
                        context.SubjectStudents.RemoveRange(subjectStudents);

                        context.StudentDetail.Remove(stdData);

                        //context.DeleteAndReseed(id);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                    else
                    {
                        return NotFound();
                    }
                }

                catch (Exception ex)
                {
                    ViewBag.errorMessage = ex.Message;
                    transaction.Rollback();
                    return View();
                }
                return RedirectToAction("Dashboard");
            }
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserSession") != null)
            {
                HttpContext.Session.Remove("UserSession");
                return RedirectToAction("Login");
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}