using DBEntity;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Threading.Tasks.Dataflow;
using System.Runtime.CompilerServices;

namespace DBEntity
{
    class Program
    {
        static void Main(string[] args)
        {

            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("Select an option: ");
                Console.WriteLine("1. Insert Student");
                Console.WriteLine("2. Update Student");
                Console.WriteLine("3. Delete Student");
                Console.WriteLine("4. GelAll Student");
                Console.WriteLine("5. Filter Student");
                Console.WriteLine("6. PatialSearch");
                Console.WriteLine("8. Exit");

                int input = Convert.ToInt32(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        //Insert

                        AddTeacher();
                        Console.WriteLine("Enter any key to Continue...");
                        Console.ReadKey();
                        break;

                    case 2:
                        //Update
                        UpdateTeacher();
                        Console.WriteLine("Enter any key to Continue...");
                        Console.ReadKey();
                        break;

                    case 3:
                        //Delete

                        break;

                    case 4:
                        //GetAll
                        /*
                        var context = new DemoContext();
                        var teracherlist = context.Teachers.ToList();

                        foreach (var teacher in teracherlist)
                        {
                            Console.WriteLine("ID:" + teacher.TeacherId + "Name:" + teacher.Name + "Narionality: " + teacher.Nationality);
                        }*/

                        using (var context = new DemoContext())
                        {
                            var assignedClasses = context.AssignedClasses
                                .Include(ac => ac.Class)
                                .Include(ac => ac.Teacher)
                                .Include(ac => ac.Student)
                                .GroupBy(ac => new  AssignClassGroupKey{ Tid = ac.TeacherId, CId = ac.ClassId })
                                .Select(group => new
                                {
                                    TeacherId = group.Key.Tid,
                                    ClassId = group.Key.CId,
                                    Count = group.Count()
                                })
                                .ToList();

                            foreach (var assignedClass in assignedClasses)
                            {
                                // var studentCount = assignedClass.Student.Classes.Count;
                                Console.WriteLine($"Teacher ID: {assignedClass.TeacherId}, " +
                                                  $"Class ID: {assignedClass.ClassId}, " +
                                                  $"Student Count: {assignedClass.Count}");
                            }
                        }
                        break;

                    case 5:
                        //Filter
                        FilterByTeacher();
                        Console.WriteLine("Enter any key to Continue...");
                        Console.ReadKey();
                        break;

                    case 6:
                        //pagination
                        Console.WriteLine("Enter page number:");
                        int page = Convert.ToInt32(Console.ReadLine());
                        int pageSize = 10;

                        using (var context4 = new DemoContext())
                        {
                            var teachers = context4.Teachers
                                .OrderBy(t => t.TeacherId)
                                .Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();

                            if (teachers != null)
                            {
                                foreach (var teacher in teachers)
                                {
                                    Console.WriteLine("ID:" + teacher.TeacherId + "Name:" + teacher.Name + "Narionality: " + teacher.Nationality);
                                }
                            }
                            else
                            {
                                Console.WriteLine("No such teacher found!");
                            }
                        }
                        
                        using (var context5 = new DemoContext())
                        {
                            var teachers = context5.Teachers
                                .GroupBy(t => t.BloodGroup)
                                .ToList();
                            foreach (var group in teachers)
                            {
                                foreach (var teacher in group)
                                {
                                    Console.WriteLine("ID:" + teacher.TeacherId + "Name:" + teacher.Name + "Narionality: " + teacher.Nationality);
                                }

                            }
                        }


                        break;
                    case 7:
                        //Group

                        //count 
                        /*using (var context6 = new DemoContext())
                        {
                            var teacherList = context6.Teachers.ToList() ;
                            var totalBloodGroup = teacherList.Count();

                            Console.WriteLine("Total Students: {0}", totalBloodGroup);

                        }

                        using (var context5 = new DemoContext())
                        {
                            var bloodGroupCounts = context5.Teachers
                                .GroupBy(t => t.BloodGroup)
                                .Select(group => new
                                {
                                    bloodGroup = group.Key,
                                    Count = group.Count()
                                })
                                .ToList();

                            foreach (var groupCount in bloodGroupCounts)
                            {
                                Console.WriteLine("Blood-Group:" +groupCount.bloodGroup + "Count:" +groupCount.Count);
                            }
                        }*/
                        break;

                    case 8:
                        //Exit
                        Console.WriteLine("Exited");
                        repeat = false;
                        break;

                    default:
                        Console.WriteLine("Exception Unhandled");
                        break;
                }
            }
        }

        private static void FilterByTeacher()
        {
            using (var context3 = new DemoContext())
            {
                Console.WriteLine("Enter string To filter:");
                string filter = Console.ReadLine();

                var filterteacher = context3.Teachers
                    .Where(t => t.Name.Contains(filter))
                    .ToList();

                if (filterteacher.Any())
                {
                    Console.WriteLine("Id\tName\tNationality");
                    foreach (var teacher in filterteacher)
                    {
                        Console.WriteLine(+teacher.TeacherId + "\t" + teacher.Name + "\t" + teacher.Nationality);
                    }
                }
                else
                {
                    Console.WriteLine("Teacher not found!");
                }
            }
        }

        private static void UpdateTeacher()
        {
            using (var context1 = new DemoContext())
            {

                Console.WriteLine("Enter Id:");
                if (int.TryParse(Console.ReadLine(), out int teacherId))
                {
                    var upteacher = context1.Teachers.FirstOrDefault(t => t.TeacherId == teacherId);
                    if (upteacher != null)
                    {
                        Console.WriteLine("Enter Name:");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter Phone:");
                        string phn = Console.ReadLine();
                        Console.WriteLine("Enter Address:");
                        string address = Console.ReadLine();
                        Console.WriteLine("Enter BloodGroup:");
                        string bg = Console.ReadLine();
                        Console.WriteLine("Enter Nationality:");
                        string nat = Console.ReadLine();
                        Console.WriteLine("Enter JoinDate:");
                        if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                        {
                            date = DateTime.Now;

                        }
                        else
                        {
                            Console.WriteLine("Invalid Date");
                        }

                        upteacher.Name = name;
                        upteacher.Phone = phn;
                        upteacher.Address = address;
                        upteacher.BloodGroup = bg;
                        upteacher.Nationality = nat;
                        upteacher.JoinDate = date;

                        context1.SaveChanges();

                        Console.WriteLine("Teacher information updated successfully!");

                    }
                    else
                    {
                        Console.WriteLine("Teacher not Found");
                    }
                }
            }
        }

        private static void AddTeacher()
        {
            using (var context = new DemoContext())
            {
                Console.WriteLine("Enter Name:");
                string name = Console.ReadLine();
                Console.WriteLine("Enter Phone:");
                string phn = Console.ReadLine();
                Console.WriteLine("Enter Address:");
                string address = Console.ReadLine();
                Console.WriteLine("Enter BloodGroup:");
                string bg = Console.ReadLine();
                Console.WriteLine("Enter Nationality:");
                string nat = Console.ReadLine();
                Console.WriteLine("Enter JoinDate:");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                {
                    date = DateTime.Now;

                }
                else
                {
                    Console.WriteLine("Invalid Date");
                }

                Teacher teacher = new Teacher()
                {
                    Name = name,
                    Phone = phn,
                    Address = address,
                    BloodGroup = bg,
                    Nationality = nat,
                    JoinDate = date
                };

                context.Teachers.Add(teacher);
                context.SaveChanges();

                Console.WriteLine("Teacher Inserted");
            }
        }

        public static TMyKey GetKey<TMyKey, TMyElement>(TMyElement ac)
        {
            return default;
        }

        public class AssignClassGroupKey
        {
            public int Tid { get; set; }
            public int CId { get; set; }
        }
    }

    
}