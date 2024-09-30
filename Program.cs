using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Xml.Serialization;

namespace Lab_10
{
    [Serializable]
    public class Student : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Student() { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Id", Id);
            info.AddValue("Name", Name);
            info.AddValue("Age", Age);
        }

        public Student(SerializationInfo info, StreamingContext context)
        {
            Id = info.GetInt32("Id");
            Name = info.GetString("Name");
            Age = info.GetInt32("Age");
        }

        public override string ToString()
        {
            return $"Student: Id: {Id}, Name: {Name}, Age: {Age}";
        }
    }

    [Serializable]
    public class StudentList : ISerializable
    {
        public List<Student> Students { get; set; } = new List<Student>();

        public StudentList() { }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Students", Students);
        }

        public StudentList(SerializationInfo info, StreamingContext context)
        {
            Students = (List<Student>)info.GetValue("Students", typeof(List<Student>));
        }

        public void Add(Student item)
        {
            Students.Add(item);
        }
    }
    class Program
    {
        public static void Main(string[] args)
        {
            StudentList studentList = new StudentList();
            studentList.Add(new Student { Id = 1, Name = "Lương", Age = 20 });
            studentList.Add(new Student { Id = 2, Name = "Huy", Age = 21 });
            studentList.Add(new Student { Id = 3, Name = "Hưởng", Age = 22 });

            //Chuyển dữ liệu Json ra file
            string filePath = "data.dat";
            string json = JsonSerializer.Serialize(studentList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
            Console.WriteLine("Data written to file.");

            //Đọc dữ liệu từ file ra Json mới
            string newjson = File.ReadAllText(filePath);
            Console.WriteLine("\nRead JSON from file:");
            Console.WriteLine(newjson);

            //Deserialize Json mới ra đối tượng dsach svien
            StudentList stlist = JsonSerializer.Deserialize<StudentList>(newjson);

            //Thêm svien mới
            stlist.Add(new Student { Id = 4, Name = "Lan", Age = 23 });
            stlist.Add(new Student { Id = 5, Name = "Bình", Age = 24 });

            //Ghi dữ liệu Json đã cập nhật vào file
            string updatedJson = JsonSerializer.Serialize(stlist, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, updatedJson);
            Console.WriteLine("\nUpdated data written to file.");

            //Đọc dữ liệu đã cập nhật dô file
            string finaljson = File.ReadAllText(filePath);
            Console.WriteLine("\nRead updated JSON from file:");
            Console.WriteLine(finaljson);

            //Deserialize chuỗi Json cuối cùng và hiện thị kết quả
            StudentList finalStudentList = JsonSerializer.Deserialize<StudentList>(finaljson);

            Console.WriteLine("\nFinal Students in the list:");
            foreach (Student student in finalStudentList.Students)
            {
                Console.WriteLine(student);
            }
            //// Bước 2: Serialize và ghi dữ liệu XML ra file
            //string filePath = "data.xml";
            //XmlSerializer xmlSerializer = new XmlSerializer(typeof(StudentList));
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    xmlSerializer.Serialize(writer, studentList);
            //}
            //Console.WriteLine("Dữ liệu đã được ghi vào file XML.");

            //// Bước 3: Đọc dữ liệu từ file vào chuỗi XML mới
            //string xmlData = File.ReadAllText(filePath);
            //Console.WriteLine("\nĐọc XML từ file:");
            //Console.WriteLine(xmlData);

            //// Bước 4: Deserialize chuỗi XML thành đối tượng StudentList
            //StudentList deserializedStudentList;
            //using (StreamReader reader = new StreamReader(filePath))
            //{
            //    deserializedStudentList = (StudentList)xmlSerializer.Deserialize(reader);
            //}

            //// Bước 5: Thêm sinh viên mới vào danh sách
            //deserializedStudentList.Add(new Student { Id = 4, Name = "Lan", Age = 23 });
            //deserializedStudentList.Add(new Student { Id = 5, Name = "Bình", Age = 24 });

            //// Bước 6: Serialize và ghi dữ liệu XML đã cập nhật vào file
            //using (StreamWriter writer = new StreamWriter(filePath))
            //{
            //    xmlSerializer.Serialize(writer, deserializedStudentList);
            //}
            //Console.WriteLine("\nDữ liệu đã được cập nhật và ghi vào file XML.");

            //// Bước 7: Đọc lại dữ liệu đã cập nhật từ file
            //string updatedXmlData = File.ReadAllText(filePath);
            //Console.WriteLine("\nĐọc dữ liệu XML đã cập nhật từ file:");
            //Console.WriteLine(updatedXmlData);

            //// Bước 8: Deserialize và hiển thị danh sách sinh viên cuối cùng
            //using (StreamReader reader = new StreamReader(filePath))
            //{
            //    deserializedStudentList = (StudentList)xmlSerializer.Deserialize(reader);
            //}

            //Console.WriteLine("\nDanh sách sinh viên cuối cùng:");
            //foreach (Student student in deserializedStudentList.Students)
            //{
            //    Console.WriteLine(student);
            //}
        }
    }
}
